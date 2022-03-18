namespace Core.DTOs
{
    public class Station
    {
        public class StationListView
        {
            public int Id { get; set; }
            public DateTimeOffset CreateDate { get; set; }
            public string ProductionLine { get; set; }
            public int Location { get; set; }
            public int PanelUserId { get; set; }
            public string Fullname { get; set; }
        }

        public class StationAdd
        {
            public string ProductionLine { get; set; }
            public int Location { get; set; }
            public int PanelUserId { get; set; }
        }

        public class StationUpdate
        {
            public int StationId { get; set; }
            public string ProductionLine { get; set; }
            public int Location { get; set; }
            public int PanelUserId { get; set; }
        }

        public class StationStatistics
        {
            public int TotalNumberOfLines { get; set; }
            public int TotalNumberOfStations { get; set; }
        }
    }
}
