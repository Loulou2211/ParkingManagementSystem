namespace ParkingManagementSystem.Models
{
    public class ParkingSpace
    {
        public int Space_ID { get; set; }

        public string Space_Number { get; set; } = "";

        public string Space_Type { get; set; } = "";

        public string Status { get; set; } = "Available";

        public int Floor { get; set; }
    }
}