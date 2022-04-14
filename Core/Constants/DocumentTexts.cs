namespace Core.Constants
{
    public class DocumentTexts
    {
        public const string AuthDesc = "Bu sistemde giriş yetki kontolü için JWT yapısı kullanılmaktadır.</br>" +
                    "Sisteme giriş yapmak için <a href = '#post-/token/authenticate'><b>authenticate</b></a>" +
                    " uç noktasından alınan token kullanılmalıdır." +
                    " Alınan token metin kutusuna başına bearer yazılarak eklenmelidir.<br /> Örneğin, bearer [alınan token]";
        public static class Bottle
        {
            public const string ControllerDesc = "Takibi yapılan, yapılacak damacanaların yönetimine olanak sağlar. " +
                "Sisteme kayıtlı damacanaların ayrıntılarını alır; damacana ekler, günceller ve siler." +
                "Bu işlemleri gerçekleştirebilmek için gerekli yetkilere sahip olunmalıdır.";
            public const string GetDesc = "Belirtilen benzersiz kimlik numarasına sahip " +
                "damacana detaylarını alır.<br /> " +
                "Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string GetbyQrDesc = "Belirtilen QR koduna sahip damacana detaylarını alır.<br /> " +
                "Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string CheckStatusDesc = "Belirtilen benzersiz takip numarasına (TrackingId) sahip " +
                "damacananın kullanım durumu detaylarını alır.<br /> " +
                "Bu işlemi yalnızca <b>Admin, Panel, Kiosk</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string GetAllDesc = "Belirtilen filtrelere göre sisteme kayıtlı damacanaların " +
                "detaylarını alır.<br /> " +
                "Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string GetStatisticsDesc = "Sisteme kayıtlı damacanalar ile ilgili istatistiki " +
                "bilgileri alır.<br /> Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PostDesc = "Sisteme yeni bir damacana ekler.<br /> " +
                "Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PutDesc = "Belirtilen benzersiz kimlik numarasına sahip " +
                "damacanayı günceller. <br /> Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string DeleteDesc = "Belirtilen benzersiz kimlik numaralarına " +
                "sahip damacanaları sistemden siler." +
                "<br /> Bu işlemi yalnızca <b>Admin, Panel, Printer</b> türündeki kullanıcılar gerçekleştirebilir.";
        }

        public static class PanelUser
        {
            public const string ControllerDesc = "Kullanıcı yönetimine olanak sağlar. İlgili uç noktalar " +
                "kullanıcı detayını alır; yeni bir kullanıcı ekler, günceller ve siler. " +
                "Bu işlemleri gerçekleştirebilmek için gerekli yetkilere sahip olunmalıdır.";
            public const string GetAllDesc = "Belirtilen filtrelere göre sisteme kayıtlı kullanıcıların " +
                "detaylarını alır.<br /> Bu işlemi yalnızca <b>Admin</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PostDesc = "Sisteme yeni bir kullanıcı ekler.<br /> Bu işlemi yalnızca " +
                "<b>Admin</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PutDesc = "Belirtilen benzersiz kimlik numarasına sahip " +
                "kullanıcıyı günceller. <br /> Bu işlemi yalnızca <b>Admin</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string ResetPasswordDesc = "Şifresini unutan kullanıcının şifresini günceller. " +
                "<br /> Bu işlemi yalnızca <b>Admin</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string DeleteDesc = "Belirtilen benzersiz kimlik numarasına " +
                "sahip kullanıcıyı sistemden siler." +
                "<br /> Bu işlemi yalnızca <b>Admin</b> türündeki kullanıcılar gerçekleştirebilir.";
        }

        public static class StationLog
        {
            public const string ControllerDesc = "Kiosklar tarafından okunan damacana QR kodu, üretim tarihi kontrolü " +
                "yapıldıktan sonra damacananın son durumu <a href = '#post-/stationlogs/-trackingId-'>ekleme </a>" +
                "uç noktası vasıtasıyla sisteme kaydedilir ve damacananın durumu güncellenir. İlgili uç noktalar " +
                "yapılan sorguları ve istatistiki bilgileri alır. " +
                "Bu işlemleri gerçekleştirebilmek için gerekli yetkilere sahip olunmalıdır.";
            public const string GetAllDesc = "Belirtilen filtrelere göre sisteme kayıtlı sorguların " +
                "detaylarını alır.<br /> Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string GetStatisticsDesc = "Sisteme kayıtlı sorguların ilgili istatistiki " +
                "bilgilerini alır.<br /> Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PostDesc = "Kiosklar tarafından yapılan kontroller sonucu oluşan kayıtları sisteme ekler. <br /> " +
                "Bu işlemi yalnızca <b>Kiosk</b> türündeki kullanıcılar gerçekleştirebilir.";
        }

        public static class Station
        {
            public const string ControllerDesc = "Damacanaların durumu, dolum aşaması öncesi ve sonrası Kiosklar yardımı" +
                " ile QR kodların okutulmasıyla takip edilir. Bu işlemleri yapan cihazların yönetimine ilgili" +
                " uç noktalar olanak sağlar. Bu işlemleri gerçekleştirebilmek için gerekli yetkilere sahip olunmalıdır.";
            public const string GetAllDesc = "Belirtilen filtrelere göre sisteme kayıtlı Kioskların " +
                "detaylarını alır.<br /> Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string GetStatisticsDesc = "Sisteme kayıtlı Kiosklar ile ilgili istatistiki " +
                "bilgileri alır.<br /> Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PostDesc = "Sisteme yeni bir kiosk ekler.<br /> " +
                "Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string PutDesc = "Belirtilen benzersiz kimlik numarasına sahip " +
                "kiosku günceller. <br /> Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
            public const string DeleteDesc = "Belirtilen benzersiz kimlik " +
                "numarasına sahip kiosku sistemden siler." +
                "<br /> Bu işlemi yalnızca <b>Admin, Panel</b> türündeki kullanıcılar gerçekleştirebilir.";
        }

        public static class Token
        {
            public const string ControllerDesc = "Sistemdeki giriş ve kullanıcı bilgilerinin doğrulanması için JWT " +
                "altyapısı kullanılmaktadır. Herhangi bir işlem yapmak isteyen kullanıcıların bilgileri kontrol edilir, " +
                "bu doğrultuda ilgili işlemi gerçekleştirebilmesine olanak sağlanır.";
            public const string AuthenticateDesc = "Kullanıcının işlem gerçekleştirebilmesi için bilgilerinin doğrulanması " +
                "gerekmektedir. Yapılan istek sonucu elde edilen 'AccessToken' kullanılır. <a href='#auth'>Kullanım...</a>";
        }
    }
}
