using Blazor.Data;
using Blazor.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq;

namespace Blazor.Pages
{
    public partial class Index
    {
        [Inject] public ProductService ProductService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public FavoriteService FavoriteService { get; set; } = default!;
        [Inject] public IToastService ToastService { get; set; } = default!;

        private Dictionary<int, int> CurrentImageIndex { get; set; } = new();
        private IEnumerable<ProductFiltreDto> popularProduct { get; set; } = new List<ProductFiltreDto>();
        private IEnumerable<ProductFiltreDto> newProduct { get; set; } = new List<ProductFiltreDto>();

        protected override async Task OnInitializedAsync()
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            var result = await ProductService.GetPopularProductsAsync();
            popularProduct = result ?? Enumerable.Empty<ProductFiltreDto>();
            var result2 = await ProductService.GetNewProductsAsync();
            newProduct = result2 ?? Enumerable.Empty<ProductFiltreDto>();
        }

        private void NavigateToBrand(int brandId)
        {
            NavigationManager.NavigateTo($"/ProductFilter?brandIds={brandId}");
        }

        private void NavigateToCategory(int GenderIds)
        {
            NavigationManager.NavigateTo($"/ProductFilter?GenderIds={GenderIds}");
        }

        private void NavigateToProductDetails(int productId)
        {
            NavigationManager.NavigateTo($"/product-details/{productId}");
        }

        private async Task AddToFavorites(int productItemId)
        {
            try
            {
                var response = await FavoriteService.CreateFavoriteAsync(productItemId);

                if (response.Success)
                {
                    ToastService.ShowSuccess("Added to favorites successfully!");
                }
                else
                {
                    var message = response.ErrorMassage ?? "An error occurred while adding the product to favorites.";
                    ToastService.ShowWarning(message);
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void NextImage(int productId)
        {
            var p = newProduct.FirstOrDefault(x => x.ProductId == productId);
            if (p?.productFiltrImagees?.Any() != true) return;

            int idx = CurrentImageIndex.GetValueOrDefault(productId, 0);
            CurrentImageIndex[productId] = (idx + 1) % p.productFiltrImagees.Count();
            StateHasChanged();
        }

        private void PrevImage(int productId)
        {
            var p = newProduct.FirstOrDefault(x => x.ProductId == productId);
            if (p?.productFiltrImagees?.Any() != true) return;

            int idx = CurrentImageIndex.GetValueOrDefault(productId, 0);
            CurrentImageIndex[productId] = (idx - 1 + p.productFiltrImagees.Count()) % p.productFiltrImagees.Count();
            StateHasChanged();
        }
    }
}
