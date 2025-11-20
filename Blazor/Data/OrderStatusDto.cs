namespace Blazor.Data
{
    public class OrderStatusDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
    }

    public class CreateOrderStatusDto
    {
        public string StatusName { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
    }



}
