using Core.Models;
using System.Linq.Expressions;

namespace Core.Extensions
{
    public static class Extensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string field, bool isDesc = false)
        {
            if (!string.IsNullOrEmpty(field))
            {
                var property = typeof(T).GetProperty(field);
                if (property != null)
                {
                    var parameter = Expression.Parameter(typeof(T));
                    var member = Expression.Property(parameter, field);
                    var propertyObject = Expression.Convert(member, typeof(object));
                    var predicate = Expression.Lambda<Func<T, object>>(propertyObject, parameter);
                    return isDesc ? query.OrderByDescending(predicate) : query.OrderBy(predicate);
                }
            }

            return query;
        }

        public static PagedData<T> Paginate<T>(this IQueryable<T> query, int pageNo, int pageSize)
        {
            var offset = (pageNo - 1) * pageSize;
            var count = query.Count();
            var data = query.Skip(offset).Take(pageSize).ToList();

            var page = new PagedData<T>
            {
                Items = data,
                TotalCount = count,
                PageNumber = pageNo,
                Previous = pageNo > 1,
                Next = offset + pageSize < count
            };

            return page;
        }
    }
}
