using blazchat.Application.DTOs;
using blazchat.Client.CustomComponentBase;
using blazchat.Client.Helper;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace blazchat.Client.Components;

public class OnlineChatBase : ComponentBaseExtends, IDisposable
{
    [Inject] private IChatEndpoints ChatEndpoints { get; set; }
    [Inject] private IMessageEndpoints MessageEndpoints { get; set; }
    [Inject] private IUserEndpoints UserEndpoints { get; set; }

    [Parameter] public Guid ChatIdParam { get; set; }

    private Task<AuthenticationState> _authenticationStateTask;

    protected UserDto CurrentUser;
    protected GuessUserDto GuesstUser;

    private HubConnection _hubConnection;
    protected List<MessageDto> Messages = [];
    protected string MessageInput;

    protected override async Task OnInitializedAsync()
    {
        try
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
            CurrentUser = await UserEndpoints.GetUser(Guid.Parse(user.FindFirst(c => c.Type == "id")?.Value));

            // if the current user is null, redirect to the login page
            if (CurrentUser is null)
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
            GuesstUser = await UserEndpoints.GetGuessUserByChatId(ChatIdParam, CurrentUser.Id);

            if (GuesstUser is null)
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
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }
    }

    protected async Task SendMessage()
    {
        try
        {
            // send the message to the hub
            if (!string.IsNullOrEmpty(MessageInput))
            {
                SendMessageDto sendMessageDto = new(ChatId: ChatIdParam, UserId: CurrentUser.Id, Message: MessageInput);

                await _hubConnection.SendAsync("SendMessage", sendMessageDto);
                MessageInput = string.Empty;
            }
        }
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }
    }

    private async Task<bool> ValidateChat()
    {
        // create a new validate object and call the endpoint
        ValidateChatDto validateChat = new(ChatId: ChatIdParam, UserId: CurrentUser.Id);
        return await ChatEndpoints.ValidateChat(validateChat);
    }

    private async Task OpenConnection()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chatHub"))
            .Build();

        // receive the message from the hub
        _hubConnection.On<Guid, string, DateTime>("ReceiveMessage", (userId, text, timestamp) =>
        {
            Messages.Add(new MessageDto(UserId: userId, Text: text, Timestamp: timestamp));
            StateHasChanged();
        });

        // create a new object to join the chat
        UserJoinChatDto userJoinChatDto = new(ChatId: ChatIdParam, UserId: CurrentUser.Id);

        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync("JoinChat", userJoinChatDto);
    }

    public async void Dispose()
    {
        await _hubConnection.DisposeAsync();
    }

    private async Task LoadMessages()
    {
        Messages = await MessageEndpoints.GetMessages(ChatIdParam);
        StateHasChanged();
    }

    protected void OnClickBack()
    {
        NavigationManager.NavigateTo("/messages");
    }
}