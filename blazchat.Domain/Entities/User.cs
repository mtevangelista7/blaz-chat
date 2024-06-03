using System.Text.Json.Serialization;

namespace blazchat.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; } // TODO: change to username
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    // Navigation properties
    [JsonIgnore] public ICollection<ChatUser> ChatUsers { get; set; }
}