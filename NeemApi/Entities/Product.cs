namespace NeemApi.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public Category Category { get; set; }
        public ICollection<UserFavorite> UserFavorite { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }
    }
}
