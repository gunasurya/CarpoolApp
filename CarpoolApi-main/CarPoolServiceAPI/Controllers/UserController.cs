using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CarpoolDTOs = CarpoolService.Contracts.DTOs;

namespace CarPoolServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Update user information via PUT request
        [HttpPut("update/{userId}")]
        public async Task<ApiResponse<CarpoolDTOs.User>> UpdateUser([FromRoute] int userId, [FromBody] User user)
        {
            try
            {
                CarpoolDTOs.User updatedUser = await _userService.UpdateUserAsync(userId, user);
                if (updatedUser == null)
                {
                    return new ApiResponse<CarpoolDTOs.User>().CreateApiResponse(false, HttpStatusCode.NotFound, null, "User not found");
                }
                return new ApiResponse<CarpoolDTOs.User>().CreateApiResponse(true, HttpStatusCode.OK, updatedUser);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CarpoolDTOs.User>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get user by ID via GET request
        [HttpGet("{userId}")]
        public async Task<ApiResponse<CarpoolDTOs.User>> GetUserById([FromRoute] int userId)
        {
            try
            {
                CarpoolDTOs.User user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<CarpoolDTOs.User>().CreateApiResponse(false, HttpStatusCode.NotFound, null, "User not found");
                }
                return new ApiResponse<CarpoolDTOs.User>().CreateApiResponse(true, HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CarpoolDTOs.User>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

    }
}
