
using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Services
{
    public class SizeService
    {
        private readonly HttpClient _httpClient;

        public SizeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<List<SizeDto>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Size");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<SizeDto>>>();

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<List<SizeDto>> { Success = false, ErrorMassage = error.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<SizeDto>> { Success = false, ErrorMassage = $"Connection error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<SizeDto>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Size/{id}");
                return await response.Content.ReadFromJsonAsync<ResponseModel<SizeDto>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<SizeDto> { Success = false, ErrorMassage = $"Connection error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> CreateAsync(CreateSizeDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Size", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = $"Error while adding: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> UpdateAsync(UpdateSizeDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Size/{dto.Id}", dto);
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = $"Error while updating: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Size/{id}");
                return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = $"Error while deleting: {ex.Message}" };
            }
        }
    }
}

