using Blazor.Data;
using Blazor.Services;
using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Pages.Favorite
{
    public partial class IndexFavorite
    {
        [Inject] private Blazor.Services.Authentication Authentication { get; set; }
        [Inject] private FavoriteService FavoriteService { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private List<FavoriteDto> favorites;
        private Dictionary<int, int> CurrentImageIndex = new();

        protected override async Task OnInitializedAsync()
        {
            var isLoggedIn = await Authentication.SetAuthorizeHeader();
            if (!isLoggedIn)
            {
                NavigationManager.NavigateTo("/auth");
                return;
            }

            await LoadFavorites();
        }

        private async Task LoadFavorites()
        {
            var response = await FavoriteService.GetFavoriteAsync();

            if (response.Success)
            {
                favorites = response.Data ?? new List<FavoriteDto>();

                foreach (var item in favorites)
                    if (!CurrentImageIndex.ContainsKey(item.FavoriteId))
                        CurrentImageIndex[item.FavoriteId] = 0;
            }
            else
            {
                ToastService.ShowError(response.ErrorMassage ?? "Failed to load favorites.");
                favorites = new List<FavoriteDto>();
            }
        }

        private void NextImage(int favoriteId)
        {
            var item = favorites.FirstOrDefault(x => x.FavoriteId == favoriteId);
            if (item?.favoriteImageeDtos?.Any() != true) return;

            int idx = CurrentImageIndex.GetValueOrDefault(favoriteId, 0);
            CurrentImageIndex[favoriteId] = (idx + 1) % item.favoriteImageeDtos.Count;
            StateHasChanged();
        }

        private void PrevImage(int favoriteId)
        {
            var item = favorites.FirstOrDefault(x => x.FavoriteId == favoriteId);
            if (item?.favoriteImageeDtos?.Any() != true) return;

            int idx = CurrentImageIndex.GetValueOrDefault(favoriteId, 0);
            CurrentImageIndex[favoriteId] = (idx - 1 + item.favoriteImageeDtos.Count) % item.favoriteImageeDtos.Count;
            StateHasChanged();
        }

        private void NavigateToProductDetails(int productItemId)
        {
            NavigationManager.NavigateTo($"/product-details/{productItemId}");
        }

        private async Task DeleteFavorite(int favoriteId)
        {
            var response = await FavoriteService.DeleteFavoriteAsync(favoriteId);

            if (response.Success)
            {
                ToastService.ShowSuccess("Product removed from favorites successfully.");
                CurrentImageIndex.Remove(favoriteId);
                await LoadFavorites();
            }
            else
            {
                ToastService.ShowError(response.ErrorMassage ?? "An error occurred while removing the product.");
            }
        }
    }
}
