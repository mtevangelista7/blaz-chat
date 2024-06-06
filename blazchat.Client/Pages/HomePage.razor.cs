using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazchat.Client.Pages;

public class HomePageBase : ComponentBase
{
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/messages");
        }
        else
        {
            NavigationManager.NavigateTo("/login");
        }
    }
}