using System.Net.Http.Json;
using blazchat.Client.Components.Dialogs;
using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components;

public class NewMessageButtonBase : ComponentBase
{
    [Inject] IDialogService dialogService { get; set; }

    [Inject] HttpClient HttpClient { get; set; }

    [Inject] NavigationManager Navigation { get; set; }

    [Inject] protected IChatEndpoints chatEndpoints { get; set; }

    protected async Task OnClickStartNewChat()
    {
        var options = new DialogOptions
            { CloseOnEscapeKey = true, ClassBackground = "dialog-blur-backgroud", CloseButton = true };
        var dialogReference = await dialogService.ShowAsync<StartNewChatDialog>("Start new chat", options);

        var result = await dialogReference.Result;

        if (result.Canceled)
        {
            return;
        }

        await StartNewChat(result.Data as UserDto);
    }

    private async Task StartNewChat(UserDto? userDto)
    {
        CreateChatDto createChatDto = new()
        {
            User1Id = APAGAR.CurrentUserId,
            User2Id = userDto.Id
        };

        var newChatId = await chatEndpoints.CreateChat(createChatDto);
        Navigation.NavigateTo($"/messages/{newChatId}");
    }
}