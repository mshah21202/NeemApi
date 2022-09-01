using System.ComponentModel.DataAnnotations;

namespace NeemApi.DTOs
{
    public class RegisterDto
    {
        [Required] public string Email { get; set; }
        [Required][StringLength(16, MinimumLength = 8)] public string Password { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Phone { get; set; }
    }
}
