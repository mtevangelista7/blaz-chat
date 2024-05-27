using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components;

namespace blazchat.Client.Components;

public class MessagesHistoryBase : ComponentBase
{
    [Parameter]
    public Guid IdUser { get; set; } = Guid.Empty;

    [Inject]
    IChatEndpoints chatEndpoints { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    protected List<ChatDto> activeChats = [];

    protected override async Task OnInitializedAsync()
    {
        activeChats = await GetMessageHistory();
    }


    private async Task<List<ChatDto>> GetMessageHistory()
    {
        return await chatEndpoints.GetActiveChats(IdUser);
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