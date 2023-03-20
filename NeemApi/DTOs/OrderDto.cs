using NeemApi.Entities;

namespace NeemApi.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public float Total { get; set; }
        public ICollection<ProductDto> Products { get; set; }
        public List<int> Quantities { get; set; }
        public Order.OrderState State { get; set; }
    }
}
