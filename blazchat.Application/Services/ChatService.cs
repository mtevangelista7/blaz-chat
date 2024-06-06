using blazchat.Application.Interfaces.Services;
using blazchat.Domain.Entities;
using blazchat.Infra.Data.Interfaces;

namespace blazchat.Application.Services;

public class ChatService(
    IChatRepository chatRepository,
    IChatUsersRepository chatUsersRepository,
    IUserRepository userRepository)
    : IChatService
{
    public async Task<Guid> CreateNewChat(Guid user1Id, Guid user2Id)
    {
        var user1 = await userRepository.GetById(user1Id);
        var user2 = await userRepository.GetById(user2Id);

        if (user1 is null || user2 is null)
        {
            throw new Exception("One or both users do not exist.");
        }

        // TODO: Ver se consigo fazer isso direto no banco
        var chatId = Guid.NewGuid();

        var chat = new Chat
        {
            Id = chatId,
            ChatUsers = new List<ChatUser>
            {
                new() { ChatId = chatId, UserId = user1Id },
                new() { ChatId = chatId, UserId = user2Id }
            }
        };

        var createdChat = await chatRepository.Add(chat);
        return createdChat.Id;
    }
    
    public async Task<bool> ValidateChat(Guid chatId, Guid userId)
    {
        return await chatUsersRepository.ValidateChatAsync(chatId, userId);
    }

    public async Task<Chat> GetChat(Guid chatId)
    {
        return await chatRepository.GetById(chatId);
    }

    public async Task<List<Chat>> GetActiveChats(Guid userId)
    {
        var chats = await chatRepository.GetActiveChatsAsync(userId);
        return chats;
    }

    public Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser)
    {
        return chatUsersRepository.GetGuessUserByChatId(chatId, currentUser);
    }
}