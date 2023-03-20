using API.Helper;

namespace NeemApi.Helper
{
    public class FavoriteParams : PaginationParams
    {
        public string Username { get; set; }
        public string Category { get; set; } = "all";
        public string OrderBy { get; set; } = "new"; // {new, old, high, low}
        public int MinPrice { get; set; } = 10;
        public int MaxPrice { get; set; } = 9999;
    }
}