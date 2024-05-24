using Microsoft.AspNetCore.Components;

namespace blazchat.Client.Components;

public class MessagesHistoryBase : ComponentBase
{
    protected List<string> Messages { get; set; } = new();
    
    protected override void OnInitialized()
    {
        Messages.Add("Hello, World!");
        Messages.Add("Welcome to BlazChat!");
        Messages.Add("This is a simple chat application.");
        Messages.Add("You can send messages and they will be displayed here.");
        Messages.Add("Have fun!");
    }
}