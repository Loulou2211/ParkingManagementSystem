using MySqlConnector;
using ParkingManagementSystem.Models;
namespace ParkingManagementSystem.Services;

public class CustomerService
{
    private readonly DatabaseConnection _database;

    public CustomerService(DatabaseConnection database)
    {
        _database = database;
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        var customers = new List<Customer>();

        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
                           SELECT Customer_ID, Customer_Name, Phone,
                                  Email, Membership_Status
                           FROM Customer
                           ORDER BY Customer_ID;
                           """;

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            customers.Add(new Customer
            {
                Customer_ID = reader.GetInt32("Customer_ID"),
                Customer_Name = reader.GetString("Customer_Name"),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                    ? null : reader.GetString("Phone"),
                Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                    ? null : reader.GetString("Email"),
                Membership_Status =
                    reader.GetString("Membership_Status")
            });
        }

        return customers;
    }
    
    public async Task AddCustomerAsync(Customer customer)
    {
        using var connection = _database.GetConnection();
        await connection.OpenAsync();

        const string sql = """
                           INSERT INTO Customer
                               (Customer_Name, Phone, Email, Membership_Status)
                           VALUES
                               (@name, @phone, @email, @status);
                           """;

        using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@name", customer.Customer_Name);
        command.Parameters.AddWithValue("@phone",
            (object?)customer.Phone ?? DBNull.Value);
        command.Parameters.AddWithValue("@email",
            (object?)customer.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", customer.Membership_Status);

        await command.ExecuteNonQueryAsync();
    }
}