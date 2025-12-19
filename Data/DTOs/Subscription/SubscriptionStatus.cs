namespace StrivoLabsTest.Data.DTOs.Subscription
{
    public class SubscriptionStatus
    {
        public string Status { get; set; } = string.Empty;
        public bool IsSubScribed { get; set; }
        public DateTime? SubscribedAt { get; set; }
        public DateTime? UnsubscribedAt { get; set; }
    }
}
