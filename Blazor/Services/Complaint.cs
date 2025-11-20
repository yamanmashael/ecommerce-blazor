using Blazor.Data;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class Complaint
    {
        private readonly HttpClient _httpClient;

        public Complaint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<object>> ComplaintAsync(ComplaintForm form)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Complaint", form);
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
    }
}
