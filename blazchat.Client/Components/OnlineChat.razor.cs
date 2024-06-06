using blazchat.Application.DTOs;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace blazchat.Client.Components;

public class OnlineChatBase : ComponentBase, IDisposable
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IChatEndpoints ChatEndpoints { get; set; }
    [Inject] public IMessageEndpoints MessageEndpoints { get; set; }
    [Inject] public IUserEndpoints UserEndpoints { get; set; }
    [Inject] public IJSRuntime JSRuntime { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

    [Parameter] public Guid ChatIdParam { get; set; }

    private Task<AuthenticationState> _authenticationStateTask;

    protected UserDto currentUser;
    protected GuessUserDto guesstUser;

    private HubConnection hubConnection;
    protected List<MessageDto> messages = [];
    protected string messageInput;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        // if the user is not authenticated, redirect to the login page
        if (!user.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/login");
            return;
        }

        // get the current user object
        currentUser = await UserEndpoints.GetUser(Guid.Parse(user.FindFirst(c => c.Type == "id")?.Value));

        // if the current user is null, redirect to the login page
        if (currentUser is null)
        {
            NavigationManager.NavigateTo("/login");
            return;
        }

        // if the chat is not valid, redirect to the login page
        if (!await ValidateChat())
        {
            NavigationManager.NavigateTo("/messages");
            return;
        }

        // get the guess user
        guesstUser = await UserEndpoints.GetGuessUserByChatId(ChatIdParam, currentUser.Id);

        if (guesstUser is null)
        {
            NavigationManager.NavigateTo("/messages");
            return;
        }

        // connect to the hub and load the messages
        await OpenConnection();
        await LoadMessages();

        // scroll to the bottom of the chat
        StateHasChanged();
        await JSRuntime.InvokeVoidAsync("scrollToBottom", "scrollablePaper");
    }

    protected async Task SendMessage()
    {
        // send the message to the hub
        if (!string.IsNullOrEmpty(messageInput))
        {
            SendMessageDto sendMessageDto = new(ChatId: ChatIdParam, UserId: currentUser.Id, Message: messageInput);

            await hubConnection.SendAsync("SendMessage", sendMessageDto);
            messageInput = string.Empty;
        }
    }

    private async Task<bool> ValidateChat()
    {
        // create a new valitdate object and call the endpoint
        ValidateChatDto validateChat = new(ChatId: ChatIdParam, UserId: currentUser.Id);
        return await ChatEndpoints.ValidateChat(validateChat);
    }

    private async Task OpenConnection()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chatHub"))
            .Build();

        // receive the message from the hub
        hubConnection.On<Guid, string, DateTime>("ReceiveMessage", (userId, text, timestamp) =>
        {
            messages.Add(new MessageDto(UserId: userId, Text: text, Timestamp: timestamp));
            StateHasChanged();
        });

        // create a new object to join the chat
        UserJoinChatDto userJoinChatDto = new(ChatId: ChatIdParam, UserId: currentUser.Id);

        await hubConnection.StartAsync();
        await hubConnection.SendAsync("JoinChat", userJoinChatDto);
    }

    public async void Dispose()
    {
        await hubConnection.DisposeAsync();
    }

    private async Task LoadMessages()
    {
        messages = await MessageEndpoints.GetMessages(ChatIdParam);
        StateHasChanged();
    }

    protected void OnClickBack()
    {
        NavigationManager.NavigateTo("/messages");
    }
}