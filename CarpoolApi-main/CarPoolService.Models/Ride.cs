namespace CarPoolService.Models
{
    public class Ride
    {
        public int UserId { get; set; } 
        public int StartPoint { get; set; }

        public int EndPoint { get; set; }

        public DateTime Date { get; set; }

        public string TimeSlot { get; set; }
    }
}
