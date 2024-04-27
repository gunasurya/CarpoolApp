using CarpoolService.Common.Exceptions;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.DAL;
using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarpoolService.BLL.Services
{
    public class RideService : IRideService
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly IRideRepository _rideRepository;
        private readonly IMemoryCache _cache;


        public RideService(IRideRepository rideRepository, CarpoolDbContext dbContext, IMemoryCache cache)
        {
            _rideRepository = rideRepository;
            _dbContext = dbContext;
            _cache = cache;
        }
        private static string GetCityNamesForStops(string stops, IEnumerable<DTO.City> cities)
        {
            if (string.IsNullOrEmpty(stops))
                return string.Empty;

            var stopIds = stops.Split(',').Select(int.Parse);
            var stopNames = stopIds.Select(id => cities.FirstOrDefault(c => c.Id == id)?.Name);
            return string.Join(",", stopNames);
        }
        #region OfferRide
        // Create an offer ride
        public async Task<DTO.Ride> CreateOfferRide(CarPoolRide poolRide)
        {
            try
            {
                DTO.Ride offeredRide = await _rideRepository.CreateOfferRide(poolRide);
                return offeredRide;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating an offer ride." + ex.Message, ex);
            }
        }

        // Get offered rides for a user
        public async Task<IEnumerable<DTO.Booking>> GetOfferedRides(int userId)
        {
            try
            {
                IEnumerable<DTO.Booking> allBookedRides = await _rideRepository.GetAllBookedRides();
                IEnumerable<DTO.Booking> offeredRides = allBookedRides.Where(bookedRide => _dbContext.CarPoolRides
                .Any(ride => ride.Id == bookedRide.RideId && ride.DriverId == userId)).ToList();

                IEnumerable<int> cityIds = offeredRides.Select(bookedRide => bookedRide.PickupLocationId).Union(offeredRides.Select(bookedRide => bookedRide.DropLocationId)).Distinct().ToList();
                IEnumerable<City> cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.Id)).ToListAsync();
                IEnumerable<int> passengerIds = offeredRides.Select(bookedRide => bookedRide.PassengerId).ToList();
                IEnumerable<User> passengers = await _dbContext.Users.Where(user => passengerIds.Contains(user.Id)).ToListAsync();
                IEnumerable<DTO.Booking> OfferedRideDTOs = offeredRides.Select(bookedRide => new DTO.Booking
                {
                    PickupLocation = cities.FirstOrDefault(c => c.Id == bookedRide.PickupLocationId)?.Name,
                    DropLocation = cities.FirstOrDefault(c => c.Id == bookedRide.DropLocationId)?.Name,
                    PickupLocationId = bookedRide.PickupLocationId,
                    DropLocationId = bookedRide.DropLocationId,
                    PassengerId = bookedRide.PassengerId,
                    RideId = bookedRide.RideId,
                    TimeSlot = bookedRide.TimeSlot ?? string.Empty,
                    Date = bookedRide.Date,
                    ReservedSeats = bookedRide.ReservedSeats,
                    Fare = bookedRide.Fare,
                    PassengerName = passengers.FirstOrDefault(d => d.Id == bookedRide.PassengerId)?.UserName,
                    PassengerImage = passengers.FirstOrDefault(d => d.Id == bookedRide.PassengerId)?.Image,
                }).ToList();
                return OfferedRideDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting offered rides for a user." + ex.Message, ex);
            }
        }
        #endregion

        #region BookRide
        // Create a booked ride

        public async Task<DTO.Booking> CreateBookRide(Booking bookRide)
        {
            try
            {
                CarPoolRide? offeredRide = await _dbContext.CarPoolRides.FirstOrDefaultAsync(x =>
                   x.Id == bookRide.RideId &&
                   x.AvailableSeats >= bookRide.ReservedSeats
                );
                if (offeredRide != null)
                {
                    offeredRide.AvailableSeats -= bookRide.ReservedSeats;
                    offeredRide.Fare -= bookRide.Fare;
                    if (offeredRide.AvailableSeats == 0)
                    {
                        offeredRide.RideStatus = true;
                    }
                    DTO.Booking bookedRide = await _rideRepository.CreateBookRide(bookRide);
                    await _rideRepository.UpdateofferedRide(offeredRide);
                    return bookedRide;
                }
                throw new NotFoundException("The Match Ride was not Found");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating a booked ride." + ex.Message, ex);
            }
        }

        // Get booked rides for a user
        public async Task<IEnumerable<DTO.Ride>> GetBookedRides(int userId)
        {
            try
            {
                IEnumerable<DTO.Ride> allOfferedRides = await _rideRepository.GetAllOfferedRides();
                IEnumerable<DTO.Booking> allBookedRides = await _rideRepository.GetAllBookedRides();
                var bookedRides = allBookedRides.Where(ride => ride.PassengerId == userId);
                var userRides = allOfferedRides.Where(offeredRide => bookedRides
               .Any(ride => ride.RideId == offeredRide.Id)).ToList();

                IEnumerable<int> cityIds = bookedRides.Select(ride => ride.PickupLocationId).Union(bookedRides.Select(ride => ride.DropLocationId)).Distinct().ToList();
                IEnumerable<City> cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.Id)).ToListAsync();
                IEnumerable<int> driverIds = userRides.Select(cpRide => cpRide.DriverId).ToList();
                IEnumerable<User> drivers = await _dbContext.Users.Where(user => driverIds.Contains(user.Id)).ToListAsync();
                IEnumerable<DTO.Ride> BookedRideDTOs =
                    from bookedRide in bookedRides
                    join ride in userRides
                   on bookedRide.RideId equals ride.Id
                    select new DTO.Ride
                    {
                        DepartureCityName = cities.FirstOrDefault(c => c.Id == bookedRide.PickupLocationId)?.Name,
                        DestinationCityName = cities.FirstOrDefault(c => c.Id == bookedRide.DropLocationId)?.Name,
                        DepartureCityId = bookedRide.PickupLocationId,
                        DestinationCityId = bookedRide.DropLocationId,
                        Id = bookedRide.RideId,
                        Stops = ride.Stops,
                        TimeSlot = bookedRide.TimeSlot,
                        Date = bookedRide.Date,
                        AvailableSeats = bookedRide.ReservedSeats,
                        RideStatus = ride.RideStatus,
                        Fare = bookedRide.Fare,
                        DriverName = drivers.FirstOrDefault(d => d.Id == ride.DriverId)?.UserName,
                        DriverImage = drivers.FirstOrDefault(d => d.Id == ride.DriverId)?.Image,
                    };

                return BookedRideDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting booked rides for a user." + ex.Message, ex);
            }
        }
        #endregion

        // Find matching rides for a given ride
        public async Task<IEnumerable<DTO.Ride>> FindMatchRides(CarPoolService.Models.Ride ride)
        {
            try
            {

                IEnumerable<DTO.Ride> allOfferedRides = await _rideRepository.GetAllOfferedRides();
                var allCities = await GetCities();
                IEnumerable<DTO.Ride> filteredRides = allOfferedRides.Where(offerRide => (offerRide.TimeSlot == ride.TimeSlot && offerRide.DriverId != ride.UserId) && (offerRide.Date == ride.Date && !offerRide.RideStatus)).ToList();
                IEnumerable<DTO.Ride> matchedRides = filteredRides.Where(offer => (ride.StartPoint == offer.DepartureCityId && ride.EndPoint == offer.DestinationCityId) ||
                (ride.StartPoint == offer.DepartureCityId || ride.EndPoint == offer.DestinationCityId || offer.Stops.Split(',').Select(int.Parse).Contains(ride.StartPoint) &&
                offer.Stops.Split(',').Select(int.Parse).Contains(ride.EndPoint))).ToList();

                IEnumerable<int> cityIds = matchedRides.Select(cpRide => cpRide.DepartureCityId).Union(matchedRides.Select(cpRide => cpRide.DestinationCityId)).Distinct().ToList();
                IEnumerable<City> cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.Id)).ToListAsync();
                IEnumerable<int> driverIds = matchedRides.Select(cpRide => cpRide.DriverId).ToList();
                IEnumerable<User> drivers = await _dbContext.Users.Where(user => driverIds.Contains(user.Id)).ToListAsync();
                IEnumerable<DTO.Ride> matchedRideDTOs = matchedRides.Select(cpRide => new DTO.Ride
                {
                    DepartureCityName = cities.FirstOrDefault(c => c.Id == cpRide.DepartureCityId)?.Name,
                    DestinationCityName = cities.FirstOrDefault(c => c.Id == cpRide.DestinationCityId)?.Name,
                    DepartureCityId = cpRide.DepartureCityId,
                    DestinationCityId = cpRide.DestinationCityId,
                    DriverId = cpRide.DriverId,
                    Id = cpRide.Id,
                    Stops = GetCityNamesForStops(cpRide.Stops, allCities),
                    TimeSlot = cpRide.TimeSlot,
                    Date = cpRide.Date,
                    AvailableSeats = cpRide.AvailableSeats,
                    RideStatus = cpRide.RideStatus,
                    Fare = cpRide.Fare,
                    DriverName = drivers.FirstOrDefault(d => d.Id == cpRide.DriverId)?.UserName,
                    DriverImage = drivers.FirstOrDefault(d => d.Id == cpRide.DriverId)?.Image,
                }).ToList();

                return matchedRideDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting match rides for a user." + ex.Message, ex);
            }
        }

        // Get a list of cities 
        public async Task<IEnumerable<DTO.City>> GetCities()
        {
            const string cacheKey = "CitiesCacheKey";

            //retrieve's cities from the cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<DTO.City> cities))
            {
                return cities;
            }

            try
            {
                cities = await _rideRepository.GetCities();
                // Cache the fetched cities for a specified duration 
                _cache.Set(cacheKey, cities, TimeSpan.FromMinutes(15));

                return cities;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting cities: " + ex.Message, ex);
            }
        }


    }
}