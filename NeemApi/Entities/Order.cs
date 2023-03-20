namespace NeemApi.Entities
{
    
    public class Order
    {
        public enum OrderState
        {
            Processing,
            Delivering,
            Delivered,
            Refunded
        }
        public int Id { get; set; }
        public float Total { get; set; }
        public OrderState State { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }
    }
}
