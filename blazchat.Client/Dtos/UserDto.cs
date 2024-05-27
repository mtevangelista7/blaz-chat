using System.ComponentModel.DataAnnotations;

namespace blazchat.Client.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; }
}