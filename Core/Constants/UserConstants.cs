namespace Core.Constants
{
    public class UserConstants
    {
        public enum Types
        {
            Admin = 1,
            Panel,
            Printer,
            Kiosk
        }

        public const int MaxNameLength = 30;
        public const int MaxSurnameLength = 30;
        public const int MinPasswordLength = 4;
    }
}
