using AutoMapper;
using CarpoolService.Common.Exceptions;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarpoolService.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBCryptService _bcrypt;
        private readonly IMapper _mapper;
        private IEnumerable<DTO.User> _users; 


        public UserService(IUserRepository userRepository, IBCryptService bcrypt, IMapper mapper)
        {
            _userRepository = userRepository;
            _bcrypt = bcrypt;
            _mapper = mapper;
        }

        private async Task<IEnumerable<DTO.User>> GetUsersAsync()
        {
            _users ??= await _userRepository.GetAllUsers();
            return _users;
        }

        public async Task<DTO.User> RegisterUserAsync(User user)
        {
            try
            {
                string hashedPassword = _bcrypt.HashPassword(user.Password);
                User userEntity = new()
                {
                    Email = user.Email,
                    Password = hashedPassword,
                    UserName = user.UserName,
                    Image = user.Image
                };
                return await _userRepository.RegisterUser(userEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user.", ex);
            }
        }

        public async Task<DTO.User> UpdateUserAsync(int userId, User updatedUser)
        {
            try
            {
                DTO.User existingUserDTO = await GetUserByIdAsync(userId) ?? throw new NotFoundException("User not found");
                existingUserDTO.Email = updatedUser.Email;
                existingUserDTO.UserName = updatedUser.UserName;
                existingUserDTO.Image = updatedUser.Image;
                User existingUser = _mapper.Map<User>(existingUserDTO); 
                return await _userRepository.UpdateUser(existingUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user.", ex);
            }
        }

        public async Task<DTO.User> AuthenticateUserAsync(Login loginUser)
        {
            try
            {
                IEnumerable<DTO.User> users = await GetUsersAsync();
                DTO.User user = users.FirstOrDefault(u => u.Email == loginUser.Email) ?? throw new NotFoundException("User not found");

                if (!_bcrypt.VerifyPassword(loginUser.Password, user.Password))
                {
                    throw new UnauthorizedAccessException("Invalid password.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user.", ex);
            }
        }

        public async Task<DTO.User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _userRepository.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by ID.", ex);
            }
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            try
            {
                IEnumerable<DTO.User> users = await GetUsersAsync();
                return users.Any(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if email is taken.", ex);
            }
        }
    }
}
