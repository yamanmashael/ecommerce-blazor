using Blazor.Data;
using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Blazor.Services;
using Blazor.Data;
namespace Blazor.Pages.Account
{
    public partial class UpdateProfile
    {

        [Inject] public IToastService ToastService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public AccountService AccountService { get; set; }

        public Blazor.Data.UpdateProfile Model { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadUserData();
        }

        private async Task LoadUserData()
        {
            var data = await AccountService.GetUserByIdAsync();
            if (data == null)
            {
                ToastService.ShowError("User data not found.");
                data = null;
            }
            Model = data;
        }

        public async Task Submit()
        {
            var response = await AccountService.UpdateProfileAsync(Model);
            if (response.Success)
            {
                ToastService.ShowSuccess("Profile updated successfully.");
                NavigationManager.NavigateTo("/updateprofile", true);
            }
            else
            {
                var errorMessage = response?.ErrorMassage ?? "An unknown error occurred.";
                ToastService.ShowError($"Update failed: {errorMessage}");
            }
        }
    }
}
