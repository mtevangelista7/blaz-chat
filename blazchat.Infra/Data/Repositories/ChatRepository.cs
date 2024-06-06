using blazchat.Domain.Entities;
using blazchat.Infra.Data.Context;
using blazchat.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Data.Repositories;

public class ChatRepository(AplicationDbContext context) : EFRepository<Chat>(context), IChatRepository
{
    public async Task<List<Chat>> GetActiveChatsAsync(Guid userId)
    {
        return await context.Chats
            .Where(chat => chat.ChatUsers.Any(cu => cu.UserId == userId))
            .Include(chat => chat.ChatUsers)
            .ThenInclude(cu => cu.User)
            .ToListAsync();
    }
}