using Core.Constants;

namespace Core.Models
{
    public class RequestFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Order Order { get; set; }
        public Dictionary<string, FieldFilter> Field { get; set; }

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

    public class FieldFilter
    {
        public string Rule { get; set; }
        public List<string> Values { get; set; }
    }

    public static class NumberComparisons
    {
        public const string Equal = "EQ";
        public const string GreaterThan = "GT";
        public const string GreaterThanOrEqual = "GTE";
        public const string LessThan = "LT";
        public const string LessThanOrEqual = "LTE";
        public const string Between = "BTW";
    }

    public static class TextComparisons
    {
        public const string Match = "EQ";
        public const string Contains = "CONT";
    }

    public static class DateComparisons
    {
        public const string Equal = "EQ";
        public const string Min = "MIN";
        public const string Max = "MAX";
        public const string Between = "BTW";
    }
}