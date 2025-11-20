using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Services
{
    public class OrderStatusService
    {

        private readonly HttpClient _httpClient;

        public OrderStatusService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ResponseModel<List<OrderStatusDto>>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<ResponseModel<List<OrderStatusDto>>>("api/OrderStatus");
        }

        public async Task<ResponseModel<OrderStatusDto>> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseModel<OrderStatusDto>>($"api/OrderStatus/{id}");
        }

        public async Task<ResponseModel<OrderStatusDto>> CreateAsync(CreateOrderStatusDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/OrderStatus", dto);
            return await response.Content.ReadFromJsonAsync<ResponseModel<OrderStatusDto>>();
        }

        public async Task<ResponseModel<OrderStatusDto>> UpdateAsync(UpdateOrderStatusDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/OrderStatus/{dto.Id}", dto);
            return await response.Content.ReadFromJsonAsync<ResponseModel<OrderStatusDto>>();
        }

        public async Task<ResponseModel<object>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/OrderStatus/{id}");
            return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
        }


    }
}
   