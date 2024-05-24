using blazchat.Entities;

namespace blazchat.Services;

public class ChatService
{
    private readonly List<Chat> chats = new();

    public Guid CreateNewChat(Guid user1Id, Guid user2Id)
    {
        var chatId = Guid.NewGuid();
        var chat = new Chat
        {
            ChatId = chatId,
            User1Id = user1Id,
            User2Id = user2Id
        };
        chats.Add(chat);
        return chatId;
    }

    public bool ValidateChat(Guid chatId, Guid userId)
    {
        var chat = chats.FirstOrDefault(c => c.ChatId == chatId);
        return chat != null && (chat.User1Id == userId || chat.User2Id == userId);
    }
}