using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectMaster.Core.Extensions
{

    public static class ExtensionMethods
    {
        public static IQueryable<T> OrderBy<T>(
               this IQueryable<T> source, string propertyName, bool asc)
        {
            var type = typeof(T);
            string methodName = asc ? "OrderBy" : "OrderByDescending";
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                              new Type[] { type, property.PropertyType },
                              source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> Like<T>(this IQueryable<T> source,
                      string propertyName, string keyword)
        {
            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var constant = Expression.Constant("%" + keyword + "%");

            //var like = typeof(SqlMethods).GetMethod("Like",
            //           new Type[] { typeof(string), typeof(string) });

            var like = typeof(IQueryable).GetMethod("Like",
                       new Type[] { typeof(string), typeof(string) });

            MethodCallExpression methodExp =
                  Expression.Call(null, like, propertyAccess, constant);

            Expression<Func<T, bool>> lambda =
                  Expression.Lambda<Func<T, bool>>(methodExp, parameter);
            return source.Where(lambda);
        }

        public static IEnumerable<R> SafeCast<T, R>(this IEnumerable<T> x) where T : R
        {
            return x.Cast<R>();
        }

        public static T ToObjects<T>(object obj)
        {
            var instance = (T)Activator.CreateInstance(typeof(T));

            PropertyInfo[] fields = obj.GetType().GetProperties();

            foreach (var f in fields)
            {
                if (instance.GetType().GetProperty(f.Name) != null)
                    try
                    {
                        instance.GetType().GetProperty(f.Name).SetValue(instance, obj.GetType().GetProperty(f.Name).GetValue(obj, null), null);
                    }
                    catch { }
            }
            return instance;
        }

    }
}
