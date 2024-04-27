using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DTO = CarpoolService.Contracts.DTOs;

namespace CarPoolServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public AuthenticationController(IUserService userService, ITokenService tokenService, IConfiguration config)
        {
            _userService = userService;
            _tokenService = tokenService;
            _config = config;
        }
        //Register a new user via POST request
       [HttpPost("register")]
        public async Task<ApiResponse<DTO.User>> RegisterUser([FromBody] User user)
        {
            try
            {
                bool emailExists = await _userService.IsEmailTakenAsync(user.Email);

                if (emailExists)
                {
                    return new ApiResponse<DTO.User>().CreateApiResponse(false, HttpStatusCode.BadRequest, null, "Email already taken");
                }

                DTO.User createdUser = await _userService.RegisterUserAsync(user);
                return new ApiResponse<DTO.User>().CreateApiResponse(true, HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DTO.User>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        //Authenticate user via POST request
       [HttpPost("login")]
        public async Task<ApiResponse<string>> Login([FromBody] Login user)
        {
            try
            {
                DTO.User authenticatedUser = await _userService.AuthenticateUserAsync(user);

                if (authenticatedUser == null)
                {
                    return new ApiResponse<string>().CreateApiResponse(false, HttpStatusCode.BadRequest, null, "User not Found");
                }
                var token = _tokenService.GenerateToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    _config["Jwt:Key"],
                    authenticatedUser
                );
                return new ApiResponse<string>().CreateApiResponse(true, HttpStatusCode.OK, token);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }
    }
}
