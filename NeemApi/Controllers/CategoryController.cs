using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemApi.Data;

namespace NeemApi.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllCategories()
        {
            var categories = await _context.Categories.Select(c => c.Name).ToListAsync();
            return categories;
        }
    }
}
