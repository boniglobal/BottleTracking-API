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
            public int? StationId { get; set; }
        }

        public class PanelUserGetResponse
        {
            /// <summary>Kullanıcının benzersiz kimlik numarası</summary>
            /// <example>1</example>
            public int Id { get; set; }
            /// <summary>Kullanıcının adı</summary>
            /// <example>Uğur</example>
            public string Name { get; set; }
            /// <summary>Kullanıcının soyadı</summary>
            /// <example>Timurçin</example>
            public string Surname { get; set; }
            /// <summary>Kullanıcının e-posta adresi</summary>
            /// <example>ugurtimurcin@boniglobal.com</example>
            public string Email { get; set; }
            /// <summary>Kullanıcı türü</summary>
            /// <example>1</example>
            public Types UserType { get; set; }
            /// <summary>Kullanıcının sisteme kaydedildiği tarih</summary>
            /// <example>2022-04-06T09:24:50.375521+00:00</example>
            public DateTimeOffset CreatedDate { get; set; }
        }

        public class PanelUserAddRequest
        {
            /// <summary>Sisteme kaydedilecek kullanıcının adı</summary>
            /// <example>Rıza</example>
            public string Name { get; set; }
            /// <summary>Sisteme kaydedilecek kullanıcının soyadı</summary>
            /// <example>Özdülger</example>
            public string Surname { get; set; }
            /// <summary>Sisteme kaydedilecek kullanıcının e-posta adresi</summary>
            /// <example>riza@boniglobal.com</example>
            public string Email { get; set; }
            /// <summary>Sisteme kaydedilecek kullanıcı için belirlenen şifre</summary>
            /// <example>riza123</example>
            public string Password { get; set; }
            /// <summary>
            /// Kullanıcı türü
            /// </summary>
            /// <example>2</example>
            public Types UserType { get; set; }
        }

        public class PanelUserUpdateRequest
        {
            /// <summary>Güncellenecek kullanıcının benzersiz kimlik numarası</summary>
            /// <example>2</example>
            public int Id { get; set; }
            /// <summary>Güncellenecek kullanıcının adı</summary>
            /// <example>Arın Deniz</example>
            public string Name { get; set; }
            /// <summary>Güncellenecek kullanıcının soyadı</summary>
            /// <example>Köklü</example>
            public string Surname { get; set; }
            /// <summary>Güncellenecek kullanıcının e-posta adresi</summary>
            /// <example>deniz@boniglobal.com</example>
            public string Email { get; set; }
            /// <summary>Güncellenecek kullanıcının türü</summary>
            /// <example>1</example>
            public Types UserType { get; set; }
        }

        public class ResetPassword
        {
            /// <summary>Şifresi sıfırlanacak kullanıcının benzersiz kimlik numarası</summary>
            /// <example>1</example>
            public int UserId { get; set; }
            /// <summary>Belirlenen yeni şifre</summary>
            /// <example>Sifre123</example>
            public string Password { get; set; }
        }
    }
}
