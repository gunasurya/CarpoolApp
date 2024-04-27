using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolService.Models.DBModels
{
    [Table("Bookings")]
    public partial class Booking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RideId { get; set; }

        [Required]
        public int PassengerId { get; set; }

        [Required]
        public int PickupLocationId { get; set; }

        [Required]
        public int DropLocationId { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ReservedSeats { get; set; }

        [Required]
        public int Fare { get; set; }
    }
}
