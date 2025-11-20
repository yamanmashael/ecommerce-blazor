using Blazor.Authentication;
using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace Blazor.Services
{
    public class Authentication
    {

        private readonly ProtectedLocalStorage _localStorage;
        private readonly HttpClient _httpClient;

        public AuthenticationStateProvider _AuthStateProvider { get; private set; }

        public Authentication(HttpClient httpClient, ProtectedLocalStorage localStorage, AuthenticationStateProvider AuthStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _AuthStateProvider = AuthStateProvider;
        }


        public async Task<bool> SetAuthorizeHeader()
        {
            var sessionstate = (await _localStorage.GetAsync<LoginResponse>("sessionState")).Value;

            if (sessionstate != null && !string.IsNullOrEmpty(sessionstate.Token))
            {
               
                if (sessionstate.TokenExpired < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                {
                    await ((CustonAuthStateProvider)_AuthStateProvider).MarkUserAsLoggedOut();
                    return false;
                }
              
                else if (sessionstate.TokenExpired < DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds())
                {
                   
                    var refreshTokenRequest = new RefreshTokenRequestDto
                    {
                        RefreshToken = sessionstate.RefreshToken
                    };

                   
                    var refreshResponse = await _httpClient.PostAsJsonAsync("api/Account/loginbyrefreshtoken", refreshTokenRequest);

                    if (refreshResponse.IsSuccessStatusCode)
                    {
                        var response = await refreshResponse.Content.ReadFromJsonAsync<LoginResponse>();
                        if (response != null)
                        {
                            await ((CustonAuthStateProvider)_AuthStateProvider).MarkUserAsAuthenticated(response);
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);
                            return true;
                        }
                        else
                        {
                      
                            await ((CustonAuthStateProvider)_AuthStateProvider).MarkUserAsLoggedOut();
                            return false;
                        }
                    }
                    else
                    {
                       
                        await ((CustonAuthStateProvider)_AuthStateProvider).MarkUserAsLoggedOut();
                        return false; 
                    }
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionstate.Token);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }



       



    }
}
