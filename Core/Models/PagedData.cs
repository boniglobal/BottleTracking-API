namespace Core.Models
{
    public class PagedData<T>
    {
        public List<T> Items { get; set; }
        public bool Previous { get; set; }
        public bool Next { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
    }
}
