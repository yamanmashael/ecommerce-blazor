using Blazor.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Blazor
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<T> GetFromJsonAsync<T>(string path)
        {
            return _httpClient.GetFromJsonAsync<T>(path);
        }



        public async Task<T1> PostAsync<T1, T2>(string path, T2 postModel)
        {
            var res = await _httpClient.PostAsJsonAsync(path, postModel);

            if (res != null && res.IsSuccessStatusCode)
            {
                //var json = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
            }

            return default;
        }





    }
}




