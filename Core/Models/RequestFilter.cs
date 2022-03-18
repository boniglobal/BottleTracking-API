using Core.Constants;

namespace Core.Models
{
    public class RequestFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Order Order { get; set; }
        public RequestFilter()
        {
            PageNumber = FilterConstants.MinIndex;
            PageSize = FilterConstants.MinPageSize;
            Order = new Order
            {
                Field = null,
                IsDesc = false
            };
        }
        public RequestFilter(int number, int size, string field, bool isDesc = false)
        {
            PageNumber = number > FilterConstants.MinIndex ? number : FilterConstants.MinIndex;
            PageSize = size > FilterConstants.MaxPageSize ? FilterConstants.MaxPageSize : size;
            Order = new Order
            {
                Field = field,
                IsDesc = isDesc
            };
        }
    }
    public class Order
    {
        public string Field { get; set; }
        public bool IsDesc { get; set; }
    }
    public class Filter
    {
        public string Field { get; set; }
        public object Value { get; set; }
    }
}
