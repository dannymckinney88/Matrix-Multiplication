// MatrixService.cs
using System;
using System.Threading.Tasks;

namespace MatrixMultiplication
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

            Console.WriteLine($"Fetching Matrix {matrixName}...");

            for (int i = 0; i < _matrixSize; i++)
            {
                double[] row = await _apiService.GetMatrixRowAsync(matrixName, "row", i);
                for (int j = 0; j < _matrixSize; j++)
                {
                    matrix[i, j] = row[j];
                }

                if (i % 100 == 0)
                {
                    Console.WriteLine($"Matrix {matrixName}: Retrieved row {i}");
                }
            }

            Console.WriteLine($"Matrix {matrixName} Data Retrieved.\n");
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
        }
    }
}
