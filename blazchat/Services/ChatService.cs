using blazchat.Entities;
using blazchat.Infra.Interfaces;
using blazchat.Services.Interfaces;

namespace blazchat.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatUsersRepository _chatUsersRepository;

    public ChatService(IChatRepository chatRepository, IChatUsersRepository chatUsersRepository)
    {
        _chatRepository = chatRepository;
        _chatUsersRepository = chatUsersRepository;
    }

    public async Task<Guid> CreateNewChat(Guid user1Id, Guid user2Id)
    {
        var createdChat = await _chatRepository.CreateChatAsync(user1Id, user2Id);
        return createdChat.Id;
    }


    public async Task<bool> ValidateChat(Guid chatId, Guid userId)
    {
        return await _chatUsersRepository.ValidateChatAsync(chatId, userId);
    }

    public async Task<Chat> GetChat(Guid chatId)
    {
        return await _chatRepository.GetChatAsync(chatId);
    }

    public async Task<List<Chat>> GetActiveChats(Guid userId)
    {
        return await _chatRepository.GetActiveChatsAsync(userId);
    }
}