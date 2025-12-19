namespace StrivoLabsTest.Data.DTOs.Services
{
    public class ServiceResponse
    {
        public string Name { get; set; } = string.Empty;

        public Guid ServiceId { get; set; }

        public bool IsActive { get; set; }
    }
}
