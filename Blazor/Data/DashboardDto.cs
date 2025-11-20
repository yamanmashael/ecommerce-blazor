namespace Blazor.Data
{
        public class SalesSummaryDto
        {
            public string Period { get; set; }
            public int OrdersCount { get; set; }
            public decimal TotalSales { get; set; }
        }

    public class OrderStatusDashboardDto
    {
        public string StatusName { get; set; }
        public int OrderStatusCount { get; set; }

    }


    public class ProductDahboardDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }

    }


    // Models/ExchangeRateResponse.cs
    public class ExchangeRateResponse
    {
        public string Result { get; set; }
        public decimal ConversionRate { get; set; }
    }


}
