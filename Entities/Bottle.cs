namespace Entities
{
    public class Bottle : BaseEntity
    {
        public DateTimeOffset ProductionDate { get; set; }
        public int RefillCount { get; set; }
        public int BottleType { get; set; }
        public string TrackingId { get; set; }
        public string QrCode { get; set; }
        public int Status { get; set; }
        public DateTimeOffset LastRefillDate { get; set; }
        public DateTimeOffset LastUpdateDate { get; set; }
        public bool Deleted { get; set; }
        public List<StationLog> StationLogs { get; set; }
    }
}