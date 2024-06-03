using blazchat.Domain.Entities;
using blazchat.Infra.Data.Context;
using blazchat.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Data.Repositories;

public class ChatRepository(AplicationDbContext context) : EFRepository<Chat>(context), IChatRepository
{
    public async Task<List<Chat>> GetActiveChatsAsync(Guid userId)
    {
        return await context.ChatUsers
            .Where(x => x.UserId == userId)
            .Include(x => x.Chat) // Inclui o chat
            .ThenInclude(c => c.ChatUsers) // Inclui os usuários no chat
            .ThenInclude(cu => cu.User) // Inclui os detalhes do usuário
            .Select(x => x.Chat)
            .ToListAsync();
    }
}