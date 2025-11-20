using Blazor.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Blazor.Services
{
    public class ProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductImageDto>> GetImagesByProductItemIdAsync(int productItemId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResponseModel<List<ProductImageDto>>>($"api/ProductImages/{productItemId}");
                if (response != null && response.Success)
                {
                    return response.Data;
                }
                return new List<ProductImageDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching images: {ex.Message}");
                return new List<ProductImageDto>();
            }
        }

        public async Task<bool> AddImageAsync(ProductImageCreateDto dto)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(dto.ImageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ImageFile.ContentType);
                content.Add(fileContent, nameof(dto.ImageFile), dto.ImageFile.Name);
                content.Add(new StringContent(dto.ProductItemId.ToString()), nameof(dto.ProductItemId));

                var response = await _httpClient.PostAsync("api/ProductImages", content);
                response.EnsureSuccessStatusCode();

                var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel<int>>();
                return responseModel != null && responseModel.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding image: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ProductImages/{id}");
                response.EnsureSuccessStatusCode();

                var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel<int>>();
                return responseModel != null && responseModel.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting image: {ex.Message}");
                return false;
            }
        }
    }
}
