using MySqlConnector;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services;

public class StaffService
{
    private readonly DatabaseConnection _database;

    public StaffService(DatabaseConnection database)
    {
        _database = database;
    }

    public async Task<List<Staff>> GetStaffAsync()
    {
        var staffMembers = new List<Staff>();

        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
                           SELECT Staff_ID, Staff_Name, Phone, Position
                           FROM Staff
                           ORDER BY Staff_ID;
                           """;

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            staffMembers.Add(new Staff
            {
                Staff_ID = reader.GetInt32("Staff_ID"),
                Staff_Name = reader.GetString("Staff_Name"),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                    ? null
                    : reader.GetString("Phone"),
                Position = reader.GetString("Position")
            });
        }

        return staffMembers;
    }
}