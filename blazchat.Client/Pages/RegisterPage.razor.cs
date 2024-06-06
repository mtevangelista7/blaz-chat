using System.Net.Http.Json;
using blazchat.Application.DTOs;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Client.Services.Interfaces;
using blazchat.Client.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace blazchat.Client.Pages;

public class RegisterPageBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public IAuthService AuthService { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

    protected CreateUserDto user = new(Username: "", Password: "");
    private bool _isShow;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/messages");
        }
    }
    
    protected void OnClickLogin()
    {
        NavigationManager.NavigateTo("/login");
    }

    protected async Task OnClickRegister(EditContext editContext)
    {
        // Validate the form
        if (!editContext.Validate()) return;

        // Call the API to register the user
        var result = await AuthService.Register(user.Username, user.Password);

        if (!result.Flag)
        {
            Snackbar.Add("Invalid username or password", Severity.Error);
            return;
        }

        var customAuthenticationStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;

        await customAuthenticationStateProvider.UpdateAuthenticationStateAsync(result.Token);

        Snackbar.Add("Your account has been created", Severity.Success);

        NavigationManager.NavigateTo($"/messages");
    }

    protected void ShowPassword()
    {
        if (_isShow)
        {
            _isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            _isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
}