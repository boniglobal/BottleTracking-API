using static Core.Constants.BottleConstants;

namespace Core.DTOs
{
    public class Bottle
    {
        public class BottleStatusGetResponse
        {
            public string TrackingId { get; set; }
            public BottleTypes Status { get; set; }
            public DateTimeOffset ProductionDate { get; set; }
            public int RefillCount { get; set; }
            public DateTimeOffset LastRefillDate { get; set; }
        }

        public class BottleView
        {
            public int Id { get; set; }
            public DateTimeOffset ProductionDate { get; set; }
            public int RefillCount { get; set; }
            public DateTimeOffset LastRefillDate { get; set; }
            public int BottleType { get; set; }
            public string QrCode { get; set; }
            public int QrPrintCount { get; set; }
            public UsageStatus Status { get; set; }
            public DateTimeOffset CreateDate { get; set; }
            public string TrackingId { get; set; }
        }

        public class BottleAdd
        {
            public string ProductionDate { get; set; }
            public int? RefillCount { get; set; }
            public int BottleType { get; set; }
        }
        public class BottleUpdate
        {
            public int Id { get; set; }
            public string ProductionDate { get; set; }
            public int BottleType { get; set; }
        }

        public class BottleStatistics
        {
            public int TotalNumberOfBottles { get; set; }
            public int TotalNumberOfBottlesInUse { get; set; }
            public int TotalNumberOfBottlesExpiredAndInUse { get; set; }
            public int TotalNumberOfBottlesOutOfUse { get; set; }
        }
    }
}
