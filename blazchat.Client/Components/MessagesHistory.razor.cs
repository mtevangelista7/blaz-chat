using blazchat.Application.DTOs;
using blazchat.Client.CustomComponentBase;
using blazchat.Client.Helper;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazchat.Client.Components;

public class MessagesHistoryBase : ComponentBaseExtends
{
    [Inject] protected IChatEndpoints ChatEndpoints { get; set; }

    [Inject] protected IUserEndpoints UserEndpoints { get; set; }

    protected List<ChatDto> ActiveChats = [];
    protected Guid IdUser;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authState = await AuthStateProvider
                .GetAuthenticationStateAsync();

            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                var id = user.FindFirst(c => c.Type == "id")?.Value;
                var nameIdent = user.FindFirst(c => c.Type == "username")?.Value;

                IdUser = Guid.Parse(id);
            }
            else
            {
                NavigationManager.NavigateTo("/login");
            }

            ActiveChats = await GetMessageHistory();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }
    }

    private async Task<List<ChatDto>> GetMessageHistory()
    {
        List<ChatDto> chats = [];

        try
        {
            chats = await ChatEndpoints.GetActiveChats(IdUser);
        }
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }

        return chats;
    }

    protected async Task OnClickChat(Guid idChat)
    {
        try
        {
            if (idChat == Guid.Empty)
            {
                await Help.ShowAlertDialog(DialogService, "Error selecting chat, chat is null");
                return;
            }

            NavigationManager.NavigateTo($"/messages/{idChat}");
        }
        catch (Exception ex)
        {
            await Help.HandleError(DialogService, ex, this);
        }
    }
}