
using MySqlConnector;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services;

public class ParkingSpaceService
{
    private readonly DatabaseConnection _database;

    public ParkingSpaceService(DatabaseConnection database)
    {
        _database = database;
    }

    public async Task<List<ParkingSpace>> GetParkingSpacesAsync()
    {
        var spaces = new List<ParkingSpace>();

        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT Space_ID, Space_Number, Space_Type, Status, Floor
            FROM Parking_Space
            ORDER BY Space_ID;
            """;

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            spaces.Add(new ParkingSpace
            {
                Space_ID = reader.GetInt32("Space_ID"),
                Space_Number = reader.GetString("Space_Number"),
                Space_Type = reader.GetString("Space_Type"),
                Status = reader.GetString("Status"),
                Floor = reader.GetInt32("Floor")
            });
        }

        return spaces;
    }

    public async Task AddParkingSpaceAsync(ParkingSpace space)
    {
        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO Parking_Space
                (Space_Number, Space_Type, Status, Floor)
            VALUES
                (@number, @type, @status, @floor);
            """;

        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@number", space.Space_Number);
        command.Parameters.AddWithValue("@type", space.Space_Type);
        command.Parameters.AddWithValue("@status", space.Status);
        command.Parameters.AddWithValue("@floor", space.Floor);

        await command.ExecuteNonQueryAsync();
    }
}
