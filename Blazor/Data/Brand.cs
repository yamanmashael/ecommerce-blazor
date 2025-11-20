using System.ComponentModel.DataAnnotations;

namespace Blazor.Data
{
    //public class Brand
    //{
    //    public int Id { get; set; }
    //    public string BarndName { get; set; }
    //    public string BarndDescription { get; set; }

    //}

    public class BrandDto
    {
        public int Id { get; set; }
        public string BarndName { get; set; }
        public string BarndDescription { get; set; }
    }




    public class CreateBrandDto
    {

        [Required]
        public string BarndName { get; set; }
        public string BarndDescription { get; set; }
    }

    public class UpdateBrandDto
    {
        public int Id { get; set; }

        [Required]
        public string BarndName { get; set; }
        public string BarndDescription { get; set; }
    }
}
