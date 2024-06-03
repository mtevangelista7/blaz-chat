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

    [Parameter]
    public Guid CurrentUserId { get; set; } = Guid.Empty;

    protected List<UserDto> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Users = await GetContactsAsync();
        Users.Remove(Users.FirstOrDefault(u => u.Id.Equals(CurrentUserId)));
        Users = [.. Users.OrderBy(u => u.Name)];
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
