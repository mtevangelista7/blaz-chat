namespace blazchat.Infra.Data.Repositories;

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

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the chat.", ex);
        }

        return chat;
    }

    public async Task<List<Chat>> GetActiveChatsAsync(Guid userId)
    {
        return await _context.ChatUsers
            .Where(x => x.UserId == userId)
            .Include(x => x.Chat)  // Inclui o chat
            .ThenInclude(c => c.ChatUsers)  // Inclui os usuários no chat
            .ThenInclude(cu => cu.User)  // Inclui os detalhes do usuário
            .Select(x => x.Chat)
            .ToListAsync();
    }
}