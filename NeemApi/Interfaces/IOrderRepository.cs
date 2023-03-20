using NeemApi.DTOs;
using NeemApi.Entities;

namespace NeemApi.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDto>> GetOrdersForUserId(int id);
        Task<Order> GetOrder(int id);
    }
}
