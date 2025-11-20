using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public CategoryService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider AuthStateProvider)
        {
            _httpClient = httpClient;
            _AuthStateProvider = AuthStateProvider;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<CategoryDto>>> GetCategoryByGenderAsync(int genderId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Category/categoriesByGender/{genderId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<CategoryDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<CategoryDto>>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Unknown error"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<CategoryDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Category");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<CategoryDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<CategoryDto>>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Unknown error"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<CategoryDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Category", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Unknown error"
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

        public async Task<ResponseModel<object>> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Category/{id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Unknown error"
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

        public async Task<ResponseModel<object>> DeleteCategoryAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Category/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Unknown error"
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
    }
}
