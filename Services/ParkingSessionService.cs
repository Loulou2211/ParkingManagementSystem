using MySqlConnector;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services;

public class ParkingSessionService
{
    private readonly DatabaseConnection _database;

    public ParkingSessionService(DatabaseConnection database)
    {
        _database = database;
    }

    public async Task<List<ParkingSession>> GetSessionsAsync()
    {
        var sessions = new List<ParkingSession>();

        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT Session_ID, Vehicle_ID, Space_ID, Staff_ID,
                   Entry_Time, Exit_Time, Duration, Amount,
                   Payment_Method, Payment_Status, Session_Status
            FROM Parking_Session
            ORDER BY Session_ID DESC;
            """;

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            sessions.Add(new ParkingSession
            {
                Session_ID = reader.GetInt32("Session_ID"),
                Vehicle_ID = reader.GetInt32("Vehicle_ID"),
                Space_ID = reader.GetInt32("Space_ID"),
                Staff_ID = reader.GetInt32("Staff_ID"),
                Entry_Time = reader.GetDateTime("Entry_Time"),
                Exit_Time = reader.IsDBNull(reader.GetOrdinal("Exit_Time"))
                    ? null
                    : reader.GetDateTime("Exit_Time"),
                Duration = reader.IsDBNull(reader.GetOrdinal("Duration"))
                    ? null
                    : reader.GetInt32("Duration"),
                Amount = reader.GetDecimal("Amount"),
                Payment_Method = reader.IsDBNull(reader.GetOrdinal("Payment_Method"))
                    ? null
                    : reader.GetString("Payment_Method"),
                Payment_Status = reader.IsDBNull(reader.GetOrdinal("Payment_Status"))
                    ? null
                    : reader.GetString("Payment_Status"),
                Session_Status = reader.IsDBNull(reader.GetOrdinal("Session_Status"))
                    ? null
                    : reader.GetString("Session_Status")
            });
        }

        return sessions;
    }

    public async Task AddSessionAsync(ParkingSession session)
    {
        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO Parking_Session
                (Vehicle_ID, Space_ID, Staff_ID, Payment_Method,
                 Payment_Status, Session_Status)
            VALUES
                (@vehicleId, @spaceId, @staffId, @paymentMethod,
                 @paymentStatus, @sessionStatus);
            """;

        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@vehicleId", session.Vehicle_ID);
        command.Parameters.AddWithValue("@spaceId", session.Space_ID);
        command.Parameters.AddWithValue("@staffId", session.Staff_ID);
        command.Parameters.AddWithValue("@paymentMethod",
            (object?)session.Payment_Method ?? DBNull.Value);
        command.Parameters.AddWithValue("@paymentStatus",
            session.Payment_Status ?? "Unpaid");
        command.Parameters.AddWithValue("@sessionStatus",
            session.Session_Status ?? "Active");

        await command.ExecuteNonQueryAsync();
    }
}
