namespace Core.Utilities.JWT
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public string SessionId { get; set; }
        public DateTimeOffset Expires { get; set; }
        public bool RememberMe { get; set; }
    }
}
