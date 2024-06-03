using System.Net.Http.Json;
using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace blazchat.Client.Pages;

public class RegisterPageBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IUserEndpoints userEndpoints { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }

    protected UserDto user = new();
    protected bool isShow;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected void OnClickLogin()
    {
        NavigationManager.NavigateTo("/login");
    }

    protected async Task OnClickRegister(EditContext editContext)
    {
        // Validate the form

        // Call the API to register the user
        var userCreated = await userEndpoints.CreateUser(user);
        var userId = userCreated.Id;

        // manter até montar o fluxo de login corretamente
        APAGAR.CurrentUserId = userId;

        // show a success message
        Snackbar.Add("Your account has been created", Severity.Success);

        // Navigate to the messages page
        NavigationManager.NavigateTo("/messages");
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