using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;

namespace blazchat.Client.Components;

public class MessagesHistoryBase : ComponentBase
{
    [Parameter]
    public Guid IdUser { get; set; } = Guid.Empty;

    [Inject]
    protected IChatEndpoints chatEndpoints { get; set; }

    [Inject]
    protected IUserEndpoints userEndpoints { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    protected List<ChatDto> activeChats = [];
    protected UserDto guesstUser = new();

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
            var chats = await chatEndpoints.GetActiveChats(IdUser);

            var tasks = chats.Select(async chat =>
            {
                var guessUser = chat.ChatUsers.FirstOrDefault(x => x.UserId != IdUser);

                if (guessUser != null)
                {
                    chat.GuessUser = await userEndpoints.GetUser(guessUser.UserId);
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