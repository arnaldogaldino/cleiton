using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace ProjectMaster.Core
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
                        if (instance.GetType().GetProperty(f.Name).PropertyType.Equals(obj.GetType().GetProperty(f.Name).PropertyType))
                            instance.GetType().GetProperty(f.Name).SetValue(instance, obj.GetType().GetProperty(f.Name).GetValue(obj, null), null);
                        else
                        {
                            var value = Convert.ChangeType(obj.GetType().GetProperty(f.Name).GetValue(obj, null).ToString(), instance.GetType().GetProperty(f.Name).PropertyType);
                            instance.GetType().GetProperty(f.Name).SetValue(instance, value, null);
                        }
                    }
                    catch { }
            }
            return instance;
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> list, Func<T, string> text, Func<T, object> value, string opcaoSelecionada = "")
        {
            var itens = list.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f).ToString()
            }).ToList();

            return string.IsNullOrEmpty(opcaoSelecionada) ? new SelectList(itens, "Value", "Text") : new SelectList(itens, "Value", "Text", opcaoSelecionada);
        }

        public static void AdicionarOpcaoPadrao(ref List<SelectListItem> itens, string textoPadrao, string valorPadrao)
        {
            if (!string.IsNullOrEmpty(textoPadrao))
            {
                itens.Insert(0, new SelectListItem()
                {
                    Text = textoPadrao,
                    Value = valorPadrao
                });
            }
        }


    }

}
