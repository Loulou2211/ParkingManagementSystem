using MySqlConnector;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services;

public class VehicleService
{
    private readonly DatabaseConnection _database;

    public VehicleService(DatabaseConnection database)
    {
        _database = database;
    }

    public async Task<List<Vehicle>> GetVehiclesAsync()
    {
        var vehicles = new List<Vehicle>();

        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
                           SELECT Vehicle_ID, License_Plate, Vehicle_Type, Customer_ID
                           FROM Vehicle
                           ORDER BY Vehicle_ID;
                           """;

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            vehicles.Add(new Vehicle
            {
                Vehicle_ID = reader.GetInt32("Vehicle_ID"),
                License_Plate = reader.GetString("License_Plate"),
                Vehicle_Type = reader.GetString("Vehicle_Type"),
                Customer_ID = reader.GetInt32("Customer_ID")
            });
        }

        return vehicles;
    }

    public async Task AddVehicleAsync(Vehicle vehicle)
    {
        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
                           INSERT INTO Vehicle (License_Plate, Vehicle_Type, Customer_ID)
                           VALUES (@plate, @type, @customerId);
                           """;

        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@plate", vehicle.License_Plate);
        command.Parameters.AddWithValue("@type", vehicle.Vehicle_Type);
        command.Parameters.AddWithValue("@customerId", vehicle.Customer_ID);

        await command.ExecuteNonQueryAsync();
    }
}