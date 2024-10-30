// Services/MatrixService.cs
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace MatrixMultiplication.Services
{
    public class MatrixService
    {
        private readonly ApiService _apiService;
        private readonly int _matrixSize;

        public MatrixService(ApiService apiService, int matrixSize)
        {
            _apiService = apiService;
            _matrixSize = matrixSize;
        }

        public async Task<double[,]> FetchMatrixAsync(string matrixName)
        {
            double[,] matrix = new double[_matrixSize, _matrixSize];
            var logEntries = new ConcurrentBag<string>();
            Console.WriteLine($"Fetching Matrix {matrixName}...");

            // Define a list of tasks for each row
            var fetchTasks = new Task[_matrixSize];

            Parallel.For(0, _matrixSize, i =>
            {
                fetchTasks[i] = Task.Run(async () =>
                {
                    double[] row = await _apiService.GetMatrixRowAsync(matrixName, "row", i);
                    for (int j = 0; j < _matrixSize; j++)
                    {
                        matrix[i, j] = row[j];
                    }

                    // Optional: log progress every 100 rows
                    if (i % 100 == 0)
                    {
                        logEntries.Add($"Matrix {matrixName}: Retrieved row {i}");
                    }
                });
            });

            // Wait for all tasks to complete
            await Task.WhenAll(fetchTasks);
            foreach (var log in logEntries.OrderBy(log => log))
            {
                Console.WriteLine(log);
            }

            Console.WriteLine($"Matrix {matrixName} data retrieval completed.\n");
            return matrix;
        }


        public void DisplaySampleData(double[,] matrix, string matrixName)
        {
            Console.WriteLine($"Sample data from Matrix {matrixName}:");

            int displaySize = Math.Min(5, _matrixSize);

            for (int i = 0; i < displaySize; i++)
            {
                for (int j = 0; j < displaySize; j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
