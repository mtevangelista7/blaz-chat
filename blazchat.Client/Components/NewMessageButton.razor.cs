using System.Net.Http.Json;
using blazchat.Application.DTOs;
using blazchat.Client.Components.Dialogs;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace blazchat.Client.Components;

public class NewMessageButtonBase : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IChatEndpoints ChatEndpoints { get; set; }
    [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    private Task<AuthenticationState> _authenticationStateTask;
    private Guid CurrentUserId;

    protected override async Task OnInitializedAsync()
    {
        _authenticationStateTask = AuthStateProvider.GetAuthenticationStateAsync();

        var user = _authenticationStateTask.Result.User;

        if (!user.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/login");
            return;
        }

        CurrentUserId = Guid.Parse(user.FindFirst("id").Value);
    }


    protected async Task OnClickStartNewChat()
    {
        // dialog options 
        var options = new DialogOptions
            { CloseOnEscapeKey = true, ClassBackground = "dialog-blur-backgroud", CloseButton = true };

        // dialog parameters
        var parameters = new DialogParameters() { { "CurrentUserId", CurrentUserId } };

        // show dialog
        var dialogReference = await DialogService.ShowAsync<StartNewChatDialog>("Start new chat", parameters, options);

        // get dialog result
        var result = await dialogReference.Result;

        // check if dialog was canceled
        if (result.Canceled)
        {
            return;
        }

        // start new chat with result from dialog
        await StartNewChat(result.Data as UserDto);
    }

    private async Task StartNewChat(UserDto? userDto)
    {
        // create new chat with the current user and the selected guess user
        CreateChatDto createChatDto = new(User1Id: CurrentUserId, User2Id: userDto.Id);

        // create the chat with endpoint
        var newChatId = await ChatEndpoints.CreateChat(createChatDto);

        // navigate to the new chat
        Navigation.NavigateTo($"/messages/{newChatId}");
    }
}