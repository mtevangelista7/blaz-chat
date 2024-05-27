using blazchat.Client.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace blazchat.Client.Pages
{
    public class MessagesPageBase : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }

        [Inject] 
        protected NavigationManager Navigation { get; set; }

        [Inject]
        HttpClient HttpClient { get; set; }
    }
}