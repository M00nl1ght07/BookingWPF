using System;
using System.Data.SqlClient;

namespace BookingWPF
{
    public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Server=95.31.128.97,1433;" +
            "Database=HotelBookingDB;" +
            "User Id=admin;" +
            "Password=winServer=;" +
            "Encrypt=True;" +
            "TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        public static bool TestConnection()
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // Вспомогательный метод для выполнения команд без возврата данных
        public static void ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ошибка выполнения запроса: " + ex.Message);
                    }
                }
            }
        }

        // Вспомогательный метод для получения данных
        public static SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                connection.Dispose();
                throw new Exception("Ошибка выполнения запроса: " + ex.Message);
            }
        }
    }
}
