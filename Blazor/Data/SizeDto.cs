namespace Blazor.Data
{
    public class SizeDto
    {
        public int Id { get; set; }
        public string SizeNmae { get; set; }
    }

    public class CreateSizeDto
    {
        public string SizeNmae { get; set; }
    }

    public class UpdateSizeDto
    {
        public int Id { get; set; }
        public string SizeNmae { get; set; }
    }
}
