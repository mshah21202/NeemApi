namespace NeemApi.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public List<string> PhotoUrls { get; set; }
        public string Category { get; set; }
    }
}
