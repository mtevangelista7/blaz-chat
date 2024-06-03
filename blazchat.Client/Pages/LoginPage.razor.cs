using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace blazchat.Client.Pages;

public class LoginPageBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IUserEndpoints userEndpoints { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }

    protected UserDto user = new();
    protected bool isShow;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected async Task OnClickLogin(EditContext context)
    {
        var userLogged = await userEndpoints.Login(user);

        if (userLogged is null)
        {
            return;
        }

        // APAGAR
        APAGAR.CurrentUserId = userLogged.Id;

        Snackbar.Add("You are logged in", Severity.Success);
        NavigationManager.NavigateTo($"/messages");
    }

    protected void OnClickRegister()
    {
        NavigationManager.NavigateTo("/register");
    }

    protected void ShowPassword()
    {
        if (isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
}