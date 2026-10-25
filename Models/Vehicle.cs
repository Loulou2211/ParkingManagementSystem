namespace ParkingManagementSystem.Models;

public class Vehicle
{
    public int Vehicle_ID { get; set; }
    public string License_Plate { get; set; } = "";
    public string Vehicle_Type { get; set; } = "";
    public int Customer_ID { get; set; }
}