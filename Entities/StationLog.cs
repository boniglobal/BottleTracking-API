namespace Entities
{
    public class StationLog : BaseEntity
    {
        public int StationId { get; set; }
        public int BottleId { get; set; }
        public int Status { get; set; }
        public string DistributorId { get; set; }
        public string DistributionRegion { get; set; }
        public Station Station { get; set; }
        public Bottle Bottle { get; set; }
    }
}
