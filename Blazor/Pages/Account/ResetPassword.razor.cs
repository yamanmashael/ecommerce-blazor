using Blazor.Data;
using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Blazor.Pages.Account
{
    public partial  class ResetPassword
    {


        public Blazor.Data.ResetPassword Model { get; set; } = new();

        [Inject]
        public AccountService AccountService { get; set; }

        [Inject]
        public IToastService toastService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        protected override void OnInitialized()
        {
         
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var query = HttpUtility.ParseQueryString(uri.Query);
            Model.Token = query["token"];

        
        }


        private async Task HandleReset()
        {
            var response = await AccountService.ResetPasswordAsync(Model);
            if (response != null && response.Success)
            {
                toastService.ShowSuccess("Password has been reset successfully.");
                NavigationManager.NavigateTo("/auth"); 
            }
            else
            {
                var errorMessage = response?.ErrorMassage ?? "An unknown error occurred while resetting the password.";
                toastService.ShowError($"Password reset failed: {errorMessage}");
            }


        }



    }
}
