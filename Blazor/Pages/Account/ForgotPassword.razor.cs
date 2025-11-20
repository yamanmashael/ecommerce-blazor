using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages.Account
{
    public partial  class ForgotPassword
    {

        public Blazor.Data.forgotPassword Model { get; set; } = new();

        [Inject]
        public AccountService AccountService { get; set; }

        [Inject]
        public IToastService toastService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        public async Task Submit()
        {

            var response = await AccountService.ForgotPasswordAsync(Model);

            if (response != null && response.Success)
            {
                toastService.ShowSuccess("Password reset link has been sent to your email.");
                NavigationManager.NavigateTo("/");
            }
            else
            {
                var errorMessage = response?.ErrorMassage ?? "An unknown error occurred while requesting password reset.";
                toastService.ShowError($"Password reset failed: {errorMessage}");
            }


        }


    }
}
