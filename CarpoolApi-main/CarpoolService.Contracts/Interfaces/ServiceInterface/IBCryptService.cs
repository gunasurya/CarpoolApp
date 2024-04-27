namespace CarPoolService.Contracts.Interfaces.Service_Interface
{
    public interface IBCryptService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
