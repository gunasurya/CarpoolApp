using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using DTO = CarpoolService.Contracts.DTOs;

namespace CarPoolService.Contracts.Interfaces.Service_Interface
{
    public interface IUserService
    {
        Task<DTO.User> RegisterUserAsync(User user);
        Task<DTO.User> UpdateUserAsync(int userId, User updatedUser);
        Task<DTO.User> AuthenticateUserAsync(Login loginUser);
        Task<DTO.User> GetUserByIdAsync(int userId);
        Task<bool> IsEmailTakenAsync(string email);
    }
}
