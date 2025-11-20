using Blazor.Data;
using Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace Blazor.Shared
{
    public partial class MainLayout : IDisposable
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthProvider { get; set; }

        [Inject]
        public CartService CartService { get; set; }

        [Inject]
        public EventService EventService { get; set; }

        [Inject]
        public ProductService ProductService { get; set; }

        // Unused property, removed to clean up code.
        // private string SearchTerm { get; set; } = string.Empty; 

        // This property is not used for the input value in the .razor file, 
        // which correctly uses the private 'searchText' field.
        [Parameter]
        public string SearchText { get; set; }


        [Parameter]
        public int ProductQuantity { get; set; }

        private string? searchText; // This is the field bound to the input
        private CancellationTokenSource? cts;
        private List<string> suggestions = new();

        protected override async Task OnInitializedAsync()
        {
            await CartQuantity();

            // Subscribe to events
            EventService.OnCartUpdated += HandleCartChanged;
            EventService.OnProductRemovedFromCart += HandleCartChanged;
            EventService.OnCartMigration += HandleCartChanged;
            EventService.OnProductAddedToCart += HandleCartChanged;
            EventService.OnOrderCreated += HandleCartChanged;
            EventService.OnUserLoggedIn += HandleCartChanged;

            // Check for search keyword in URL on load
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);
            if (queryParams.TryGetValue("Keyword", out var keyword))
                searchText = keyword;

            await CartQuantity();
        }

        private async Task OnInputChanged(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;

            if (cts != null)
                cts.Cancel(); // Cancel the previous request

            if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 2) // Added check for minimum length
            {
                suggestions.Clear();
                return;
            }

            cts = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, cts.Token); // Debounce delay
                suggestions = await ProductService.GetSuggestionsAsync(searchText);
            }
            catch (TaskCanceledException)
            {
                // Request was canceled (normal during active typing)
            }
            catch (Exception ex)
            {
                // Handle other exceptions if necessary
                Console.WriteLine($"Error fetching suggestions: {ex.Message}");
            }
        }

        private void SelectSuggestion(string suggestion)
        {
            searchText = suggestion;
            suggestions.Clear();
            OnSearch();
        }

        private void OnSearch()
        {
            suggestions.Clear(); // Close suggestions on search
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                NavigationManager.NavigateTo($"/ProductFilter?Keyword={searchText}", forceLoad: true);
            }
            else
            {
                NavigationManager.NavigateTo("/ProductFilter", forceLoad: true);
            }
        }

        private async void HandleCartChanged()
        {
            await CartQuantity();
            await InvokeAsync(StateHasChanged);
        }

        private async Task CartQuantity()
        {
            ProductQuantity = await CartService.CartQuantity();
            // No need to call StateHasChanged here if it's called in HandleCartChanged, 
            // but keeping it doesn't hurt, just ensure it runs on the UI thread.
            await InvokeAsync(StateHasChanged);
        }

        private void HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                OnSearch();
            }
        }

        private async Task Logout()
        {
            // You don't need to call GetAuthenticationStateAsync() before navigating to /logout
            NavigationManager.NavigateTo("/logout", forceLoad: true);
        }

        public void Dispose()
        {
            // Unsubscribe from events to prevent memory leaks
            EventService.OnProductAddedToCart -= HandleCartChanged;
            EventService.OnCartUpdated -= HandleCartChanged;
            EventService.OnProductRemovedFromCart -= HandleCartChanged;
            EventService.OnCartMigration -= HandleCartChanged;
            EventService.OnOrderCreated -= HandleCartChanged;
            EventService.OnUserLoggedIn -= HandleCartChanged;
        }
    }
}