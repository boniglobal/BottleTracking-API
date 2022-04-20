namespace Core.Constants
{
    public class BottleConstants
    {
        public enum UsageStatus
        {
            Valid = 1,
            Expired,
            Trash
        }

        public enum BottleTypes
        {
            L12 = 1
        }

        //Total days
        public const int BottleShelfLife = 1826;
        public const int BottleUsageTime = 5;
        public const string ProductionDateFormat = "MM/yyyy";
    }
}
