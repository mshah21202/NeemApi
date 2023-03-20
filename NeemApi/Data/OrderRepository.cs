using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Interfaces;

namespace NeemApi.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersForUserId(int id)
        {
            var orders = _context.Orders.Where(o => o.UserId == id);
            return orders.ProjectTo<OrderDto>(_mapper.ConfigurationProvider).AsNoTracking();
        }
    }
}
