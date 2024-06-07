using blazchat.Application.DTOs;
using blazchat.Client.CustomComponentBase;
using blazchat.Client.Helper;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components.Dialogs;

public class StartNewChatDialogBase : ComponentBaseExtends
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Inject] IUserEndpoints UserEndpoints { get; set; }

    [Parameter] public Guid CurrentUserId { get; set; } = Guid.Empty;

    protected List<UserDto> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Users = await GetContactsAsync();
            Users.Remove(Users.FirstOrDefault(u => u.Id.Equals(CurrentUserId)) ??
                         throw new InvalidOperationException());
            Users = [.. Users.OrderBy(u => u.Username)];
        }
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }
    }

    private async Task<List<UserDto>> GetContactsAsync()
    {
        // get contacts from API
        return await UserEndpoints.GetUsers();
    }

    protected void StartChat(UserDto user)
    {
        MudDialog.Close(DialogResult.Ok(user));
    }
}