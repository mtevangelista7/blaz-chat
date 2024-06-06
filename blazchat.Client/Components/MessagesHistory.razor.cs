using blazchat.Application.DTOs;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;

namespace blazchat.Client.Components;

public class MessagesHistoryBase : ComponentBase
{
    [Parameter] public Guid IdUser { get; set; } = Guid.Empty;

    [Inject] protected IChatEndpoints ChatEndpoints { get; set; }

    [Inject] protected IUserEndpoints UserEndpoints { get; set; }

    [Inject] NavigationManager NavigationManager { get; set; }

    protected List<ChatDto> activeChats = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
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

            // TODO: isso aqui não ta legal, tenta puxar todos os dados de uma vez e se não der certo, muda a lógica
            var tasks = chats.Select(async chat =>
            {
                var guessUserId = chat.ChatUsers.FirstOrDefault(x => x.UserId != IdUser).UserId;

                if (!guessUserId.Equals(Guid.Empty))
                {
                    chat.GuessUser = await UserEndpoints.GetGuessUserByChatId(chat.Id, guessUserId);
                }
            }).ToArray();

            await Task.WhenAll(tasks);

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