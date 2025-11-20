using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace Blazor.Services
{
    public class ColorService
    {
        private readonly HttpClient _httpClient;

        public ColorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      
        public async Task<ResponseModel<List<ColorDto>>> GetColorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Color");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<ColorDto>>>();
                }
                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<ColorDto>> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ColorDto>> { Success = false, ErrorMassage = ex.Message };
            }
        }

    
        public async Task<ResponseModel<ColorDto>> GetColorByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Color/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<ColorDto>>();
                }
                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<ColorDto> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ColorDto> { Success = false, ErrorMassage = ex.Message };
            }
        }

    
        public async Task<ResponseModel<object>> CreateColorAsync(CreateColorDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Color", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }
                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = ex.Message };
            }
        }

        public async Task<ResponseModel<object>> UpdateColorAsync(UpdateColorDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Color/{dto.Id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }
                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = ex.Message };
            }
        }

        public async Task<ResponseModel<object>> DeleteColorAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Color/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }
                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object> { Success = false, ErrorMassage = ex.Message };
            }
        }
    }
}
