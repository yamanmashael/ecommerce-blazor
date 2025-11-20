using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Services
{
    public class ProductItemService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public ProductItemService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider AuthStateProvider, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _AuthStateProvider = AuthStateProvider;
            _localStorage = localStorage;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<ProductItemDto>>> GetProductItemByProduct(int ProductId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductItem/{ProductId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<ProductItemDto>>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<ResponseModel<ProductItemDto>>();
                    return new ResponseModel<List<ProductItemDto>>
                    {
                        Success = false,
                        ErrorMassage = error.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ProductItemDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<ProductItemDto>> GetProductItemById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductItem/single/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<ProductItemDto>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<ProductItemDto>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<ProductItemDto>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel> CreateProductItemAsync(CreateProductItemDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/ProductItem", dto);
                if (response.IsSuccessStatusCode)
                {
                    return new BaseResponseModel { Success = true };
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new BaseResponseModel
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel> UpdateProductItemAsync(UpdateProductItemDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/ProductItem/{dto.Id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return new BaseResponseModel { Success = true };
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new BaseResponseModel
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel> DeleteProductItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ProductItem/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return new BaseResponseModel { Success = true };
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new BaseResponseModel
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }
    }
}
