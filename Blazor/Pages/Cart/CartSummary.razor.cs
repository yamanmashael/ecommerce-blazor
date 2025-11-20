using Blazor.Data;
using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Pages.Cart
{
    public partial class CartSummary : ComponentBase
    {
        [Inject] public CartService CartService { get; set; }
        [Inject] public AddressService AddressService { get; set; }
        [Inject] public OrderService OrderService { get; set; }
        [Inject] public EventService EventService { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int AddressId { get; set; }

        private CartDto? cartDto;
        private AddressDto? addresses;
        private bool isLoading = true;
        private string? errorMessage;
        private bool termsAccepted = false;
        private string? selectedPaymentMethod;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            isLoading = false;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var cartResponse = await CartService.GetCartAsync();
                var addressResponse = await AddressService.GetAddressByIdAsync(AddressId);

                if (cartResponse.Success && addressResponse.Success)
                {
                    cartDto = cartResponse.Data;
                    addresses = addressResponse.Data;
                    selectedPaymentMethod = "cash_on_delivery";
                }
                else
                {
                    errorMessage = cartResponse.ErrorMassage ?? addressResponse.ErrorMassage ?? "Failed to load data.";
                    ToastService.ShowError(errorMessage);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Unexpected error: {ex.Message}";
                ToastService.ShowError(errorMessage);
            }
        }

        private async Task FinalizeOrder()
        {
            if (!termsAccepted)
            {
                ToastService.ShowWarning("Please agree to the terms and conditions before proceeding.");
                return;
            }

            var response = await OrderService.CreateOrderAsync(AddressId);

            if (response.Success)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Your order has been successfully placed! ✅");
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ToastService.ShowError(response.ErrorMassage ?? "Order creation failed. Please try again.");
            }
        }
    }
}
