// Services/ApiService.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MatrixMultiplication.Models;

namespace MatrixMultiplication.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetApiDataInitAsync(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<double[]> GetMatrixRowAsync(string dataset, string type, int idx)
        {
            string apiUrl = $"https://recruitment-test.investcloud.com/api/numbers/{dataset}/{type}/{idx}";
            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            ApiResponse? apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
            if (apiResponse == null || apiResponse.Value == null)
            {
                throw new InvalidOperationException($"Failed to deserialize API response. Content: {content}");
            }

            return apiResponse.Value;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            HttpResponseMessage response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
