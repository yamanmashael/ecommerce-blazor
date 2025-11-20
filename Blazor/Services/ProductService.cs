using Blazor.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Blazor.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace Blazor.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthenticationStateProvider _AuthStateProvider { get; private set; }
        private readonly Authentication _authentication;

        public ProductService(Authentication authentication, HttpClient httpClient, ProtectedLocalStorage localStorage, AuthenticationStateProvider AuthStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _AuthStateProvider = AuthStateProvider;
            _authentication = authentication;
        }



        public async Task<List<ProductFiltreDto>> GetPopularProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductFiltreDto>>("api/Product/PopularProduct");
            return response ?? new List<ProductFiltreDto>();
        }

        public async Task<List<ProductFiltreDto>> GetNewProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductFiltreDto>>("api/Product/NewProduct");
            return response ?? new List<ProductFiltreDto>();
        }

        public async Task<List<string>> GetSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            var response = await _httpClient.GetFromJsonAsync<List<string>>($"api/Product/SearchSuggestions?query={query}");
            return response ?? new List<string>();
        }

        public async Task<FiltreDto> GetFilteredProductsAsync(SearchFilterDto request)
        {
            var queryParams = new List<string>();

            ProcessListParameters(queryParams, request.GenderIds, "GenderIds");
            ProcessListParameters(queryParams, request.CategoryIds, "CategoryIds");
            ProcessListParameters(queryParams, request.CategoryItemIds, "CategoryItemIds");
            ProcessListParameters(queryParams, request.BrandIds, "BrandIds");
            ProcessListParameters(queryParams, request.SizeIds, "SizeIds");
            ProcessListParameters(queryParams, request.ColorIds, "ColorIds");

            if (request.MinPrice.HasValue)
                queryParams.Add($"MinPrice={request.MinPrice.Value}");

            if (request.MaxPrice.HasValue)
                queryParams.Add($"MaxPrice={request.MaxPrice.Value}");

            if (!string.IsNullOrEmpty(request.Keyword))
                queryParams.Add($"Keyword={Uri.EscapeDataString(request.Keyword)}");

            var query = "api/Product/filter";
            if (queryParams.Any())
                query += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(query);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<FiltreDto>();

            return new FiltreDto
            {
                productFiltreDtos = Enumerable.Empty<ProductFiltreDto>(),
                categoryFiltrDtos = Enumerable.Empty<CategoryFiltrDto>(),
                CategoryItemFiltrDto = Enumerable.Empty<CategoryItemFiltrDto>()
            };
        }

        private void ProcessListParameters(List<string> queryParams, List<int>? ids, string parameterName)
        {
            if (ids != null && ids.Any())
            {
                queryParams.AddRange(ids.Select(id => $"{parameterName}={id}"));
            }
        }

        public async Task<ResponseModel<List<ProductDto>>> GetAllProductAsync(RequestProduct request)
        {
            try
            {
                if (request.PageNumber == 0)
                    request.PageNumber = 1;

                if (request.PageSize == 0)
                    request.PageSize = 5;

                var query = $"api/Product?genderId={request.genderId}&categoryId={request.categoryId}" +
                            $"&categoryItemId={request.categoryItemId}&brandId={request.BrandId}" +
                            $"&search={request.search}&pageNumber={request.PageNumber}&pageSize={request.PageSize}";

                var response = await _httpClient.GetAsync(query);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<ProductDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new ResponseModel<List<ProductDto>>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ProductDto>>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<ProductDto>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<ProductDto>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<ProductDto>>();
                return new ResponseModel<ProductDto>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ProductDto>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<ProductDetailsDto>> GetProductDatailesByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<ProductDetailsDto>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<ProductDetailsDto>>();
                return new ResponseModel<ProductDetailsDto>
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Unknown error."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ProductDetailsDto>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel<object>> CreateProductAsync(CreateProductDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Product", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = error.ErrorMassage
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel> UpdateProductAsync(UpdateProductDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Product/{dto.Id}", dto);

                if (response.IsSuccessStatusCode)
                {
                    return new BaseResponseModel { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Update failed due to unknown error."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return new BaseResponseModel { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = error?.ErrorMassage ?? "Failed to delete due to unknown error."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Success = false,
                    ErrorMassage = $"Error during connection: {ex.Message}"
                };
            }
        }
    }
}
