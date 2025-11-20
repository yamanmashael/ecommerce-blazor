using Blazor.Data;
using Blazor.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System;
using Blazored.Toast.Services;
using System.Threading;
using Syncfusion.Blazor;

namespace Blazor.Pages.ProductFilter
{
    public partial class ProductFilter
    {
        [Inject] public ProductService ProductService { get; set; }
        [Inject] public GenderService GenderService { get; set; }
        [Inject] public BrandService BrandService { get; set; }
        [Inject] public SizeService SizeService { get; set; }
        [Inject] public FavoriteService FavoriteService { get; set; } = default!;
        [Inject] public ColorService ColorService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; } = default!;

        private SearchFilterDto Filter { get; set; } = new()
        {
            GenderIds = new List<int>(),
            BrandIds = new List<int>(),
            SizeIds = new List<int>(),
            ColorIds = new List<int>(),
            CategoryIds = new List<int>(),
            CategoryItemIds = new List<int>()
        };


        private string searchKeyword = string.Empty;
        private bool isLoading = true;

   
        private IEnumerable<ProductFiltreDto> Products { get; set; } = new List<ProductFiltreDto>();
        private IEnumerable<GenderDto> Genders { get; set; } = new List<GenderDto>();
        private IEnumerable<BrandDto> Brands { get; set; } = new List<BrandDto>();
        private IEnumerable<SizeDto> Sizes { get; set; } = new List<SizeDto>();
        private IEnumerable<ColorDto> Colors { get; set; } = new List<ColorDto>();
        private IEnumerable<CategoryFiltrDto> Categories { get; set; } = new List<CategoryFiltrDto>();
        private IEnumerable<CategoryItemFiltrDto> CategoryItems { get; set; } = new List<CategoryItemFiltrDto>();

     
        private Dictionary<int, int> CurrentImageIndex { get; set; } = new();

  
        private decimal? minPriceValue;
        private decimal? maxPriceValue;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            await ParseQueryParameters();
            await LoadFilterDataAsync();
            await LoadProductsAsync();
            isLoading = false;
        }

        private async Task ParseQueryParameters()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("genderIds", out var genderValues))
            {
                Filter.GenderIds = genderValues.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToList();
            }

            if (queryParams.TryGetValue("brandIds", out var brandValues))
            {
                Filter.BrandIds = brandValues.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToList();
            }

            if (queryParams.TryGetValue("sizeIds", out var sizeValues))
            {
                Filter.SizeIds = sizeValues.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToList();
            }

            if (queryParams.TryGetValue("colorIds", out var colorValues))
            {
                Filter.ColorIds = colorValues.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToList();
            }

            if (queryParams.TryGetValue("categoryIds", out var categoryValues))
            {
                Filter.CategoryIds = categoryValues.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToList();
            }

            if (queryParams.TryGetValue("categoryItemIds", out var categoryItemValues))
            {
                Filter.CategoryItemIds = categoryItemValues.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToList();
            }

            if (queryParams.TryGetValue("minPrice", out var minPriceStr) && decimal.TryParse(minPriceStr, out var minPrice))
            {
                Filter.MinPrice = minPrice;
                minPriceValue = minPrice;
            }

            if (queryParams.TryGetValue("maxPrice", out var maxPriceStr) && decimal.TryParse(maxPriceStr, out var maxPrice))
            {
                Filter.MaxPrice = maxPrice;
                maxPriceValue = maxPrice;
            }

            if (queryParams.TryGetValue("keyword", out var keyword))
            {
                Filter.Keyword = keyword;
                searchKeyword = keyword;
            }
        }

        private async Task LoadFilterDataAsync()
        {
            var genderResult = await GenderService.GetGendersAsync();
            if (genderResult?.Data != null)
                Genders = genderResult.Data;

            var brandResult = await BrandService.GetBrandsAsync();
            if (brandResult?.Data != null)
                Brands = brandResult.Data;

            var sizeResult = await SizeService.GetAllAsync();
            if (sizeResult?.Data != null)
                Sizes = sizeResult.Data;

            var colorResult = await ColorService.GetColorsAsync();
            if (colorResult?.Data != null)
                Colors = colorResult.Data;
        }

        private async Task LoadProductsAsync()
        {
            if (string.IsNullOrWhiteSpace(searchKeyword))
                Filter.Keyword = null;
            else
                Filter.Keyword = searchKeyword;

            var result = await ProductService.GetFilteredProductsAsync(Filter);
            Products = result.productFiltreDtos ?? Enumerable.Empty<ProductFiltreDto>();
            Categories = result.categoryFiltrDtos ?? Enumerable.Empty<CategoryFiltrDto>();
            CategoryItems = result.CategoryItemFiltrDto ?? Enumerable.Empty<CategoryItemFiltrDto>();

            UpdateUrl();
        }

        private void UpdateUrl()
        {
            var queryParams = new List<string>();

            AddListToQueryParams(queryParams, "genderIds", Filter.GenderIds);
            AddListToQueryParams(queryParams, "brandIds", Filter.BrandIds);
            AddListToQueryParams(queryParams, "sizeIds", Filter.SizeIds);
            AddListToQueryParams(queryParams, "colorIds", Filter.ColorIds);
            AddListToQueryParams(queryParams, "categoryIds", Filter.CategoryIds);
            AddListToQueryParams(queryParams, "categoryItemIds", Filter.CategoryItemIds);

            if (Filter.MinPrice.HasValue)
                queryParams.Add($"minPrice={Filter.MinPrice.Value}");

            if (Filter.MaxPrice.HasValue)
                queryParams.Add($"maxPrice={Filter.MaxPrice.Value}");

            if (!string.IsNullOrEmpty(Filter.Keyword))
                queryParams.Add($"keyword={Uri.EscapeDataString(Filter.Keyword)}");

            var query = queryParams.Any()
                ? "/ProductFilter?" + string.Join("&", queryParams)
                : "/ProductFilter";

            NavigationManager.NavigateTo(query, forceLoad: false);
        }

        private void AddListToQueryParams(List<string> queryParams, string paramName, List<int> ids)
        {
            if (ids.Any())
            {
          
                foreach (var id in ids)
                {
                    queryParams.Add($"{paramName}={id}");
                }
            }
        }

        private async Task OnFilterChanged(object value, int id, string propertyName)
        {
            bool isChecked = (bool)value;
            var property = Filter.GetType().GetProperty(propertyName);

            if (property?.GetValue(Filter) is List<int> list)
            {
                if (isChecked && !list.Contains(id))
                    list.Add(id);
                else if (!isChecked && list.Contains(id))
                    list.Remove(id);
            }

            await LoadProductsAsync();
        }


        private async Task ApplyPriceFilter()
        {
            Filter.MinPrice = minPriceValue;
            Filter.MaxPrice = maxPriceValue;
            await LoadProductsAsync();
        }

        private async Task ResetFilters()
        {
            Filter = new SearchFilterDto
            {
                GenderIds = new List<int>(),
                BrandIds = new List<int>(),
                SizeIds = new List<int>(),
                ColorIds = new List<int>(),
                CategoryIds = new List<int>(),
                CategoryItemIds = new List<int>()
            };
            minPriceValue = null;
            maxPriceValue = null;
            searchKeyword = string.Empty;
            await LoadProductsAsync();
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
                    ToastService.ShowSuccess("Added to Favorites successfully!");
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
            var p = Products.FirstOrDefault(x => x.ProductId == productId);
            if (p?.productFiltrImagees?.Any() != true) return;

            int idx = CurrentImageIndex.GetValueOrDefault(productId, 0);
            CurrentImageIndex[productId] = (idx + 1) % p.productFiltrImagees.Count();
            StateHasChanged();
        }

        private void PrevImage(int productId)
        {
            var p = Products.FirstOrDefault(x => x.ProductId == productId);
            if (p?.productFiltrImagees?.Any() != true) return;

            int idx = CurrentImageIndex.GetValueOrDefault(productId, 0);
            CurrentImageIndex[productId] = (idx - 1 + p.productFiltrImagees.Count()) % p.productFiltrImagees.Count();
            StateHasChanged();
        }
    }
}