using blazchat.Client.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace blazchat.Client.Components;

public class OnlineChatBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public HttpClient HttpClient { get; set; }
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

        // Validate if the user is authenticated
        var response = await HttpClient.GetAsync($"https://localhost:7076/api/chat/validate/chatId={ChatId}/userId={currentUser.Id}");

        if (!response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/");
        }

        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chatHub"))
            .Build();

        hubConnection.On<UserDto, string>("ReceiveMessage", (user, text) =>
        {
            messages.Add(new MessageDto { User = user, Text = text, Timestamp = DateTime.Now });

            StateHasChanged();
        });

        await hubConnection.StartAsync();
        await hubConnection.SendAsync("JoinChat", ChatId.ToString(), currentUser);
    }

    protected async Task SendMessage()
    {
        if (!string.IsNullOrEmpty(messageInput))
        {
            await hubConnection.SendAsync("SendMessage", ChatId.ToString(), currentUser, messageInput);
            messageInput = string.Empty;
        }
    }
}