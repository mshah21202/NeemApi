using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeemApi.Data;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Interfaces;

namespace NeemApi.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly DataContext _context;

        public AdminController(UserManager<AppUser> userManager, IOrderRepository orderRepository, DataContext context)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _context = context;
        }

        [Authorize(Policy="RequireAdminRole")]
        [HttpPost("update-order-status")]
        public async Task<ActionResult> UpdateOrderState(UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetOrder(updateOrderDto.Id);
            Order.OrderState newState;
            switch (updateOrderDto.StateId)
            {
                case 0: newState = Order.OrderState.Processing; break;
                case 1: newState = Order.OrderState.Delivering; break;
                case 2: newState = Order.OrderState.Delivered; break;
                case 3: newState = Order.OrderState.Refunded; break;
                default: newState = Order.OrderState.Processing; break;
            }
            order.State = newState;

            _context.Orders.Update(order);

            if (await _context.SaveChangesAsync() > 0) return Ok();

            return BadRequest("Could not update order");
        }
    }
}
