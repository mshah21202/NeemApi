using API.Helper;

namespace NeemApi.Helper
{
    public class ProductParams : PaginationParams
    {
        public string Category { get; set; } = "all";
        public string OrderBy { get; set; } = "new";
        public int MinPrice { get; set; } = 10;
        public int MaxPrice { get; set; } = 9999;

    }
}
