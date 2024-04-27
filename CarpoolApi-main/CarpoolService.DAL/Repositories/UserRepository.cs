using AutoMapper;
using CarpoolService.Common.Exceptions;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.DAL;
using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarpoolService.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(CarpoolDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Register a new user
        public async Task<DTO.User> RegisterUser(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<DTO.User>(user);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error adding a new user. Database update failed.", ex);
            }
        }

        // Update user information
        public async Task<DTO.User> UpdateUser(CarPoolService.Models.DBModels.User updatedUser)
        {
            try
            {
                _dbContext.Entry(updatedUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<DTO.User>(updatedUser);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error updating user information. Database update failed.", ex);
            }
        }


        // Get a user by ID
        public async Task<DTO.User> GetUserById(int userId)
        {
            try
            {
                User user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId) ?? throw new NotFoundException("User not found");
                return _mapper.Map<DTO.User>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by ID.", ex);
            }
        }

        // Get all the users from Database
        public async Task<IEnumerable<DTO.User>> GetAllUsers()
        {
            try
            {
                IEnumerable<User> users = await _dbContext.Users.ToListAsync();
                return _mapper.Map<IEnumerable<DTO.User>>(users);    
            }
           catch(Exception ex)
            {
                throw new Exception("Error getting users from Database.", ex);
            }

        }

        // Get the count of all users in the database       
        public async Task<int> GetUserCount()
        {
            try
            {
                int count = await _dbContext.Users.CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting the count of users from the Database.", ex);
            }
        }

    }
}
