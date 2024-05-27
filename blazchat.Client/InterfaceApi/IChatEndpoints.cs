using blazchat.Client.Dtos;
using Refit;

namespace blazchat.Client.InterfaceApi
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

        [Get("/api/chat/validate/chatId={chatId:guid}/userId={userId:guid}")]
        public Task<bool> ValidateChat(Guid chatId, Guid userId);

        [Get("/api/chat/getActiveChats/userId={userId}")]
        public Task<List<ChatDto>> GetActiveChats(Guid userId);
    }
}
