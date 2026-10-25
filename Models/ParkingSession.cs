namespace ParkingManagementSystem.Models;

public class ParkingSession
{
    public int Session_ID { get; set; }
    public int Vehicle_ID { get; set; }
    public int Space_ID { get; set; }
    public int Staff_ID { get; set; }

    public DateTime Entry_Time { get; set; }
    public DateTime? Exit_Time { get; set; }

    public int? Duration { get; set; }
    public decimal Amount { get; set; }

    public string? Payment_Method { get; set; }
    public string? Payment_Status { get; set; }
    public string? Session_Status { get; set; }
}