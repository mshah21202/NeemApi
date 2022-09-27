using NeemApi.DTOs;
using NeemApi.Entities;

namespace NeemApi.Interfaces
{
    public interface IFavoritesRepository
    {
        Task<bool> SaveAllAsync();
        Task<IEnumerable<ProductDto>> GetFavoritesForUser(string username);
        Task<UserFavorite> GetFavoriteUser(int userId, int productId);
    }
}
