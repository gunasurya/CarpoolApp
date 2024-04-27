using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DTO = CarpoolService.Contracts.DTOs;

namespace CarPoolServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarpoolController : ControllerBase
    {
        private readonly IRideService _rideService;

        public CarpoolController(IRideService rideService)
        {
            _rideService = rideService;
        }

        // Create a new offer ride via POST request
        [HttpPost("offerARide")]
        public async Task<ApiResponse<DTO.Ride>> OfferARide([FromBody] CarPoolRide ride)
        {
            try
            {
                DTO.Ride offeredRide = await _rideService.CreateOfferRide(ride);
                return new ApiResponse<DTO.Ride>().CreateApiResponse(true, HttpStatusCode.OK, offeredRide);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DTO.Ride>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get offered rides for a user via GET request
        [HttpGet("ridesOffered/{userId}")]
        public async Task<ApiResponse<IEnumerable<DTO.Booking>>> GetRidesOffered([FromRoute] int userId)
        {
            try
            {
                IEnumerable<DTO.Booking> offeredRides = await _rideService.GetOfferedRides(userId);
                return new ApiResponse<IEnumerable<DTO.Booking>>().CreateApiResponse(true, HttpStatusCode.OK, offeredRides);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DTO.Booking>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Create a new booked ride via POST request
        [HttpPost("bookARide")]
        public async Task<ApiResponse<DTO.Booking>> BookARide([FromBody] Booking booking)
        {
            try
            {
                DTO .Booking bookedRide = await _rideService.CreateBookRide(booking);
                return new ApiResponse<DTO.Booking>().CreateApiResponse(true, HttpStatusCode.OK, bookedRide);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DTO.Booking>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get booked rides for a user via GET request
        [HttpGet("ridesBooked/{userId}")]
        public async Task<ApiResponse<IEnumerable<DTO.Ride>>> GetRidesBooked([FromRoute] int userId)
        {
            try
            {
                IEnumerable<DTO.Ride> bookedRides = await _rideService.GetBookedRides(userId);
                return new ApiResponse<IEnumerable<DTO.Ride>>().CreateApiResponse(true, HttpStatusCode.OK, bookedRides);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DTO.Ride>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Find matching rides for a given ride via POST request
        [HttpPost("matchRides")]
        public async Task<ApiResponse<IEnumerable<DTO.Ride>>> FindMatchingRides([FromBody] Ride ride)
        {
            try
            {
                IEnumerable<DTO.Ride> matchedRides = await _rideService.FindMatchRides(ride);
                return new ApiResponse<IEnumerable<DTO.Ride>>().CreateApiResponse(true, HttpStatusCode.OK, matchedRides);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DTO.Ride>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get a list of cities via GET request
        [HttpGet("cities")]
        public async Task<ApiResponse<IEnumerable<DTO.City>>> GetCities()
        {
            try
            {
                IEnumerable<DTO.City> cities = await _rideService.GetCities();
                return new ApiResponse<IEnumerable<DTO.City>>().CreateApiResponse(true, HttpStatusCode.OK, cities);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DTO.City>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

    }
}