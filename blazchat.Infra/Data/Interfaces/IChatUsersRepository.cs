namespace blazchat.Infra.Data.Interfaces
{
    public interface IChatUsersRepository
    {
        Task<bool> ValidateChatAsync(Guid chatId, Guid userId);
        Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser);
    }
}
