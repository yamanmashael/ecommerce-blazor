namespace Blazor.Data
{
    public class RequestProduct
    {
        public int? genderId { get; set; }
        public int? categoryId { get; set; }
        public int? categoryItemId { get; set; }
        public int? BrandId { get; set; }
        public string? search { get; set; }



        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
    }
}
