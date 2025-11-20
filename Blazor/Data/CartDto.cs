namespace Blazor.Data
{


    public class UpdateCartItemQuantityDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartDto
    {
        public int ProductItemId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
    }

 

    public class CartDto
    {
        public int CartId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItemDto> CartItemDto { get; set; }
    }
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductItemId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string BrandName { get; set; }
        public string SizeName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }


        public class LocalCartItem
        {
            public int cartItemId { get; set; }
            public int ProductItemId { get; set; }
            public int SizeId { get; set; }
            public int Quantity { get; set; }
        }

    public class CheckStoc
    {
        public int productItemId { get; set; }
        public int sizeId { get; set; }
    }




    public  class GuestCartItemDto
    {
        public int ProductItemId { get; set; }
        public int? SizeId { get; set; }
        public int Quantity { get; set; }
    }

    public  class CartMigrationRequest
    {
        public List<GuestCartItemDto> Items { get; set; } = new();
    }







}
