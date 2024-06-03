using blazchat.Client.Entities;
using Refit;

namespace blazchat.Client.RefitInterfaceApi
{
    public interface IChatEndpoints
    {
        [Post("/api/chat/create")]
        public Task<Guid> CreateChat(Chat createChatDto);

        [Get("/api/chat/getChat/{id}")]
        public Task<Chat> GetChat(Guid id);

        [Get("/api/chat/getChats")]
        public Task<List<Chat>> GetChats();

        [Put("/api/chat/updateChat")]
        public Task UpdateChat(Chat chat);

        [Delete("/api/chat/deleteChat/{id}")]
        public Task DeleteChat(Guid id);

        [Post("/api/chat/validate")]
        public Task<bool> ValidateChat(ValidateChat validateChat);

        [Get("/api/chat/getActiveChats/{userId}")]
        public Task<List<Chat>> GetActiveChats(Guid userId);
    }
}