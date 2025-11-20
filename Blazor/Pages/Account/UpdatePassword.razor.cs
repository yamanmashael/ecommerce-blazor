using Blazor.Data;
using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;

namespace Blazor.Pages.Account
{
    public partial class UpdatePassword
    {
        [Parameter] public string ConfirmNewPassword { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public AccountService AccountService { get; set; }
        [Inject] public IToastService ToastService { get; set; }

        public Blazor.Data.UpdatePassword Model { get; set; } = new();

        private async Task HandleUpdatePassword()
        {
            if (string.IsNullOrWhiteSpace(Model.NewPassword))
            {
                ToastService.ShowError("Please enter a new password.");
                return;
            }

            if (Model.NewPassword.Length < 8)
            {
                ToastService.ShowWarning("Password must be at least 8 characters long and should include at least one uppercase letter.");
                return;
            }

            if (!Model.NewPassword.Any(char.IsUpper))
            {
                ToastService.ShowWarning("Password should include at least one uppercase letter.");
                return;
            }

            if (Model.NewPassword != ConfirmNewPassword)
            {
                ToastService.ShowError("New password and confirmation do not match.");
                return;
            }

            var response = await AccountService.UpdatePasswordAsync(Model);

            if (response.Success)
            {
                ToastService.ShowSuccess("Password updated successfully.");
                NavigationManager.NavigateTo("/");
            }
            else
            {
                var errorMessage = response.ErrorMassage ?? "An unknown error occurred.";
                ToastService.ShowError($"Update failed: {errorMessage}");
            }
        }
    }
}
