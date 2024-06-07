using System.Security.Claims;
using blazchat.Client.CustomComponentBase;
using blazchat.Client.Helper;
using blazchat.Client.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazchat.Client.Pages
{
    public class MessagesPageBase : ComponentBaseExtends
    {
        [Parameter] public Guid Id { get; set; } = Guid.Empty;

        protected Guid CurrentUserId = Guid.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await AuthStateProvider
                    .GetAuthenticationStateAsync();

                var user = authState.User;

                if (user.Identity is not null && user.Identity.IsAuthenticated)
                {
                    var id = user.FindFirst(c => c.Type == "id")?.Value;
                    CurrentUserId = Guid.Parse(id);
                }
                else
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
            catch (Exception ex)
            {
                await Help.HandleError(DialogService, ex, this);
            }
        }
    }
}