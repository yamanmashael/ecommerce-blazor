namespace Blazor.Data
{
    public class ColorDto
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
     
    }
        
    public class CreateColorDto
    {
        public string ColorName { get; set; }
    }

    public class UpdateColorDto
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
    }

}
