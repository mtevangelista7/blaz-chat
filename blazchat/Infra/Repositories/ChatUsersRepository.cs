using blazchat.Entities;
using blazchat.Infra.Context;
using blazchat.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Repositories
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

        public async Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser)
        {
            var chatUser = await _dbSet.FirstOrDefaultAsync(cu => cu.ChatId == chatId && cu.UserId != currentUser);
            return chatUser?.UserId ?? Guid.Empty;
        }

        public async Task<bool> ValidateChatAsync(Guid chatId, Guid userId)
        {
            return await _dbSet.AnyAsync(cu => cu.UserId == userId && cu.ChatId == chatId);
        }
    }
}
