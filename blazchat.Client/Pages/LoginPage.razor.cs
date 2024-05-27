using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazchat.Client.Pages;

public class LoginPageBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IUserEndpoints userEndpoints { get; set; }

    protected UserDto user = new();

    protected async Task OnClickLogin(EditContext context)
    {
        var userLogged = await userEndpoints.Login(user);

        if (userLogged is null)
        {
            return;
        }

        // APAGAR
        APAGAR.CurrentUserId = userLogged.Id;

        NavigationManager.NavigateTo($"/messages");
    }

    protected void OnClickRegister()
    {
        NavigationManager.NavigateTo("/register");
    }
}