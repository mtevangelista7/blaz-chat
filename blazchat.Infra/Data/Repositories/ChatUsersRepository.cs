using blazchat.Domain.Entities;
using blazchat.Infra.Data.Context;
using blazchat.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Data.Repositories
{
    public class ChatUsersRepository(AplicationDbContext context)
        : EFRepository<ChatUser>(context), IChatUsersRepository
    {
        public async Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser)
        {
            var chatUser =
                await context.ChatUsers.FirstOrDefaultAsync(cu => cu.ChatId == chatId && cu.UserId != currentUser);
            return chatUser?.UserId ?? Guid.Empty;
        }

        public async Task<bool> ValidateChatAsync(Guid chatId, Guid userId)
        {
            return await context.ChatUsers.AnyAsync(cu => cu.UserId == userId && cu.ChatId == chatId);
        }
    }
}