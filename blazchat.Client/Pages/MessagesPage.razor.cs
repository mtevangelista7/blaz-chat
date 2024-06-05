using System.Security.Claims;
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
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider
                .GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                _claims = user.Claims;
                var nameIdent = user.FindFirst(c => c.Type == "id")?.Value;

                currentUserId = Guid.Parse(nameIdent);
            }
            else
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}