using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace blazchat.Client.Pages
{
    public class MessagesBase : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }
        
        [Inject]
        protected NavigationManager Navigation { get; set; }
    }
}
