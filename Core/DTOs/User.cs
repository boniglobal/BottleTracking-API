namespace Core.DTOs
{
    public class User
    {
        public class UserInfo
        {
            public string SessionId { get; set; }
            public int Id { get; set; }
            public string Email { get; set; }
            public int Type { get; set; }
            public int? StationId { get; set; }
        }
    }
}
