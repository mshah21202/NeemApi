using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Helper;
using NeemApi.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NeemApi.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDetailDto> GetProductDetailsByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Category).Include(x => x.Photos).FirstOrDefaultAsync(p=> p.Id == id);
            var result = _mapper.Map<ProductDetailDto>(product);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(List<int> ids)
        {
            List<ProductDto> products = new List<ProductDto>();
            foreach (int id in ids)
            {
                var product = _mapper.Map<ProductDto>(await _context.Products.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id));
                products.Add(product);
            }
            return products;
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams)
        {
            var products = _context.Products.Include(x => x.Photos).AsQueryable();
            //products = products.Where(x => x.Category.Name == productParams.Category);
            products = products.Where(x => x.Price >= productParams.MinPrice && x.Price <= productParams.MaxPrice);
            products = productParams.Category switch
            {
                "all" => products,
                _ => products.Where(p => p.Category.Name == productParams.Category.ToLower())
            };
            products = productParams.OrderBy switch
            {
                "new" => products.OrderBy(p => p.Id),
                "old" => products.OrderByDescending(p => p.Id),
                "high" => products.OrderByDescending(p => p.Price),
                "low" => products.OrderBy(p => p.Price),
                _ => products.OrderByDescending(p => p.Name)
            };
            var result = await PagedList<ProductDto>.CreateAsync(products.ProjectTo<ProductDto>(_mapper
                .ConfigurationProvider).AsNoTracking(), productParams.PageNumber, productParams.PageSize);

            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategory(string category)
        {
            var products = _context.Products.Include(x => x.Photos).Where(x => x.Category.Name == category).AsQueryable();
            var result = products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();
            return result;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
