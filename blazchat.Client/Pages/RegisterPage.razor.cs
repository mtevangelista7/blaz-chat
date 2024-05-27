using System.Net.Http.Json;
using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazchat.Client.Pages;

public class RegisterPageBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Inject] HttpClient HttpClient { get; set; }

    [Inject] public IUserEndpoints userEndpoints { get; set; }

    protected UserDto user = new UserDto();

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
        NavigationManager.NavigateTo("/messages");
    }
}