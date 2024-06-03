using System.Text.Json.Serialization;

namespace blazchat.Domain.Entities
{
    public class ChatUser : EntityBase
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        
        // Navigation properties
        [JsonIgnore] public User User { get; set; }
        [JsonIgnore] public Chat Chat { get; set; }
    }
}