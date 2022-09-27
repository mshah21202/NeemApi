using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Interfaces;
using SQLitePCL;

namespace NeemApi.Data
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FavoritesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetFavoritesForUser(string username)
        {
            var products = _context.Products.Where(p => p.UserFavorite.Any(x => x.User.UserName == username));
            var result = products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();
            return result;
        }

        public async Task<UserFavorite> GetFavoriteUser(int userId, int productId)
        {
            return await _context.UserFavorite.FindAsync(userId, productId);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
