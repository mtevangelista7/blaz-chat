using blazchat.Client.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace blazchat.Client.Components;

public class OnlineChatBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Parameter] public Guid ChatId { get; set; }

    private HubConnection hubConnection;
    protected List<MessageDto> messages = [];
    protected string userInput = "Matheus";
    protected string messageInput;

    // APAGAR
    protected UserDto currentUser = new UserDto { Id = Guid.NewGuid(), Name = "Matheus" };

    protected override async Task OnInitializedAsync()
    {
        currentUser = new UserDto
        {
            Id = APAGAR.Id,
            Name = "Matheus"
        };

        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
            .Build();

        hubConnection.On<UserDto, string, DateTime>("ReceiveMessage", (user, text, timestamp) =>
        {
            messages.Add(new MessageDto { User = user, Text = text, Timestamp = timestamp });
            StateHasChanged();
        });

        await hubConnection.StartAsync();
    }

    protected async Task Send()
    {
        if (!string.IsNullOrEmpty(messageInput))
        {
            await hubConnection.SendAsync("SendMessage", ChatId, currentUser, messageInput, DateTime.Now);
            messageInput = string.Empty;
        }
    }
    
    protected bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;
}