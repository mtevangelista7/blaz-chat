namespace blazchat.Client.Dtos
{
    public class ChatUserDto
    {
        public Guid ChatId { get; set; }
        public ChatDto Chat { get; set; }
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
    }
}
