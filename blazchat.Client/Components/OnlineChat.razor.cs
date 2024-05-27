using blazchat.Client.Dtos;
using blazchat.Client.InterfaceApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace blazchat.Client.Components;

public class OnlineChatBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public HttpClient HttpClient { get; set; }
    [Inject] public IChatEndpoints ChatEndpoints { get; set; }

    [Parameter] public Guid ChatId { get; set; }

    private HubConnection hubConnection;
    protected List<MessageDto> messages = [];
    protected string messageInput;

    // APAGAR
    protected UserDto currentUser;

    protected override async Task OnInitializedAsync()
    {
        // APAGAR
        currentUser = new UserDto
        {
            Id = APAGAR.CurrentUserId,
            Name = "Matheus"
        };

        // validate if the chat is valid
        var isValid = await ChatEndpoints.ValidateChat(ChatId, currentUser.Id);

        // if the chat is not valid, redirect to the login page
        if (!isValid)
        {
            NavigationManager.NavigateTo("/login");
        }

        // open connection with the chat hub
        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chatHub"))
            .Build();

        // receive messages from the hub
        hubConnection.On<UserDto, string>("ReceiveMessage", (user, text) =>
        {
            messages.Add(new MessageDto { User = user, Text = text, Timestamp = DateTime.Now });
            StateHasChanged();
        });

        // start the connection
        await hubConnection.StartAsync();

        // join the chat with the current user
        await hubConnection.SendAsync("JoinChat", ChatId.ToString(), currentUser);
    }

    protected async Task SendMessage()
    {
        // send the message to the hub
        if (!string.IsNullOrEmpty(messageInput))
        {
            await hubConnection.SendAsync("SendMessage", ChatId.ToString(), currentUser, messageInput);
            messageInput = string.Empty;
        }
    }
}