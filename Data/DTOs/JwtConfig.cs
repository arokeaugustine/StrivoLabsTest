namespace StrivoLabsTest.Data.DTOs
{
    public class JwtConfig
    {
        public string Secret { get; set; } = null!;
        public int RefreshTokenTTl { get; set; }
        public int TokenValidityPeriod { get; set; }
    }
}
