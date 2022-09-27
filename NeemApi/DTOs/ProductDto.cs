using NeemApi.Entities;

namespace NeemApi.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string PhotoUrl { get; set; }
        public string Category { get; set; }
        public bool IsFavorite { get; set; }
    }
}
