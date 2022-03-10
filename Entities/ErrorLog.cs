namespace Entities
{
    public class ErrorLog : BaseEntity
    {
        public int Type { get; set; }
        public string Message { get; set; }
    }
}
