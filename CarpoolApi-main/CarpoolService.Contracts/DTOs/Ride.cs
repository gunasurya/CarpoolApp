namespace CarpoolService.Contracts.DTOs
{
    public class Ride
    {

        public Guid Id { get; set; }

        public int DepartureCityId { get; set; }

        public int DestinationCityId { get; set; }

        public string Stops { get; set; } = null!;

        public string TimeSlot { get; set; } = null!;

        public DateTime Date { get; set; }

        public int AvailableSeats { get; set; }

        public bool RideStatus { get; set; }

        public string Fare { get; set; }

        public string DriverImage { get; set; }

        public string DepartureCityName { get; set; }

        public string DestinationCityName { get; set; }
        public string? DriverName { get; set; }
        public int DriverId { get; set; }

    }
}
