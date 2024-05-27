using blazchat.Context;
using blazchat.Entities;
using blazchat.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Repositories
{
    public class ChatUsersRepository : IChatUsersRepository
    {
        protected AplicationDbContext _context;
        protected DbSet<ChatUser> _dbSet;

        public ChatUsersRepository(AplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<ChatUser>();
        }

        public async Task<bool> ValidateChatAsync(Guid chatId, Guid userId)
        {
            return await _dbSet.AnyAsync(cu => cu.UserId == userId && cu.ChatId == chatId);
        }
    }
}
