using Blazor.Data;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class DashboardService
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://your-exchange-rate-api.com/v6/latest/USD/TRY";

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<SalesSummaryDto>> GetSalesAsync(string periodType, DateTime startDate)
        {
            var url = $"api/Dashboard/sales?periodType={periodType}&startDate={startDate:O}";
            var data = await _httpClient.GetFromJsonAsync<List<SalesSummaryDto>>(url);

            return data ?? new List<SalesSummaryDto>();
        }





        public async Task<List<OrderStatusDashboardDto>> GetOrderStatus()
        {
          
            var data = await _httpClient.GetFromJsonAsync<List<OrderStatusDashboardDto>>("api/Dashboard/order");

            return data ?? new List<OrderStatusDashboardDto>();
        }




        public async Task<int> NewCustomersCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/dashboard/customer");
            return result;
        }



        public async Task<List<ProductDahboardDto>> GetPopularProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductDahboardDto>>("api/dashboard/product");
            return response ?? new List<ProductDahboardDto>();
        }


        public async Task<decimal> GetUsdToTryRateAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(ApiUrl);

                if (response != null && response.Result == "success")
                {
                    return response.ConversionRate;
                }
                return 32.00m;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching exchange rate: {ex.Message}");
                return 32.00m; 
            }
        }










    }

 
}
