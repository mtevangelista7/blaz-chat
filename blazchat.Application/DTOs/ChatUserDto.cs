namespace blazchat.Application.DTOs
{
    public class ChatUserDto
    {
        public Guid ChatId { get; set; }
        public ChatDto Chat { get; set; }
        public Guid UserId { get; set; }
        public CreateUserDto User { get; set; }
    }
}