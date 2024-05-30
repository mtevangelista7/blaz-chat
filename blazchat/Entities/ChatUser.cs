using System.Text.Json.Serialization;

namespace blazchat.Entities
{
    public class ChatUser
    {
        public Guid ChatId { get; set; }
        [JsonIgnore]
        public virtual Chat Chat { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
