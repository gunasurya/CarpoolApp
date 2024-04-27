﻿
using CarPoolService.Contracts.Interfaces.Service_Interface;

namespace CarpoolService.BLL.Services
{
    public class BCryptAdapterService:IBCryptService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
