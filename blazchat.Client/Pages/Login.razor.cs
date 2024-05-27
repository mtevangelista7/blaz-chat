using blazchat.Client.Dtos;
using blazchat.Client.InterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazchat.Client.Pages;

public class LoginBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IUserEndpoints userEndpoints { get; set; }

    protected UserDto user = new();

    protected async Task OnClickLogin(EditContext context)
    {
        var userLogged = await userEndpoints.Login(user);

        if (userLogged == null)
        {
            return;
        }

        NavigationManager.NavigateTo($"/messages");
    }
}