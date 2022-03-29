using static Core.Constants.UserConstants;

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
        }

        public class PanelUserGetResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public Types UserType { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
        }

        public class PanelUserAddRequest
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public Types UserType { get; set; }
        }

        public class PanelUserUpdateRequest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public Types UserType { get; set; }
        }

        public class ResetPassword
        {
            public int UserId { get; set; }
            public string Password { get; set; }
        }
    }
}
