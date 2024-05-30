using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace blazchat.Client.Components;

public class OnlineChatBase : ComponentBase, IDisposable
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IChatEndpoints ChatEndpoints { get; set; }

    [Inject]
    public IMessageEndpoints MessageEndpoints { get; set; }

    [Inject]
    public IUserEndpoints UserEndpoints { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public Guid ChatId { get; set; }

    private HubConnection hubConnection;
    protected List<MessageDto> messages = [];
    protected string messageInput;

    // APAGAR
    protected UserDto currentUser = new();
    protected UserDto guesstUser = new();

    protected override async Task OnInitializedAsync()
    {
        currentUser = await UserEndpoints.GetUser(APAGAR.CurrentUserId);

        // if the chat is not valid, redirect to the login page
        if (!await ValidateChat())
        {
            NavigationManager.NavigateTo("/login");
            return;
        }

        // get the guess user
        guesstUser = await ChatEndpoints.GetGuessUserByChatId(ChatId, currentUser.Id);

        await OpenConnection();
        await LoadMessages();

        StateHasChanged();

        await JSRuntime.InvokeVoidAsync("scrollToBottom", "scrollablePaper");
    }

    protected async Task SendMessage()
    {
        // send the message to the hub
        if (!string.IsNullOrEmpty(messageInput))
        {
            await hubConnection.SendAsync("SendMessage", ChatId, currentUser, messageInput);
            messageInput = string.Empty;
        }
    }

    private async Task<bool> ValidateChat()
    {
        ValidateChatDto validateChat = new()
        {
            ChatId = ChatId,
            UserId = currentUser.Id
        };

        return await ChatEndpoints.ValidateChat(validateChat);
    }

    private async Task OpenConnection()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chatHub"))
            .Build();

        hubConnection.On<Guid, string, DateTime>("ReceiveMessage", (userId, text, timestamp) =>
        {
            messages.Add(new MessageDto { UserId = userId, Text = text, Timestamp = timestamp });
            StateHasChanged();
        });

        await hubConnection.StartAsync();
        await hubConnection.SendAsync("JoinChat", ChatId, currentUser);
    }

    public async void Dispose()
    {
        await hubConnection.DisposeAsync();
    }

    private async Task LoadMessages()
    {
        messages = await MessageEndpoints.GetMessages(ChatId);
        StateHasChanged();
    }

    protected void OnClickBack()
    {
        NavigationManager.NavigateTo("/messages");
    }
}