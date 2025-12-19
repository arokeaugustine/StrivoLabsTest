namespace StrivoLabsTest.Data.DTOs.Subscription
{
    public class SubscribersRequest
    {
        public Guid Service_id { get; set; }
        public Guid Token_id { get; set; }
        public string Phone_number { get; set; }
    }
}
