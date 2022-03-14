namespace Entities
{
    public class Station : BaseEntity
    {
        public string ProductionLine { get; set; }
        public int Location { get; set; }
        public int PanelUserId { get; set; }
        public bool Deleted { get; set; }
        public List<StationLog> StationLogs { get; set; }
    }
}
