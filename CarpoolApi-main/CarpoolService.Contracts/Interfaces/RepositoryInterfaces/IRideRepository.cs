using CarPoolService.Models.DBModels;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarPoolService.Contracts.Interfaces.Repository_Interfaces
{
    public interface IRideRepository
    {
        Task<DTO.Ride> CreateOfferRide(CarPoolRide poolRide);
        Task<IEnumerable<DTO.Ride>> GetAllOfferedRides();
        Task<DTO.Booking> CreateBookRide(Booking bookRide);
        Task<IEnumerable<DTO.Booking>> GetAllBookedRides();
        Task<IEnumerable<DTO.City>> GetCities();
        Task UpdateofferedRide(CarPoolRide offeredRide);
    }
}
