using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Extensions
{
    public static class Extensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, ref RequestFilter filters)
        {
            if (filters.Field != null)
            {
                foreach (var key in filters.Field.Keys)
                {
                    var property = typeof(T).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && filters.Field[key].Values?.Count > 0)
                    {
                        var rule = filters.Field[key].Rule;
                        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                        MemberExpression member = Expression.Property(parameter, key);
                        if (property.PropertyType.IsText())
                        {
                            var values = filters.Field[key].Values;
                            Expression<Func<T, bool>> predicate = null;
                            if (rule == TextComparisons.Match)
                            {
                                foreach (var value in values)
                                {
                                    ConstantExpression constant = Expression.Constant(value, typeof(string));
                                    BinaryExpression body = Expression.Equal(member, constant);
                                    var condition = Expression.Lambda<Func<T, bool>>(body, new[] { parameter });
                                    predicate = predicate.Or(condition);
                                }
                                query = query.Where(predicate);
                            }
                            else if (rule == TextComparisons.Contains)
                            {
                                foreach (var value in values)
                                {
                                    ConstantExpression constant = Expression.Constant($"%{value}%", typeof(string));

                                    var _ILike = typeof(NpgsqlDbFunctionsExtensions).GetMethod("ILike", BindingFlags.Static | 
                                        BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(DbFunctions), typeof(string),
                                    typeof(string) }, null );

                                    var bodyLike = Expression.Call(_ILike, 
                                        Expression.Constant(null, typeof(DbFunctions)), member, constant);

                                    var condition = Expression.Lambda<Func<T, bool>>(bodyLike, parameter);
                                    predicate = predicate.And(condition);
                                }
                                query = query.Where(predicate);
                            }

                            filters.Field.Remove(key);
                        }
                        else if (property.PropertyType.IsNumber())
                        {
                            var values = filters.Field[key].Values.Select(x => ParseNumberValue(x)).OrderBy(x=>x).ToList();
                            Expression<Func<T, bool>> predicate = null;
                            ConstantExpression constant = Expression.Constant(Convert.ChangeType(values.First(), member.Type), member.Type);
                            BinaryExpression body = null;

                            switch (rule)
                            {
                                case NumberComparisons.Equal:
                                    body = Expression.Equal(member, constant);
                                    break;
                                case NumberComparisons.GreaterThan:
                                    body = Expression.GreaterThan(member, constant);
                                    break;
                                case NumberComparisons.GreaterThanOrEqual:
                                    body = Expression.GreaterThanOrEqual(member, constant);
                                    break;
                                case NumberComparisons.LessThan:
                                    body = Expression.LessThan(member, constant);
                                    break;
                                case NumberComparisons.LessThanOrEqual:
                                    body = Expression.LessThanOrEqual(member, constant);
                                    break;
                                case NumberComparisons.Between:
                                    var last = values.Last();
                                    var lastConst = Expression.Constant(Convert.ChangeType(values.Last(), member.Type), member.Type);
                                    var GT = Expression.GreaterThanOrEqual(member, constant);
                                    var LT = Expression.LessThanOrEqual(member, lastConst);
                                    body = Expression.And(GT, LT);
                                    break;

                            }
                            predicate = Expression.Lambda<Func<T, bool>>(body, new[] { parameter });
                            query = query.Where(predicate);
                            filters.Field.Remove(key);
                        }
                        else if (property.PropertyType.IsDate())
                        {
                            Expression<Func<T, bool>> predicate = null;
                            member = Expression.Property(parameter, $"{key}");
                            var values = filters.Field[key].Values.Select(x => ParseDateValue(x)).ToList();
                            var type = values.First().GetType();
                            ConstantExpression constant = Expression.Constant(values.First(), member.Type);
                            BinaryExpression body = null;
                            switch (rule)
                            {
                                case DateComparisons.Equal:
                                    var first = values.First();
                                    var last = values.Last().GetValueOrDefault();
                                    body = Expression.Equal(member, constant);
                                    break;
                                case DateComparisons.Min:
                                    body = Expression.GreaterThanOrEqual(member, constant);
                                    break;
                                case DateComparisons.Max:
                                    body = Expression.LessThanOrEqual(member, constant);
                                    break;
                                case DateComparisons.Between:
                                    var lastConst = Expression.Constant(values.Last(), member.Type);
                                    var GT = Expression.GreaterThanOrEqual(member, constant);
                                    var LT = Expression.LessThanOrEqual(member, lastConst);
                                    body = Expression.And(GT, LT);
                                    break;
                            }
                            predicate = Expression.Lambda<Func<T, bool>>(body, new[] { parameter });
                            query = query.Where(predicate);
                            filters.Field.Remove(key);
                        }
                        else if (property.PropertyType.IsBoolean())
                        {
                            //NOTE - No need for now
                            var values = filters.Field[key].Values.Select(x => ParseBooleanValue(x)).ToList();
                            //Expression<Func<T, bool>> predicate = x => values.Contains(EF.Property<bool?>(x, key));
                            Expression<Func<T, bool>> predicate = null;
                            query = query.Where(predicate);
                            filters.Field.Remove(key);
                        }
                    }
                }
            }

            return query;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string field, bool isDesc = false)
        {
            if (!string.IsNullOrEmpty(field))
            {
                var property = typeof(T).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
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

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
                return right;
            if (right == null)
                return left;

            var parameter = Expression.Parameter(typeof(T));
            var combinedExpression = new CustomVisitor(parameter).Visit(Expression.OrElse(left.Body, right.Body));
            return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
                return right;
            if (right == null)
                return left;

            var parameter = Expression.Parameter(typeof(T));
            var combinedExpression = new CustomVisitor(parameter).Visit(Expression.AndAlso(left.Body, right.Body));
            return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        }

        class CustomVisitor : ExpressionVisitor
        {
            readonly ParameterExpression _parameter;

            internal CustomVisitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameter;
            }
        }

        //Type categorization methods to apply filter rules
        public static bool IsText(this Type type)
        {
            return typeof(string) == type;
        }

        public static bool IsNumber(this Type type)
        {
            var numericTypes = new HashSet<Type>
            {
                typeof(int),
                typeof(double),
                typeof(float),
                typeof(decimal),
                typeof(long),
                typeof(short)
            };

            return numericTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
        }

        public static bool IsDate(this Type type)
        {
            var dateTypes = new HashSet<Type>
            {
                typeof(DateTime),
                typeof(DateTimeOffset)
            };

            return dateTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
        }

        public static bool IsBoolean(this Type type)
        {
            return typeof(bool) == (Nullable.GetUnderlyingType(type) ?? type);
        }

        //field value parsing methods
        public static double? ParseNumberValue(string value)
        {
            return double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out double n) ? n : null;
        }

        public static DateTimeOffset? ParseDateValue(string value)
        {
            return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, 
                                           DateTimeStyles.AssumeUniversal, out DateTimeOffset d) ? d : null;
        }

        public static bool? ParseBooleanValue(string value)
        {
            return bool.TryParse(value, out bool b) ? b : null;
        }
    }
}