namespace NeemApi.DTOs
{
    public class UpdateUserDto
    {
        public String Name { get; set; }
        public String Username { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String CurrentPassword { get; set; } = String.Empty;
        public String NewPassword { get; set; } = String.Empty;
    }
}
