using blazchat.Client.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace blazchat.Client.Pages
{
    public class MessagesBase : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }

        [Inject] 
        protected NavigationManager Navigation { get; set; }

        [Inject]
        HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            var userDto = new UserDto
            {
                Id = APAGAR.CurrentUserId,
                Name = "Matheus current"
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7076/api/user/createUser", userDto);
        }
    }
}