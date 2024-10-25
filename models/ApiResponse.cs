// Models/ApiResponse.cs
namespace MatrixMultiplication.Models
{
    public class ApiResponse
    {
        public double[] Value { get; set; } = System.Array.Empty<double>();
        public object? Cause { get; set; }
        public bool Success { get; set; }
    }
}
