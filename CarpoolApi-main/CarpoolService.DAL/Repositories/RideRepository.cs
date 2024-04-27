using AutoMapper;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.DAL;
using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using DTO = CarpoolService.Contracts.DTOs;

namespace CarpoolService.DAL.Repositories
{
    public class RideRepository : IRideRepository
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly IMapper _mapper;

        public RideRepository(CarpoolDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }

        // Method for offering a ride
        public async Task<DTO.Ride> CreateOfferRide(CarPoolRide poolRide)
        {
            try
            {
                await _dbContext.CarPoolRides.AddAsync(poolRide);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<DTO.Ride>(poolRide);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while creating an offer ride. Details: " + e.Message, e);
            }
        }

        // Method for getting offered rides for a user
        public async Task<IEnumerable<DTO.Booking>> GetAllBookedRides()
        {
            try
            {
                IEnumerable<Booking> allBookedRides = await _dbContext.Bookings.ToListAsync();
                return _mapper.Map<IEnumerable<DTO.Booking>>(allBookedRides);

            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving booked rides: " + e.Message, e);
            }
        }

        // Method for booking a ride
        public async Task<DTO.Booking> CreateBookRide(Booking bookRide)
        {
            try
            {
                await _dbContext.Bookings.AddAsync(bookRide);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<DTO.Booking>(bookRide);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while booking a ride: " + e.Message, e);
            }
        }

        // Method for getting booked rides for a user
        public async Task<IEnumerable<DTO.Ride>> GetAllOfferedRides()
        {
            try
            {
                IEnumerable<CarPoolRide> allOfferedRides = await _dbContext.CarPoolRides.ToListAsync(); 
                return _mapper.Map<IEnumerable<DTO.Ride>>(allOfferedRides);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving offered rides: " + e.Message,e);
            }
        }

        // Method for getting a list of cities
        public async Task<IEnumerable<DTO.City>> GetCities()
        {
            try
            {
                IEnumerable<City> cities = await _dbContext.Cities.ToListAsync();
                return _mapper.Map<IEnumerable<DTO.City>>(cities);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving cities: " + e.Message,e);
            }
        }

        public async Task UpdateofferedRide(CarPoolRide offeredRide)
        {
            try
            {
                _dbContext.CarPoolRides.Update(offeredRide);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating offeredRide: " + e.Message, e);
            }
        }


    }

}