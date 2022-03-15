namespace Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset ExpirationTime { get; set; }
        public DateTimeOffset? RevokedTime { get; set; }
        public string SessionId { get; set; }
        public bool? HasLongTTL { get; set; }
    }
}
