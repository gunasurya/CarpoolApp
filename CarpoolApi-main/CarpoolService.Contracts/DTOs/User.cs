namespace CarpoolService.Contracts.DTOs
{
    public class User
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Image { get; set; }
    }
}
