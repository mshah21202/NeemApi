using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeemApi.Data;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Extensions;
using NeemApi.Interfaces;
using SQLitePCL;

namespace NeemApi.Controllers
{
    [Authorize]
    public class FavoriteController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFavoritesRepository _favoritesRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FavoriteController(DataContext context, IUserRepository userRepository, IFavoritesRepository favoritesRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _favoritesRepository = favoritesRepository;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetFavoriteProducts()
        {
            var products = await _favoritesRepository.GetFavoritesForUser(User.GetUsername());
            List<ProductDto> result = new List<ProductDto>();
            foreach (var product in products)
            {
                product.IsFavorite = true;
                result.Add(product);
            }
            return Ok(result);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> AddRemoveFavorite(int id)
        {
            var favorite = await _favoritesRepository.GetFavoriteUser(User.GetUserId(), id);

            if (favorite != null)
            {
                _context.UserFavorite.Remove(favorite);
            }
            else
            {
                favorite = new UserFavorite
                {
                    ProductId = id,
                    UserId = User.GetUserId()
                };

                await _context.UserFavorite.AddAsync(favorite);
            }

            if (await _favoritesRepository.SaveAllAsync()) return Ok();

            return BadRequest("Couldn't add to favorites");
        }
    }
}
