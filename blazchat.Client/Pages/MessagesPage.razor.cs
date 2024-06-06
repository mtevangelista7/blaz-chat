using System.Security.Claims;
using blazchat.Client.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazchat.Client.Pages
{
    public class MessagesPageBase : ComponentBase
    {
        [Parameter] public Guid Id { get; set; } = Guid.Empty;

        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        protected Guid currentUserId = Guid.Empty;
        protected string UserName = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider
                .GetAuthenticationStateAsync();

            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                var id = user.FindFirst(c => c.Type == "id")?.Value;
                var nameIdent = user.FindFirst(c => c.Type == "username")?.Value;

                currentUserId = Guid.Parse(id);
                UserName = nameIdent;
            }
            else
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}