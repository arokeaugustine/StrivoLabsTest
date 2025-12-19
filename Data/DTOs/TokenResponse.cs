namespace StrivoLabsTest.Data.DTOs
{
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
