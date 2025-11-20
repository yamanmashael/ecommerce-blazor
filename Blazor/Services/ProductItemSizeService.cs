using Blazor.Data;
using Blazor.Pages.Admin.Product;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Services
{
    public class ProductItemSizeService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public ProductItemSizeService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider AuthStateProvider, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _AuthStateProvider = AuthStateProvider;
            _localStorage = localStorage;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<ProductItemSizeDto>>> GetProductItemSizeByProductItem(int ProductItemId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductItemSize/{ProductItemId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<ProductItemSizeDto>>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<ResponseModel<ProductItemSizeDto>>();
                    return new ResponseModel<List<ProductItemSizeDto>>
                    {
                        Success = false,
                        ErrorMassage = error.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ProductItemSizeDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<ProductItemSizeDto>> GetProductItemSizeById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductItemSize/single/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<ProductItemSizeDto>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<ProductItemSizeDto>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<ProductItemSizeDto>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel> CreateProductItemSizeAsync(CreateProductItemSizeDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/ProductItemSize", dto);
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

        public async Task<BaseResponseModel> UpdateProductItemSizeAsync(UpdateProductItemSizeDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/ProductItemSize/{dto.Id}", dto);
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

        public async Task<BaseResponseModel> DeleteProductItemSizeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ProductItemSize/{id}");
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
