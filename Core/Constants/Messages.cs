namespace Core.Constants
{
    public static class Messages
    {
        public const string Unauthorized = "Unauthorized";

        // Station Management error messages
        public const string AssignDifferentUserType = "A kiosk user cannot be registered to more than one station.";
        public const string UserNotFound = "User not found.";
        public const string AssignUserMoreThanOneStation = "A kiosk user cannot be registered to more than one station.";

        //PanelUser Management
        public const string NonUniqueEmail = "The email address must be unique.";
        public const string PasswordWithoutLetter = "Your password must contain at least one letter.";
        public const string PasswordWithoutNumber = "Your password must contain at least one number.";

        //Bottle Management
        public const string InValidDateFormat = "Hatalı tarih formatı. Tarih 'AA/YYYY' formatında olmalı.";
    }
}
