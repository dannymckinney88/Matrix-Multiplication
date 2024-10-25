// Program.cs
using System;
using System.Threading.Tasks;

namespace MatrixMultiplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var apiService = new ApiService();

            try
            {
                int matrixSize = 1000; 
                string matrixInitUrl = $"https://recruitment-test.investcloud.com/api/numbers/init/{matrixSize}";

                Console.WriteLine("Initializing Matrices...");
                string matrixInitData = await apiService.GetApiDataInitAsync(matrixInitUrl);
                Console.WriteLine("Matrices Initialized.");
                Console.WriteLine("\nInitialization Response:");
                Console.WriteLine(matrixInitData);

                var matrixService = new MatrixService(apiService, matrixSize);

                // Fetch Matrix A
                double[,] matrixA = await matrixService.FetchMatrixAsync("A");
                matrixService.DisplaySampleData(matrixA, "A");

                // Fetch Matrix B
                double[,] matrixB = await matrixService.FetchMatrixAsync("B");
                matrixService.DisplaySampleData(matrixB, "B");

                // Continue with matrix multiplication or other operations...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
