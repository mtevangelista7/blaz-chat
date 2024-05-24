using blazchat.Client.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components;

public class NewMessageBase : ComponentBase
{
    [Inject]
    IDialogService dialogService { get; set; }

    protected async Task OnClickStartNewChat()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, ClassBackground = "dialog-blur-backgroud", CloseButton = true };
        await dialogService.ShowAsync<StartNewChatDialog>("Start new chat", options);
    }
}