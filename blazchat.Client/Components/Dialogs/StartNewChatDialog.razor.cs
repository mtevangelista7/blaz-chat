using blazchat.Client.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Components.Dialogs;

public class StartNewChatDialogBase : ComponentBase
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    protected List<ContactDto> Contacts { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Contacts = await GetContactsAsync();
    }
    
    private Task<List<ContactDto>> GetContactsAsync()
    {
        return Task.FromResult(new List<ContactDto>
        {
            new ContactDto { Id = Guid.Parse("CCE322A0-B0C0-4ACD-B9D5-0AD8B07C7E7F"), Name = "John Doe" },
            new ContactDto { Id = Guid.Parse("871FF7BD-A98D-438A-8766-52831D1867FE"), Name = "Jane Smith" },
            new ContactDto { Id = Guid.Parse("1FAEB3DB-7876-47B1-9709-AC288F603FA4"), Name = "Matheus" }
        });
    }
    
    protected void StartChat(ContactDto contact)
    {
        MudDialog.Close(DialogResult.Ok(contact));
    }
}
