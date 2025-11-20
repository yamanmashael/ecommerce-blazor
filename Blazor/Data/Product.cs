using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blazor.Data
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryItemName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateProductDto
    {
        public int CategoryItemId { get; set; }
        public int BrandId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }

    public class UpdateProductDto
    {
        public int Id { get; set; }
        public int CategoryItemId { get; set; }
        public int BrandId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }





    public class FiltreDto
    {
        public IEnumerable<ProductFiltreDto> productFiltreDtos { get; set; }
        public IEnumerable<CategoryItemFiltrDto> CategoryItemFiltrDto { get; set; }
        public IEnumerable<CategoryFiltrDto> categoryFiltrDtos { get; set; }

    }

    public class CategoryFiltrDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryItemFiltrDto
    {
        public int Id { get; set; }
        public string CategoryItemName { get; set; }
    }


    public class ProductFiltreDto
    {
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string BarndName { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public decimal SalesPrice { get; set; }
        public List<ProductFiltrImageeDto> productFiltrImagees { get; set; }


    }

    public class ProductFiltrImageeDto
    {
        public string ImageFilename { get; set; }
    }



    public class SearchFilterDto
    {
        public List<int>? GenderIds { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? CategoryItemIds { get; set; }
        public List<int>? BrandIds { get; set; }
        public List<int>? SizeIds { get; set; }
        public List<int>? ColorIds { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Keyword { get; set; }
    }










}
