using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortBy, bool isDescending)
        {
            // Create a parameter for the expression (a variable to be used in the lambda expression)
            var parameter = Expression.Parameter(typeof(T), "x");

            // Get a property of object T by its name (sortBy)
            var property = Expression.Property(parameter, sortBy);

            // Create a lambda expression to access the object property
            var lambda = Expression.Lambda(property, parameter);

            // Define the sorting method, "OrderBy" or "OrderByDescending"
            var methodName = isDescending ? "OrderByDescending" : "OrderBy";

            // Call the corresponding sorting method via reflection
            var expression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type }, // Specify the types of method arguments
                query.Expression, // Initial query expression
                Expression.Quote(lambda) // Lambda expression wrapped in Quote
                );

            // Create and return a new query that includes sorting
            return query.Provider.CreateQuery<T>(expression);
        }
    }
}
