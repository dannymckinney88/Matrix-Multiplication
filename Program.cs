// Program.cs
using System;
using System.Threading.Tasks;
using MatrixMultiplication.Services;

namespace MatrixMultiplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Matrix Multiplication Program Started.");
            var apiService = new ApiService();

            try
            {
                int matrixSize = 1000; // 1000x1000 matrices
                string matrixInitUrl = $"https://recruitment-test.investcloud.com/api/numbers/init/{matrixSize}";

                // Initialize matrices on the server
                Console.WriteLine("Initializing matrices...");
                string matrixInitData = await apiService.GetApiDataInitAsync(matrixInitUrl);
                Console.WriteLine("Matrices initialized successfully.");
                Console.WriteLine("\nInitialization Response:");
                Console.WriteLine(matrixInitData);

                // Create MatrixService instance
                var matrixService = new MatrixService(apiService, matrixSize);

                // Fetch Matrix A
                double[,] matrixA = await matrixService.FetchMatrixAsync("A");
                matrixService.DisplaySampleData(matrixA, "A");

                // Fetch Matrix B
                double[,] matrixB = await matrixService.FetchMatrixAsync("B");
                matrixService.DisplaySampleData(matrixB, "B");

                // Start the multiplication and result processing
                var matrixProcessor = new MatrixProcessor();
                await matrixProcessor.ProcessMatricesAsync(matrixA, matrixB, matrixSize, apiService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Program finished.");
        }
    }
}
