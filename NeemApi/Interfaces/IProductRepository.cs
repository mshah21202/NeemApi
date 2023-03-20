using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Helper;

namespace NeemApi.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product user);
        Task<bool> SaveAllAsync();
        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);
        Task<ProductDetailDto> GetProductDetailsByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategory(string category);

        Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(List<int> ids);
    }
}
