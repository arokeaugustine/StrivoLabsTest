namespace StrivoLabsTest.Data.DTOs
{
    public class SubscribersRequest
    {
        public Guid Service_id { get; set; }
        public Guid Token_id { get; set; }
        public string Phone_number { get; set; }
    }
}
