using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.Data
{


    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ProductImageCreateDto
    {
        public int ProductItemId { get; set; }
        public IBrowserFile ImageFile { get; set; }
    }
}
