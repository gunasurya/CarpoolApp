using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolService.Models.DBModels
{
    [Table("CarPoolRides")]
    public partial class CarPoolRide
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int DriverId { get; set; }

        [Required]
        public int DepartureCityId { get; set; }

        [Required]
        public int DestinationCityId { get; set; }

        [Required]
        public string Stops { get; set; } = null!;

        [Required]
        public string TimeSlot { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        public bool RideStatus { get; set; }

        [Required]
        public int Fare { get; set; }
    }
}
