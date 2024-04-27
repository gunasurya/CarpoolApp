namespace CarpoolService.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? customMessage = "") : base(customMessage) { }
    }
}
