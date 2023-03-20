using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using NeemApi.Data;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Extensions;
using NeemApi.Helper;
using NeemApi.Interfaces;

namespace NeemApi.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly DataContext _context;
        private readonly IFavoritesRepository _favoriteRepository;

        public ProductController(IProductRepository productRepository, DataContext context, IFavoritesRepository favoritesRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _favoriteRepository = favoritesRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts([FromQuery]ProductParams productParams)
        {
            var products = await _productRepository.GetProductsAsync(productParams);
            List<ProductDto> result = new List<ProductDto>();
            if (User.Identity.IsAuthenticated)
            {
                FavoriteParams favoriteParams = new FavoriteParams
                { 
                    Username = User.GetUsername()
                };
                var favorites = await _favoriteRepository.GetFavoritesForUser(favoriteParams);
                foreach(var p in products)
                {
                    if (favorites.Any(f => f.Id == p.Id))
                    {
                        p.IsFavorite = true;
                    }
                }
            }

            result.AddRange(products);

            Response.AddPaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ProductDetailDto> GetProductById(int id)
        {
            var product = await _productRepository.GetProductDetailsByIdAsync(id);
            if (User.Identity.IsAuthenticated)
            {
                FavoriteParams favoriteParams = new FavoriteParams
                {
                    Username = User.GetUsername()
                };
                var favorites = await _favoriteRepository.GetFavoritesForUser(favoriteParams);
                if (favorites.Any(f => f.Id == product.Id))
                {
                    product.IsFavorite = true;
                }
            }
            return product;
        }

        [HttpGet("cart-products")]
        public async Task<IEnumerable<ProductDto>> GetProductsByIds([FromQuery]List<int> id)
        {
            var products = await _productRepository.GetProductsByIdsAsync(id);
            
            return products;
        }

        //[HttpGet("category/{category}")]
        //public async Task<IEnumerable<ProductDto>> GetProductsByCategory(string category)
        //{
        //    var products = await _productRepository.GetProductsByCategory(category);
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var favorites = await _favoriteRepository.GetFavoritesForUser(User.GetUsername());
        //        foreach (var p in products)
        //        {
        //            if (favorites.Any(f => f.Id == p.Id))
        //            {
        //                p.IsFavorite = true;
        //            }
        //        }
        //    }
        //    return products;
        //}

        [HttpPost("create")]
        public async Task<ActionResult<List<ProductDto>>> CreateProducts(List<CreateProductDto> productDtos)
        {
            foreach (var productDto in productDtos)
            {
                var category = _context.Categories.FirstOrDefault(x => x.Name == productDto.Category);
                if (category == null)
                { 
                    category = new Category { Name = productDto.Category };
                    _context.Categories.Add(category);
                }

                var photos = new List<Photo>();
                foreach (var url in productDto.PhotoUrls)
                {
                    var photo = new Photo
                    {
                        Url = url,
                    };
                    photos.Add(photo);
                }
                var product = new Product
                {
                    Name = productDto.Name,
                    Category = category,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Photos = photos
                };
                await _context.Products.AddAsync(product);
            }
            if (await _context.SaveChangesAsync() > 0) return Ok("Created products successfully");
            return BadRequest("Couldn't Create products");
        }
    }
}
