using blazchat.Application.DTOs;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazchat.Client.Components;

public class MessagesHistoryBase : ComponentBase
{
    [Inject] protected IChatEndpoints ChatEndpoints { get; set; }

    [Inject] protected IUserEndpoints UserEndpoints { get; set; }

    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

    protected List<ChatDto> activeChats = [];
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


            activeChats = await GetMessageHistory();
            StateHasChanged();
        }
        catch (Exception err)
        {
            throw new Exception(err.Message);
        }
    }

    private async Task<List<ChatDto>> GetMessageHistory()
    {
        try
        {
            var chats = await ChatEndpoints.GetActiveChats(IdUser);
            return chats;
        }
        catch (Exception err)
        {
            throw new Exception(err.Message);
        }
    }

    protected void OnClickChat(Guid idChat)
    {
        if (idChat == Guid.Empty)
        {
            return;
        }

        NavigationManager.NavigateTo($"/messages/{idChat}");
    }
}