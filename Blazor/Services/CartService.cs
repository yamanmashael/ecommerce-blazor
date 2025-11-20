using Blazor.Authentication;
using Blazor.Data;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor.Services
{
    public class CartService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly IToastService _toastService;
        private readonly Authentication _authentication;
        private readonly EventService _eventService;

        public CartService(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            ProtectedLocalStorage localStorage,
            IToastService toastService,
            Authentication authentication,
            EventService eventService)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _toastService = toastService;
            _authentication = authentication;
            _eventService = eventService;
        }

        public async Task MigrateGuestCartToUserCart()
        {
            var local = await _localStorage.GetAsync<List<LocalCartItem>>("cart");
            var items = local.Value?.Where(x => x.Quantity > 0).ToList();
            if (items is null || items.Count == 0) return;

            await _authentication.SetAuthorizeHeader();

            var payload = new CartMigrationRequest
            {
                Items = items.Select(i => new GuestCartItemDto
                {
                    ProductItemId = i.ProductItemId,
                    SizeId = i.SizeId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var res = await _httpClient.PostAsJsonAsync("api/Cart/migrate", payload);

            if (res.IsSuccessStatusCode)
            {
                await _localStorage.DeleteAsync("cart");
                _toastService.ShowSuccess("Guest cart has been successfully merged with your account.");
                _eventService.RaiseCartMigration();
            }
            else
            {
                var msg = await res.Content.ReadAsStringAsync();
                _toastService.ShowError($"Merge failed: {msg}");
            }
        }


        // 🔹 Get total cart quantity
        public async Task<int> CartQuantity()
        {
            if (await _authentication.SetAuthorizeHeader())
            {
                var response = await _httpClient.GetAsync("api/Cart/quantity");
                var result = await response.Content.ReadFromJsonAsync<ResponseModel<int>>();

                return result?.Success == true ? result.Data : 0;
            }
            else
            {
                var localCart = await _localStorage.GetAsync<List<LocalCartItem>>("cart");
                return localCart.Success && localCart.Value != null
                    ? localCart.Value.Sum(x => x.Quantity)
                    : 0;
            }
        }

        
        public async Task<ResponseModel<CartDto>> GetCartAsync()
        {
            if (await _authentication.SetAuthorizeHeader())
            {
                var response = await _httpClient.GetAsync("api/Cart");
                return await response.Content.ReadFromJsonAsync<ResponseModel<CartDto>>();
            }

            var guestCart = await _localStorage.GetAsync<List<LocalCartItem>>("cart");

            if (guestCart.Value != null && guestCart.Value.Any())
            {
                var response = await _httpClient.PostAsJsonAsync("api/Cart/guest-cart", guestCart.Value);
                return await response.Content.ReadFromJsonAsync<ResponseModel<CartDto>>();
            }

            return new ResponseModel<CartDto>
            {
                Success = true,
                Data = new CartDto { TotalPrice = 0, CartItemDto = new List<CartItemDto>() }
            };
        }

 
        public async Task AddItemToCartAsync(AddToCartDto addToCart)
        {
            try
            {
                if (await _authentication.SetAuthorizeHeader())
                {
                    var response = await _httpClient.PostAsJsonAsync("api/Cart", addToCart);
                    var result = await response.Content.ReadFromJsonAsync<ResponseModel<CartDto>>();

                    if (response.IsSuccessStatusCode && result?.Success == true)
                        _toastService.ShowSuccess("Product successfully added to your cart!");
                    else
                        _toastService.ShowWarning(result?.ErrorMassage ?? "Failed to add product to cart.");
                }
                else
                {
                    await HandleGuestCartAddition(addToCart);
                }

                _eventService.RaiseProductAddedToCart();
            }
            catch
            {
                _toastService.ShowError("An unexpected error occurred while adding the product.");
            }
        }

        private async Task HandleGuestCartAddition(AddToCartDto addToCart)
        {
            var result = await _localStorage.GetAsync<List<LocalCartItem>>("cart");
            var guestCart = result.Value ?? new List<LocalCartItem>();

            var existingItem = guestCart.FirstOrDefault(i =>i.ProductItemId == addToCart.ProductItemId && i.SizeId == addToCart.SizeId);

            if (existingItem != null)
            {
                var stock = new CheckStoc
                {
                    productItemId = addToCart.ProductItemId,
                    sizeId = addToCart.SizeId
                };

                var response = await _httpClient.PostAsJsonAsync("api/Cart/stock", stock);
                var stockResult = await response.Content.ReadFromJsonAsync<ResponseModel<int>>();

                if (stockResult?.Success == true)
                {
                    if (stockResult.Data >= existingItem.Quantity + addToCart.Quantity)
                    {
                        existingItem.Quantity += addToCart.Quantity;
                        _toastService.ShowSuccess("Product quantity updated in your cart!");
                    }
                    else
                    {
                        _toastService.ShowWarning($"Only {stockResult.Data} items available in stock!");
                    }
                }
            }
            else
            {
                guestCart.Add(new LocalCartItem
                {
                    cartItemId = new Random().Next(1, int.MaxValue),
                    ProductItemId = addToCart.ProductItemId,
                    SizeId = addToCart.SizeId,
                    Quantity = addToCart.Quantity
                });

                _toastService.ShowSuccess("Product added to your cart!");
            }

            await _localStorage.SetAsync("cart", guestCart);
        }

       
        public async Task UpdateItemQuantityAsync(UpdateCartItemQuantityDto dto)
        {
            try
            {
                if (await _authentication.SetAuthorizeHeader())
                {
                    var response = await _httpClient.PutAsJsonAsync("api/Cart", dto);
                    var result = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();

                    if (response.IsSuccessStatusCode && result?.Success == true)
                        _toastService.ShowSuccess("Product quantity updated successfully!");
                    else
                        _toastService.ShowWarning(result?.ErrorMassage ?? "Failed to update product quantity.");
                }
                else
                {
                    var result = await _localStorage.GetAsync<List<LocalCartItem>>("cart");
                    var guestCart = result.Value ?? new List<LocalCartItem>();
                    var existingItem = guestCart.FirstOrDefault(i => i.cartItemId == dto.CartItemId);

                    if (existingItem == null)
                    {
                        _toastService.ShowError("Product not found in guest cart.");
                        return;
                    }

                    var stockCheck = new CheckStoc
                    {
                        productItemId = existingItem.ProductItemId,
                        sizeId = existingItem.SizeId
                    };

                    var stockResponse = await _httpClient.PostAsJsonAsync("api/Cart/stock", stockCheck);
                    var stockData = await stockResponse.Content.ReadFromJsonAsync<ResponseModel<int>>();

                    if (stockData?.Success == true && (existingItem.Quantity + dto.Quantity) <= stockData.Data)
                    {
                        existingItem.Quantity += dto.Quantity;
                        await _localStorage.SetAsync("cart", guestCart);
                        _toastService.ShowSuccess("Product quantity updated successfully!");
                    }
                    else
                    {
                        _toastService.ShowWarning($"Only {stockData?.Data ?? 0} items available in stock!");
                    }
                }

                _eventService.RaiseCartUpdated();
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error: {ex.Message}");
            }
        }


        public async Task RemoveCartItemAsync(int cartItemId)
        {
            try
            {
                if (await _authentication.SetAuthorizeHeader())
                {
                    var response = await _httpClient.DeleteAsync($"api/Cart/{cartItemId}");

                    if (response.IsSuccessStatusCode)
                        _toastService.ShowSuccess("Product removed from your cart successfully!");
                    else
                        _toastService.ShowError("Failed to remove product from cart.");
                }
                else
                {
                    await HandleGuestCartRemoval(cartItemId);
                }

                _eventService.RaiseProductRemovedFromCart();
            }
            catch
            {
                _toastService.ShowError("An unexpected error occurred while removing the product.");
            }
        }

        private async Task HandleGuestCartRemoval(int cartItemId)
        {
            var result = await _localStorage.GetAsync<List<LocalCartItem>>("cart");
            var guestCart = result.Value ?? new List<LocalCartItem>();

            var existingItem = guestCart.FirstOrDefault(i => i.cartItemId == cartItemId);
            if (existingItem != null)
            {
                guestCart.Remove(existingItem);
                await _localStorage.SetAsync("cart", guestCart);
                _toastService.ShowSuccess("Product removed from guest cart.");
            }
            else
            {
                _toastService.ShowError("Product not found in guest cart.");
            }
        }




 



       

    }
}
