namespace Blazor.Data
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public List<ProductItemDetailsDto> ProductItems { get; set; }
    }

    public class ProductItemDetailsDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public string ColorName { get; set; }
        public string ProductCode { get; set; }
        public List<string> ImageFilenames { get; set; }
        public List<ProductSizeDetailsDto> Sizes { get; set; }
    }

    public class ProductSizeDetailsDto
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int Stock { get; set; }
    }
}
