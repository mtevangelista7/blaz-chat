using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components.Dialogs.Shared;

public class AlertDialogBase : ComponentBase
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public string Message { get; set; } = string.Empty;

    protected void Close() => MudDialog.Close();
}