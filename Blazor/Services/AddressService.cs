using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.Authentication;

namespace Blazor.Services
{
    public class AddressService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public AddressService(Authentication authentication, HttpClient httpClient, AuthenticationStateProvider AuthStateProvider, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _AuthStateProvider = AuthStateProvider;
            _localStorage = localStorage;
            _authentication = authentication;
        }

        public async Task<ResponseModel<List<AddressDto>>> GetAllAddressesAsync()
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.GetAsync("api/Address");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<AddressDto>>>();

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<AddressDto>>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<AddressDto>>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<AddressDto>> GetAddressByIdAsync(int id)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.GetAsync($"api/Address/{id}");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ResponseModel<AddressDto>>();

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<AddressDto>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<AddressDto>
                {
                    Success = false,
                    ErrorMassage = $"Connection error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateAddressAsync(CreateAddressDto address)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.PostAsJsonAsync("api/Address", address);

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

        public async Task<ResponseModel<object>> UpdateAddressAsync(UpdateAddressDto address)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.PutAsJsonAsync("api/Address", address);

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

        public async Task<ResponseModel<object>> DeleteAddressAsync(int id)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.DeleteAsync($"api/Address/{id}");

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
    }
}
