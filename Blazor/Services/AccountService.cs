using Blazor.Authentication;
using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class AccountService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly HttpClient _httpClient;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;
        private readonly EventService _eventService;

        public AccountService(EventService eventService, Authentication authentication, HttpClient httpClient, ProtectedLocalStorage localStorage, AuthenticationStateProvider AuthStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _AuthStateProvider = AuthStateProvider;
            _authentication = authentication;
            _eventService = eventService;
        }

        public async Task<ResponseModel<object>> UpdatePasswordAsync(UpdatePassword updatePassword)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.PostAsJsonAsync("api/Account/update-password", updatePassword);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
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
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> ConfirmEmailAsync(string token, string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Account/confirmemail?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
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
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<LoginResponse>> LoginWithGoogleAsync(string idToken)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/google-login", new { IdToken = idToken });


                if (response.IsSuccessStatusCode)
                
                return await response.Content.ReadFromJsonAsync<ResponseModel<LoginResponse>>();
              
                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<LoginResponse>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<LoginResponse>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<LoginResponse>> LoginAsync(Login model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/login", model);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<LoginResponse>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<LoginResponse>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<LoginResponse>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> RegisterAsync(Register model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/register", model);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
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
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> UpdateProfileAsync(UpdateProfile updateProfile)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.PutAsJsonAsync("api/Account/updateprofile", updateProfile);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
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
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<UpdateProfile> GetUserByIdAsync()
        {
            await _authentication.SetAuthorizeHeader();
            var res = await _httpClient.GetFromJsonAsync<BaseResponseModel>("api/Account/user");

            if (res != null && res.Success && res.Data != null)
                return JsonConvert.DeserializeObject<UpdateProfile>(res.Data.ToString());

            return null;
        }

        public async Task<BaseResponseModel> ForgotPasswordAsync(forgotPassword forgotPassword)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/forgotpassword", forgotPassword);
            return await response.Content.ReadFromJsonAsync<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> ResetPasswordAsync(ResetPassword resetPassword)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/resetpassword", resetPassword);
            return await response.Content.ReadFromJsonAsync<BaseResponseModel>();
        }
    }
}
