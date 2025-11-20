using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Services
{
    public class GenderService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthenticationStateProvider AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public GenderService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider authStateProvider, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            AuthStateProvider = authStateProvider;
            _localStorage = localStorage;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<GenderDto>>> GetGendersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Gender");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<GenderDto>>>();
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                    return new ResponseModel<List<GenderDto>>
                    {
                        Success = false,
                        ErrorMassage = error?.ErrorMassage ?? "Unknown error"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GenderDto>>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateGenderAsync(CreateGenderDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Gender", dto);
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
                        ErrorMassage = error?.ErrorMassage ?? "Unknown error"
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

        public async Task<ResponseModel<object>> UpdateGenderAsync(int id, UpdateGenderDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Gender/{id}", dto);
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
                        ErrorMassage = error?.ErrorMassage ?? "Unknown error"
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

        public async Task<ResponseModel<object>> DeleteGenderAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Gender/{id}");
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
                        ErrorMassage = error?.ErrorMassage ?? "Unknown error"
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
