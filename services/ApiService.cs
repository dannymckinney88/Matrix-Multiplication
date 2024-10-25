using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MatrixMultiplication
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient(); // HTTP Request
        }

        public async Task<string> GetApiDataInitAsync(string url) // Async Fetch
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<double[]> GetMatrixRowAsync(string dataset, string type, int idx)
        {
            string apiUrl = $"https://recruitment-test.investcloud.com/api/numbers/{dataset}/{type}/{idx}";

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            
            // Deserialize into ApiResponse
            ApiResponse? apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
            if (apiResponse == null)
            {
                throw new InvalidCastException($"Faild to desrialize API response. Content: {content}");
            }
            double[]? rowData = apiResponse.Value;

            if (rowData == null)
            {
                throw new InvalidOperationException($"Failed to deserialize matrix row. Dataset: {dataset}, Type: {type}, Index: {idx}, Content: {content}");
            }

            return rowData;
        }
    }

    // Define the ApiResponse class
    public class ApiResponse
    {
        public double[] Value { get; set; } = Array.Empty<double>();
        public object? Cause { get; set; }
        public bool Success { get; set; }
    }
}
