using static Core.Constants.BottleConstants;

namespace Core.DTOs
{
    public class StationLog
    {
        public class StationLogGetResponse
        {
            public int Id { get; set; }
            public DateTimeOffset BottleProductionDate { get; set; }
            public string TrackingId { get; set; }
            public string ProductionLine { get; set; }
            public int Location { get; set; }
            public UsageStatus BottleStatus { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
        }

        public class StationLogStatistics
        {
            public int TotalQuery { get; set; }
            public int AverageOfDailyQuery { get; set; }
            public int NumberOfTodayQuery { get; set; }
            public int NumberOfTodayBottle { get; set; }
        }
    }
}
