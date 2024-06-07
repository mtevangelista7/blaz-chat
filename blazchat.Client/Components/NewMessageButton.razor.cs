using System.Net.Http.Json;
using blazchat.Application.DTOs;
using blazchat.Client.Components.Dialogs;
using blazchat.Client.CustomComponentBase;
using blazchat.Client.Helper;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;

namespace blazchat.Client.Components;

public class NewMessageButtonBase : ComponentBaseExtends
{
    [Inject] IChatEndpoints ChatEndpoints { get; set; }
    [Parameter] public Guid CurrentUserId { get; set; } = Guid.Empty;

    protected async Task OnClickStartNewChat()
    {
        try
        {
            // dialog options 
            var options = new DialogOptions
                { CloseOnEscapeKey = true, ClassBackground = "dialog-blur-backgroud", CloseButton = true };

            // dialog parameters
            var parameters = new DialogParameters() { { "CurrentUserId", CurrentUserId } };

            // show dialog
            var dialogReference =
                await DialogService.ShowAsync<StartNewChatDialog>("Start new chat", parameters, options);

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
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }
    }

    private async Task StartNewChat(UserDto? userDto)
    {
        // create new chat with the current user and the selected guess user
        CreateChatDto createChatDto = new(User1Id: CurrentUserId, User2Id: userDto.Id);

        // create the chat with endpoint
        var newChatId = await ChatEndpoints.CreateChat(createChatDto);

        // navigate to the new chat
        NavigationManager.NavigateTo($"/messages/{newChatId}");
    }
}