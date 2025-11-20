using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Blazor.Data;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Pages
{
    public partial class ProductDetails : ComponentBase
    {
        [Inject] public ProductService ProductApiService { get; set; } = default!;
        [Inject] public CartService CartService { get; set; } = default!;
        [Inject] public FavoriteService FavoriteService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public IToastService ToastService { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private ProductDetailsDto? product;
        private bool isLoading = true;
        private string? errorMessage;

        private int selectedProductItemId;
        private ProductItemDetailsDto? selectedProductItem;
        private int selectedSizeId;
        private ProductSizeDetailsDto? selectedSize;

        private string mainImageUrl;

        protected override async Task OnInitializedAsync()
        {
            await LoadProductDetails();
        }

        private async Task LoadProductDetails()
        {
            isLoading = true;
            try
            {
                var apiResponse = await ProductApiService.GetProductDatailesByIdAsync(Id);
                if (apiResponse.Success)
                {
                    product = apiResponse.Data;
                    if (product?.ProductItems != null && product.ProductItems.Any())
                    {
                        SelectProductItem(product.ProductItems.First().Id);
                    }
                }
                else
                {
                    errorMessage = apiResponse.ErrorMassage;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Server connection error: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SelectProductItem(int productItemId)
        {
            selectedProductItemId = productItemId;
            selectedProductItem = product?.ProductItems.FirstOrDefault(pi => pi.Id == productItemId);

            mainImageUrl = selectedProductItem?.ImageFilenames.FirstOrDefault();

            selectedSizeId = 0;
            selectedSize = null;
        }

        private void SelectSize(int sizeId)
        {
            selectedSizeId = sizeId;
            selectedSize = selectedProductItem?.Sizes.FirstOrDefault(s => s.SizeId == sizeId);
        }

        private bool CanAddToCart()
        {
            return selectedProductItem != null && selectedSize != null && selectedSize.Stock > 0;
        }

        private async void AddToCart(int ProductItemId, int SizeId)
        {
            var addcart = new AddToCartDto()
            {
                ProductItemId = ProductItemId,
                SizeId = SizeId,
                Quantity = 1
            };

            await CartService.AddItemToCartAsync(addcart);
        }

        private async Task AddToFavorites(int productItemId)
        {
            try
            {
                var response = await FavoriteService.CreateFavoriteAsync(productItemId);

                if (response.Success)
                {
                    ToastService.ShowSuccess("Added to favorites successfully.");
                }
                else
                {
                    var message = response.ErrorMassage ?? "An error occurred while adding the product to favorites.";
                    ToastService.ShowWarning(message);

                    if (message.Contains("login"))
                    {
                        NavigationManager.NavigateTo("/login");
                    }
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Unexpected error: {ex.Message}");
            }
        }

        private void ChangeMainImage(string imageUrl)
        {
            mainImageUrl = imageUrl;
        }
    }
}
