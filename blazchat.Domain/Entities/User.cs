using System.Text.Json.Serialization;

namespace blazchat.Domain.Entities;

public class User : EntityBase
{
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    // Navigation properties
    [JsonIgnore] public ICollection<ChatUser> ChatUsers { get; set; }
}