﻿using blazchat.Application.DTOs;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Client.Services;
using blazchat.Client.Services.Interfaces;
using blazchat.Client.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace blazchat.Client.Pages;

public class LoginPageBase : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IAuthService AuthService { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
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

    protected async Task OnClickLogin(EditContext context)
    {
        if (!context.Validate()) return;

        var result = await AuthService.Login(user.Username, user.Password);

        if (!result.Flag)
        {
            Snackbar.Add("Invalid username or password", Severity.Error);
            return;
        }

        var customAuthenticationStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;

        await customAuthenticationStateProvider.UpdateAuthenticationStateAsync(result.Token);

        Snackbar.Add("You are logged in", Severity.Success);

        NavigationManager.NavigateTo($"/messages");
    }

    protected void OnClickRegister()
    {
        NavigationManager.NavigateTo("/register");
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