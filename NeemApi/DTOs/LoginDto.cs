using System.ComponentModel.DataAnnotations;

namespace NeemApi.DTOs
{
    public class LoginDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
