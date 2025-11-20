namespace Blazor.Data
{
    public class ProductItemSizeDto
    {
        public int Id { get; set; }
        public int PrductItemId { get; set; }
        public int Stock { get; set; }

        public int SizeId { get; set; }
        public string SizeName { get; set; }
    }



    // للإنشاء
    public class CreateProductItemSizeDto
    {
        public int PrductItemId { get; set; }
        public int Stock { get; set; }
        public int SizeId { get; set; }
    }

    // للتعديل
    public class UpdateProductItemSizeDto
    {
        public int Id { get; set; }
        public int PrductItemId { get; set; }
        public int Stock { get; set; }
        public int SizeId { get; set; }
    }


}
