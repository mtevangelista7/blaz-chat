using blazchat.Domain.Entities;

namespace blazchat.Infra.Data.Interfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<List<Chat>> GetActiveChatsAsync(Guid userId);
}