using System.Text.Json.Serialization;

namespace blazchat.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public virtual ICollection<ChatUser> ChatUsers { get; set; }
}