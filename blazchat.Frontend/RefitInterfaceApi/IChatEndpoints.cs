using blazchat.Client.Dtos;
using Refit;

namespace blazchat.Client.RefitInterfaceApi
{
    public interface IChatEndpoints
    {
        [Post("/api/chat/create")]
        public Task<Guid> CreateChat(CreateChatDto createChatDto);

        [Get("/api/chat/getChat/{id}")]
        public Task<ChatDto> GetChat(Guid id);

        [Get("/api/chat/getChats")]
        public Task<List<ChatDto>> GetChats();

        [Put("/api/chat/updateChat")]
        public Task UpdateChat(ChatDto chat);

        [Delete("/api/chat/deleteChat/{id}")]
        public Task DeleteChat(Guid id);

        [Post("/api/chat/validate")]
        public Task<bool> ValidateChat(ValidateChatDto validateChat);

        [Get("/api/chat/getActiveChats/{userId}")]
        public Task<List<ChatDto>> GetActiveChats(Guid userId);

        [Get("/api/chat/getGuessUserByChatId/{chatId}/{currentUser}")]
        public Task<UserDto> GetGuessUserByChatId(Guid chatId, Guid currentUser);
    }
}
