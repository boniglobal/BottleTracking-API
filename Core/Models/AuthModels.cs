namespace Core.Models
{
    public class AuthModels
    {
        public class AuthRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool? RememberMe { get; set; }
        }
        public class AuthResponse
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string AccessToken { get; set; }
            public DateTimeOffset TokenExpiration { get; set; }
        }
    }
}
