// Services/MatrixProcessor.cs
using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Globalization;

namespace MatrixMultiplication.Services
{
    public class MatrixProcessor
    {
        public async Task ProcessMatricesAsync(double[,] matrixA, double[,] matrixB, int size, ApiService apiService)
        {
            // Start total timer
            var totalStartTime = DateTime.Now;

            // Multiply matrices
            double[,] resultMatrix = MultiplyMatrices(matrixA, matrixB, size);

            // Concatenate result
            string concatenatedResult = ConcatenateMatrix(resultMatrix, size);

            // Compute MD5 hash
            string hashString = ComputeMD5Hash(concatenatedResult);

            // Submit the hash to the server
            await SubmitHashAsync(hashString, apiService);

            // End total timer
            var totalEndTime = DateTime.Now;
            var totalDuration = totalEndTime - totalStartTime;
            Console.WriteLine($"Total execution time: {totalDuration.TotalSeconds} seconds.");
        }

        // Change method to public for testing
        public double[,] MultiplyMatrices(double[,] matrixA, double[,] matrixB, int size)
        {
            double[,] result = new double[size, size];
            Console.WriteLine("Starting matrix multiplication...");

            var multiplicationStartTime = DateTime.Now;

            Parallel.For(0, size, i =>
            {
                for (int j = 0; j < size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < size; k++)
                    {
                        sum += matrixA[i, k] * matrixB[k, j];
                    }
                    result[i, j] = sum;
                }
            });

            var multiplicationEndTime = DateTime.Now;
            var multiplicationDuration = multiplicationEndTime - multiplicationStartTime;
            Console.WriteLine($"Matrix multiplication completed in {multiplicationDuration.TotalSeconds} seconds.\n");

            return result;
        }

        private string ConcatenateMatrix(double[,] matrix, int size)
        {
            Console.WriteLine("Starting result concatenation...");
            var concatenationStartTime = DateTime.Now;

            StringBuilder sb = new StringBuilder(size * size * 10); 

            for (int j = 0; j < size; j++) // Outer loop over columns
            {
                for (int i = 0; i < size; i++) // Inner loop over rows
                {
             
                    sb.Append(matrix[j, i]);
                }
            }

            string concatenatedResult = sb.ToString();

            var concatenationEndTime = DateTime.Now;
            var concatenationDuration = concatenationEndTime - concatenationStartTime;
            Console.WriteLine($"Result concatenation completed in {concatenationDuration.TotalSeconds} seconds.\n");

            return concatenatedResult;
        }

        private string ComputeMD5Hash(string input)
        {
            Console.WriteLine("Computing MD5 hash...");
            var md5StartTime = DateTime.Now;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert hash bytes to a base64 string (since we should not use Hex)
                string hashString = Convert.ToBase64String(hashBytes);

                var md5EndTime = DateTime.Now;
                var md5Duration = md5EndTime - md5StartTime;
                Console.WriteLine($"MD5 computation completed in {md5Duration.TotalSeconds} seconds.\n");

                Console.WriteLine($"Computed MD5 Hash: {hashString}\n");
                return hashString;
            }
        }

        private async Task SubmitHashAsync(string hashString, ApiService apiService)
        {
            Console.WriteLine("Submitting hash to the server...");

            string submissionUrl = "https://recruitment-test.investcloud.com/api/numbers/validate";
            string jsonPayload = JsonConvert.SerializeObject(hashString);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await apiService.PostAsync(submissionUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Submission Response:");
            Console.WriteLine(responseContent);
        }
    }
}
