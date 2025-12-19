namespace StrivoLabsTest.Data.DTOs.Login
{
    public class LoginResponse
    {
        public Guid TokenID { get; set; } = Guid.Empty;
        public string Token { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
