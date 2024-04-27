namespace CarpoolService.Contracts.DTOs
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid RideId { get; set; }

        public int PassengerId { get; set; }

        public string? PassengerName { get; set; }

        public int PickupLocationId { get; set; }

        public int DropLocationId { get; set; }

        public string TimeSlot { get; set; } = null!;

        public DateTime Date { get; set; }

        public int ReservedSeats { get; set; }

        public string Fare { get; set; }

        public string PassengerImage { get; set; }

        public string DropLocation { get; set; }

        public string PickupLocation { get; set; }
    }
}
