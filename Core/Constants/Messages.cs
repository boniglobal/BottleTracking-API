namespace Core.Constants
{
    public static class Messages
    {
        public const string Unauthorized = "Yetkisiz";
        public const string InvalidUserInfo = "Geçersiz kullanıcı bilgisi.";
        public const string InvalidToken = "Geçersiz token";

        // Station Management error messages
        public const string AssignDifferentUserType = "İstasyona sadece Kiosk türündeki kullanıcılar atanabilir.";
        public const string UserNotFound = "Kullanıcı bulunamadı.";
        public const string AssignUserMoreThanOneStation = "Kiosk türündeki bir kullanıcı birden fazla istasyona atanamaz.";

        //PanelUser Management
        public const string NonUniqueEmail = "E-posta adresi benzersiz olmalıdır.";
        public const string PasswordWithoutLetter = "Şifre en az bir harf içermelidir.";
        public const string PasswordWithoutNumber = "Şifre en az bir rakam içermelidir.";

        //Bottle Management
        public const string InValidDate = "Tarih 'AA/YYYY' formatında olmalı ve bugünden ileri olmamalı.";

        //StationLog Management
        public const string UserNotAssignedToKiosk = "Kullanıcının bir istasyona atandığından emin olunuz.";
        public const string BottleNotFound = "Sisteme kayıtlı damacana bulunamadı.";
    }
}
