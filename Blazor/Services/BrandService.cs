
using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class BrandService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthenticationStateProvider AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public BrandService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider authStateProvider, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            AuthStateProvider = authStateProvider;
            _localStorage = localStorage;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<BrandDto>>> GetBrandsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Brand");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<BrandDto>>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<List<BrandDto>>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage ?? "An error occurred while fetching data."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<BrandDto>>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<BrandDto>> GetBrandByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Brand/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<BrandDto>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<BrandDto>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage ?? "An error occurred while fetching brand data."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<BrandDto>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateBrandAsync(CreateBrandDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Brand", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<object>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage ?? "Failed to create brand."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> UpdateBrandAsync(int id, UpdateBrandDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Brand/{id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<object>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage ?? "Failed to update brand."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> DeleteBrandAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Brand/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<object>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage ?? "Failed to delete brand."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }
    }
}
