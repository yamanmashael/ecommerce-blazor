namespace Blazor.Data
{
    public class BaseResponseModel
    {
        public bool Success { get; set; }
        public string ErrorMassage { get; set; }
        public Object Data { get; set; }
    }
}
