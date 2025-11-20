using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;
        private readonly EventService _eventService;

        public OrderService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider AuthStateProvider, EventService eventService)
        {
            _httpClient = httpClient;
            _AuthStateProvider = AuthStateProvider;
            _authentication = authentication;
            _eventService = eventService;
        }

        public async Task<ResponseOrder<IEnumerable<OrderDto>>> GetOrdersAsync(RequestOrder request)
        {
            try
            {
                if (request.PageNumber == 0) request.PageNumber = 1;
                if (request.PageSize == 0) request.PageSize = 10;

                var query = $"api/Orders/admin?orderId={request.OrderId}&status={request.Status}" +
                            $"&paymentMethod={request.PaymentMethod}&shippingCompany={request.ShippingCompany}" +
                            $"&startDate={request.StartDate}&endDate={request.EndDate}" +
                            $"&pageNumber={request.PageNumber}&pageSize={request.PageSize}";

                var response = await _httpClient.GetAsync(query);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseOrder<IEnumerable<OrderDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseOrder<IEnumerable<OrderDto>>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseOrder<IEnumerable<OrderDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<List<OrderDto>>> GetOrderByUserId()
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.GetAsync("api/Orders/order");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<OrderDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<OrderDto>>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<OrderDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<OrderDetailsDto>>> GetOrderById(int id)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.GetAsync($"api/Orders/orderdatails/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<IEnumerable<OrderDetailsDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<IEnumerable<OrderDetailsDto>>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<OrderDetailsDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateOrderAsync(int AdressId)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.PostAsJsonAsync("api/Orders", AdressId);

                if (response.IsSuccessStatusCode)
                {
                    _eventService.RaiseOrderCreated();
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CancelOrder(int OrderId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Orders/cancelOrder", OrderId);

                if (response.IsSuccessStatusCode)
                {
                    _eventService.RaiseCancelOrder();
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> UpdateShippingAsync(int id, [FromBody] UpdateShippingDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Orders/UpdateShipping/{id}", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Error during update: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> UpdatePaymentStatusAsync(int id, [FromBody] UpdatePaymentStatusDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Orders/UpdatePaymentS/{id}", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Error during update: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> UpdateStatusAsync(int id, [FromBody] UpdateStatusDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Orders/UpdateStatus/{id}", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Error during update: {ex.Message}"
                };
            }
        }
    }
}
