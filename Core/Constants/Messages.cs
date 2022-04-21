namespace Core.Constants
{
    public static class Messages
    {
        public const string Unauthorized = "Yetkisiz";

        // Station Management error messages
        public const string AssignDifferentUserType = "İstasyona sadece Kiosk türündeki kullanıcılar atanabilir.";
        public const string UserNotFound = "Kullanıcı bulunamadı.";
        public const string AssignUserMoreThanOneStation = "Kiosk türündeki bir kullanıcı birden fazla istasyona atanamaz.";

        //PanelUser Management
        public const string NonUniqueEmail = "E-posta adresi benzersiz olmalıdır.";
        public const string PasswordWithoutLetter = "Şifre en az bir harf içermelidir.";
        public const string PasswordWithoutNumber = "Şifre en az bir rakam içermelidir.";

        //Bottle Management
        public const string InValidDateFormat = "Hatalı tarih formatı. Tarih 'AA/YYYY' formatında olmalı.";
    }
}
