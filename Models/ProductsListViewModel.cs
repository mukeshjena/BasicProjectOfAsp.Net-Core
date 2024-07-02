namespace PracticeForRevision.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
