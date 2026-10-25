using MySqlConnector;

namespace ParkingManagementSystem;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(IConfiguration configuration)
    {
        _connectionString = configuration
                                .GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException(
                                "Database connection string is missing.");
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}