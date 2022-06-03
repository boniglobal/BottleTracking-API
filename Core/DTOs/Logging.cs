namespace Core.DTOs
{
    public class Logging
    {
        public string Endpoint { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTimeOffset RequestTimestamp { get; set; }
    }
}
