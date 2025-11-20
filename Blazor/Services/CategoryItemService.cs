using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Services
{
    public class CategoryItemService
    {
        private readonly HttpClient _httpClient;

        public CategoryItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<List<CategoryItemDto>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/CategoryItem");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<CategoryItemDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<List<CategoryItemDto>> { Success = false, ErrorMassage = error.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<CategoryItemDto>> { Success = false, ErrorMassage = $"Error during connection: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<List<CategoryItemDto>>> GetCategoryItemByCategoryAsync(int categoryId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/CategoryItem/CategoryItemByCategory/{categoryId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<CategoryItemDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<List<CategoryItemDto>> { Success = false, ErrorMassage = error.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<CategoryItemDto>> { Success = false, ErrorMassage = $"Error during connection: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> CreateAsync(CreateCategoryItemDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/CategoryItem", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = $"Error during creation: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> UpdateAsync(int id, UpdateCategoryItemDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/CategoryItem/{id}", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = $"Error during update: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/CategoryItem/{id}");
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = $"Error during deletion: {ex.Message}" };
            }
        }
    }
}
