using System.Data.SqlClient;

namespace BookingWPF
{
    public static class DatabaseConnection
    {
        private static readonly string _connectionString = 
            "Server=95.31.128.97,1433;" +
            "Database=Bookingwpf;" +
            "User Id=admin;" +
            "Password=winServer=;" +
            "TrustServerCertificate=True;" +
            "Encrypt=True;";

        public static bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
} 
