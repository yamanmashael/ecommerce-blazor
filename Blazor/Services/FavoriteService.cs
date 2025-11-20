using Blazor.Authentication;
using Blazor.Data;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class FavoriteService
    {
        private readonly HttpClient _httpClient;
        private readonly Authentication _authentication;

        public FavoriteService(Authentication authentication, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<FavoriteDto>>> GetFavoriteAsync()
        {
            try
            {
                var isLoggedIn = await _authentication.SetAuthorizeHeader();
                if (!isLoggedIn)
                {
                    return new ResponseModel<List<FavoriteDto>>
                    {
                        Success = false,
                        ErrorMassage = "Please log in first."
                    };
                }

                var response = await _httpClient.GetAsync("api/Favorite");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                    return new ResponseModel<List<FavoriteDto>>
                    {
                        Success = result.Success,
                        Data = result.Data != null
                            ? System.Text.Json.JsonSerializer.Deserialize<List<FavoriteDto>>(
                                  result.Data.ToString(),
                                  new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                            : new List<FavoriteDto>(),
                        ErrorMassage = result.ErrorMassage
                    };
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<FavoriteDto>>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "An unknown error occurred while fetching favorites."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<FavoriteDto>>
                {
                    Success = false,
                    ErrorMassage = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateFavoriteAsync(int productItemId)
        {
            try
            {
                var isLoggedIn = await _authentication.SetAuthorizeHeader();
                if (!isLoggedIn)
                {
                    return new ResponseModel<object>
                    {
                        Success = false,
                        ErrorMassage = "Please log in first."
                    };
                }

                var response = await _httpClient.PostAsync($"api/Favorite?productItemId={productItemId}", null);
                var result = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                return new ResponseModel<object>
                {
                    Success = result.Success,
                    ErrorMassage = result.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> DeleteFavoriteAsync(int favoriteId)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.DeleteAsync($"api/Favorite?favoriteId={favoriteId}");
                var result = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                return new ResponseModel<object>
                {
                    Success = result.Success,
                    ErrorMassage = result.ErrorMassage
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
