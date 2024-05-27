using System.Net.Http.Json;
using blazchat.Client.Components.Dialogs;
using blazchat.Client.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components;

public class NewMessageBase : ComponentBase
{
    [Inject]
    IDialogService dialogService { get; set; }

    [Inject]
    HttpClient HttpClient { get; set; }

    [Inject]
    NavigationManager Navigation { get; set; }

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

        await StartNewChat(result.Data as ContactDto);
    }

    private async Task StartNewChat(ContactDto? contact)
    {
        var response = await HttpClient.PostAsJsonAsync("https://localhost:7076/api/chat/create", new { user1Id = APAGAR.CurrentUserId, user2Id = contact?.Id });
        response.EnsureSuccessStatusCode();
        var newChatId = await response.Content.ReadFromJsonAsync<Guid>();
        Navigation.NavigateTo($"/messages/{newChatId}");
    }
}