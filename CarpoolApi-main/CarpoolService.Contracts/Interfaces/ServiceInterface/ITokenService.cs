using DTO = CarpoolService.Contracts.DTOs;

namespace CarPoolService.Contracts.Interfaces.Service_Interface
{
    public interface ITokenService
    {
        string GenerateToken(string issuer, string audience, string key, DTO.User authenticatedUser);
    }
}
