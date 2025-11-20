namespace Blazor.Data
{

    public class GenderDto
    {
        public int Id { get; set; }
        public string GenderName { get; set; }
    }

    public class CreateGenderDto
    {
        public string GenderName { get; set; }
    }

    public class UpdateGenderDto
    {
        public string GenderName { get; set; }
    }


}
