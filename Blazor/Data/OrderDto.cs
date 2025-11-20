namespace Blazor.Data
{
    public class OrderDto
    {

        public int Id { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public string ShippingCompany { get; set; }
        public string TrackingNumber { get; set; }

        public List<OrderDetailsDto> orderDetailsDtos { get; set; }
    }




    public class OrderDetailsDto
    {


        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SizeName { get; set; }

        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string Adress { get; set; }



    }




    public class UpdateShippingDto
    {
        public int OrderId { get; set; }
        public string ShippingCompany { get; set; }
        public string TrackingNumber { get; set; }

    }

    public class UpdatePaymentStatusDto
    {
        public int OrderId { get; set; }
        public bool PaymentStatus { get; set; }

    }

    public class UpdateStatusDto
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }

    }


    public class ResponseOrder<T>
    {
        public decimal TotalPrice { get; set; }
        public int TotalCountOrder { get; set; }
        public int Pending { get; set; }
        public int Processing { get; set; }
        public int Shipped { get; set; }
        public int Completed { get; set; }
        public int Canceled { get; set; }
        public int Returned { get; set; }



        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public bool Success { get; set; }
        public string ErrorMassage { get; set; }
        public T Data { get; set; }
    }


    public class RequestOrder
    {

        public int? OrderId { get; set; }
        public int? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingCompany { get; set; }
        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }


        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }











}
