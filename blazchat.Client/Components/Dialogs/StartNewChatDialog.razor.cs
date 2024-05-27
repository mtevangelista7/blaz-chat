using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components.Dialogs;

public class StartNewChatDialogBase : ComponentBase
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Inject]
    protected IUserEndpoints userEndpoints { get; set; }

    protected List<UserDto> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Users = await GetContactsAsync();
    }
    
    private async Task<List<UserDto>> GetContactsAsync()
    {
        // get contacts from API
        return await userEndpoints.GetUsers();
    }
    
    protected void StartChat(UserDto user)
    {
        MudDialog.Close(DialogResult.Ok(user));
    }
}
