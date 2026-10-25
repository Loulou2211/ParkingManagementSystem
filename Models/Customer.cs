namespace ParkingManagementSystem.Models;

public class Customer
{
    public int Customer_ID { get; set; }
    public string Customer_Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Membership_Status { get; set; } = "Active";
}