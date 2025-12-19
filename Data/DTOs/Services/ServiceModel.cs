namespace StrivoLabsTest.Data.DTOs.Services
{
    public class ServiceModel
    {
        public string Name { get; set; } = string.Empty;

        public Guid ServiceId { get; set; }

        public bool IsActive { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
