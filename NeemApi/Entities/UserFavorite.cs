namespace NeemApi.Entities
{
    public class UserFavorite
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
