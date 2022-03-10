namespace Entities
{
    public class StationLog : BaseEntity
    {
        public int StationId { get; set; }
        public int BottleId { get; set; }
        public int Status { get; set; }
        public Station Station { get; set; }
        public Bottle Bottle { get; set; }
    }
}
