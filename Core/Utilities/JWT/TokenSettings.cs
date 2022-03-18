namespace Core.Utilities.JWT
{
    public class TokenSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenTTL { get; set; }
        public int RefreshTokenShortTTL { get; set; }
        public int RefreshTokenLongTTL { get; set; }
    }
}
