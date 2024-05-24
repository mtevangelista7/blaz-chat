using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace blazchat.Client.Pages
{
    public class MessagesBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        private HubConnection hubConnection;
        private List<string> messages = [];
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);

                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        Task Send() => hubConnection.SendAsync("SendMessage", userInput, messageInput);

        protected bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;
    }
}
