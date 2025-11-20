namespace Blazor.Data
{
    public class CategoryItemDto
    {
        public int Id { get; set; }
        public string CategoryItemName { get; set; }
        public string CategoryItemDescription { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // لعرض اسم القسم الأب
    }

    public class CreateCategoryItemDto
    {
        public string CategoryItemName { get; set; }
        public string CategoryItemDescription { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateCategoryItemDto
    {
        public string CategoryItemName { get; set; }
        public string CategoryItemDescription { get; set; }
        public int CategoryId { get; set; }
    }
}
