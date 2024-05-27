using blazchat.Context;
using blazchat.Entities;
using blazchat.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Repositories;

public class ChatRepository : IChatRepository
{
    protected AplicationDbContext _context;
    protected DbSet<Chat> _dbSet;

    public ChatRepository(AplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Chat>();
    }

    public async Task<Chat> GetChatAsync(Guid chatId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == chatId);
    }

    public async Task<Chat> CreateChatAsync(Guid user1Id, Guid user2Id)
    {
        // Verifique se os usuários existem no banco de dados
        var user1 = await _context.Users.FindAsync(user1Id);
        var user2 = await _context.Users.FindAsync(user2Id);

        if (user1 == null || user2 == null)
        {
            throw new Exception("One or both users do not exist.");
        }

        // Primeiro, crie o ID do Chat
        var chatId = Guid.NewGuid();

        var chat = new Chat
        {
            Id = chatId, // Atribua o ID do Chat
            ChatUsers = new List<ChatUser>
        {
            new ChatUser { ChatId = chatId, UserId = user1Id },
            new ChatUser { ChatId = chatId, UserId = user2Id }
        }
        };

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        return chat;
    }
}