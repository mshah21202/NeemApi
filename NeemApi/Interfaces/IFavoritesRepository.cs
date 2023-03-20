using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Helper;

namespace NeemApi.Interfaces
{
    public interface IFavoritesRepository
    {
        Task<bool> SaveAllAsync();
        Task<PagedList<ProductDto>> GetFavoritesForUser(FavoriteParams favoriteParams);
        Task<UserFavorite> GetFavoriteUser(int userId, int productId);
    }
}
