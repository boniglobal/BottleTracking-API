namespace Core.DTOs
{
    public class ExternalLog
    {
        public string Endpoint { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTimeOffset RequestTimestamp { get; set; }
    }
}
