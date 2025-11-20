using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Blazor.Data;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Blazor.Pages.Cart
{
    public partial class Cart : IDisposable
    {
        [Inject] public CartService CartService { get; set; }
        [Inject] public EventService EventService { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private CartDto? cartDto;
        private bool isLoading = true;
        private string? errorMessage;
        private HashSet<int> updatingItems = new();

        [Parameter]
        public int ProductQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            EventService.OnCartUpdated += HandleCartChanged;
            EventService.OnProductRemovedFromCart += HandleCartChanged;
            EventService.OnCartMigration += HandleCartChanged;
            EventService.OnProductAddedToCart += HandleCartChanged;
            EventService.OnOrderCreated += HandleCartChanged;

            await LoadCartData();
        }

        private async void HandleCartChanged()
        {
            await LoadCartData();
            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadCartData()
        {
            isLoading = true;
            try
            {
                ProductQuantity = await CartService.CartQuantity();
                var response = await CartService.GetCartAsync();

                if (response.Success)
                {
                    cartDto = response.Data ?? new CartDto { TotalPrice = 0, CartItemDto = new List<CartItemDto>() };
                }
                else
                {
                    errorMessage = response.ErrorMassage ?? "An unknown error occurred while loading the cart.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load cart: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task UpdateQuantity(int cartItemId, int change)
        {
            if (updatingItems.Contains(cartItemId))
                return;

            try
            { 
                updatingItems.Add(cartItemId);

                var dto = new UpdateCartItemQuantityDto
                {
                    CartItemId = cartItemId,
                    Quantity = change
                };

                await CartService.UpdateItemQuantityAsync(dto);
                await LoadCartData();
            }
            finally
            {
                updatingItems.Remove(cartItemId);
                StateHasChanged();
            }
        }

        private async Task RemoveItem(int cartItemId)
        {
            await CartService.RemoveCartItemAsync(cartItemId);
        }

        private void Checkout()
        {
            NavigationManager.NavigateTo("/select-address");
        }

        public void Dispose()
        {
            EventService.OnCartUpdated -= HandleCartChanged;
            EventService.OnProductRemovedFromCart -= HandleCartChanged;
            EventService.OnCartMigration -= HandleCartChanged;
            EventService.OnOrderCreated -= HandleCartChanged;
        }
    }
}
