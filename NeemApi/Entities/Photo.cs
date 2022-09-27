namespace NeemApi.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int PublicId { get; set; }
        public Product Product { get; set; }
    }
}
