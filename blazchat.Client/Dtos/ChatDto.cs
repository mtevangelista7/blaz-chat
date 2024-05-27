namespace blazchat.Client.Dtos
{
    public class ChatDto
    {
        public Guid ChatId { get; set; }
        public UserDto User { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
