using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Helper;
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

        public async Task<PagedList<ProductDto>> GetFavoritesForUser(FavoriteParams favoriteParams)
        {
            var products = _context.Products.Include(x => x.Photos).AsQueryable();
            products = products.Where(x => x.UserFavorite.Any(u => u.User.UserName == favoriteParams.Username));
            //products = products.Where(x => x.Category.Name == productParams.Category);
            products = products.Where(x => x.Price >= favoriteParams.MinPrice && x.Price <= favoriteParams.MaxPrice);
            products = favoriteParams.Category switch
            {
                "all" => products,
                _ => products.Where(p => p.Category.Name == favoriteParams.Category.ToLower())
            };
            products = favoriteParams.OrderBy switch
            {
                "new" => products.OrderBy(p => p.Id),
                "old" => products.OrderByDescending(p => p.Id),
                "high" => products.OrderByDescending(p => p.Price),
                "low" => products.OrderBy(p => p.Price),
                _ => products.OrderByDescending(p => p.Name)
            };
            var result = await PagedList<ProductDto>.CreateAsync(products.ProjectTo<ProductDto>(_mapper
                .ConfigurationProvider).AsNoTracking(), favoriteParams.PageNumber, favoriteParams.PageSize);

            return result;
        }

        public async Task<UserFavorite> GetFavoriteUser(int userId, int productId)
        {
            var userFavorite = await _context.UserFavorite.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
            return userFavorite;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
