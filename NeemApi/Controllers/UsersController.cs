using Microsoft.AspNetCore.Mvc;
using NeemApi.Entities;
using NeemApi.Interfaces;

namespace NeemApi.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _userRepository.GetUsersAsync();
        }

        [HttpGet("{username}")]
        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }
    }
}
