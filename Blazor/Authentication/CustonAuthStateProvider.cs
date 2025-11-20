using Blazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blazor.Authentication
{
    public class CustonAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;

        public CustonAuthStateProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }



            public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                var sessionModel = (await _localStorage.GetAsync<LoginResponse>("sessionState")).Value;
                var identity = sessionModel == null ? new ClaimsIdentity() : GetClaimsIdentity(sessionModel.Token);
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);

            }
    

        public async Task MarkUserAsAuthenticated(LoginResponse model)
        {
            await _localStorage.SetAsync("sessionState" , model);
            var identity = GetClaimsIdentity(model.Token); 
            var user =new  ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }



        private ClaimsIdentity GetClaimsIdentity(String token)
        {
            var Handler = new JwtSecurityTokenHandler();
            var jwtToken = Handler.ReadJwtToken(token);
            var claims= jwtToken.Claims;    
            return new ClaimsIdentity(claims , "jwt");  
        }




        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.DeleteAsync("sessionState");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));


        }



    }
}





