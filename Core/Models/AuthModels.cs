namespace Core.Models
{
    public class AuthModels
    {
        public class AuthRequest
        {
            /// <summary>Kullanıcı e-posta adresi </summary>
            /// <example>hakanyaman@boniglobal.com</example>
            public string Email { get; set; }
            /// <summary>Kullanıcı şifresi</summary>
            /// <example>Hakan245Yaman</example>
            public string Password { get; set; }
            /// <summary></summary>
            /// <example>true</example>
            public bool? RememberMe { get; set; }
        }
        public class AuthResponse
        {
            /// <summary>Kullanıcı adı ve soyadı</summary>
            /// <example>Hakan Yaman</example>
            public string Name { get; set; }
            /// <summary>Kullanıcı e-posta adresi</summary>
            /// <example>hakanyaman@boniglobal.com</example>
            public string Email { get; set; }
            /// <summary>Kullanıcı tipi</summary>
            /// <example>Admin</example>
            public string UserType { get; set; }
            /// <summary>Sisteme erişim için oluşturulan token</summary>
            /// <example>eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXN</example>
            public string AccessToken { get; set; }
            /// <summary>Sisteme erişim için oluşturulan tokenın geçerlilik süresi</summary>
            /// <example>2022-04-09T12:45:00.000Z</example>
            public DateTimeOffset TokenExpiration { get; set; }
        }
    }
}
