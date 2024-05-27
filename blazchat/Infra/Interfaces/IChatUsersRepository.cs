namespace blazchat.Infra.Interfaces
{
    public interface IChatUsersRepository
    {
        Task<bool> ValidateChatAsync(Guid chatId, Guid userId);
    }
}
