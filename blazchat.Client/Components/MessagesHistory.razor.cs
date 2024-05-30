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
    NavigationManager NavigationManager { get; set; }

    protected List<ChatDto> activeChats = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            activeChats = await GetMessageHistory();
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
            return await chatEndpoints.GetActiveChats(IdUser);
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