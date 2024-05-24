using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components.Dialogs;

public class StartNewChatDialogBase : ComponentBase
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    protected List<string> Contacts { get; set; } = [];

    protected override void OnInitialized()
    {
        Contacts = ["Alice", "Bob", "Charlie", "David", "Eve"];
    }
}
