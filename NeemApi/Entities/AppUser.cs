using Microsoft.AspNetCore.Identity;

namespace NeemApi.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public ICollection<UserFavorite> UserFavorite { get; set; }
        public int Pin { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
