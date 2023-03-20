using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemApi.Data;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Extensions;
using NeemApi.Interfaces;

namespace NeemApi.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IOrderRepository _orderRepositor;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderController(DataContext context, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository, IMapper mapper)
        {
            _context = context;
            _orderRepositor = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var id = User.GetUserId();
            var orders = await _orderRepositor.GetOrdersForUserId(id);
            List<OrderDto> result = new List<OrderDto>();
            foreach (var order in orders)
            {
                var p = (await _context.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync()).Where(p => order.Products.Any(x => p.Id == x.Id));
                order.Products = p.ToList();
                order.Quantities = await _context.OrderProduct.Where(p => p.OrderId == order.Id).Select(p => p.ProductQuantity).ToListAsync();
                result.Add(order);
            }
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            float total = 0;
            Order order = new Order
            {
                UserId = User.GetUserId()
            };

            _context.Orders.Add(order);

            List<int> quantities = new List<int>();

            foreach (var productDto in createOrderDto.Products)
            {
                var product = await _context.Products.FindAsync(productDto.Id);
                OrderProduct orderProduct = new OrderProduct
                {
                    Order = order,
                    Product = product,
                    ProductQuantity = productDto.Quantity,
                };
                quantities.Add(orderProduct.ProductQuantity);
                total = total + product.Price * (float)productDto.Quantity;
                await _context.OrderProduct.AddAsync(orderProduct);
            }
            order.Total = total;

            if (await _context.SaveChangesAsync() > 0)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                var p = (await _context.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync()).Where(p => orderDto.Products.Any(x => p.Id == x.Id));
                orderDto.Products = p.ToList();
                orderDto.Quantities = quantities;
                return Ok(orderDto);
            }

            return BadRequest("Couldn't create orders");
        }
    }
}
