using ICSLicenseMaintV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ICSLicenseMaintV2
{
    public static class LinqExtensions
    {

        public static Exception GetInnerException(this Exception ex)
        {
            if(ex.InnerException != null)
            {
                return ex.InnerException.GetInnerException();
            }
            return ex;
        }

        public static bool TryAndHandleErrorWithAlert(this Controller controller, Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                controller.AddAlert(AlertModel.Error(ex.GetInnerException().Message));
                return false;
            }
        }

        public static void AddAlert(this Controller controller, AlertModel alertModel)
        {
            List<AlertModel> alerts;
            
            if (controller.Session["ALERTS"] == null)
            {
                controller.Session["ALERTS"] = alerts = new List<AlertModel>();
            }
            else
            {
                alerts = controller.Session["ALERTS"] as List<AlertModel>;
            }

            alerts.Add(alertModel);
        }

        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            try
            {
                var prop = Expression.Property(param, SortField);
                var exp = Expression.Lambda(prop, param);
                string method = Ascending ? "OrderBy" : "OrderByDescending";
                Type[] types = new Type[] { q.ElementType, exp.Body.Type };
                var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
                return q.Provider.CreateQuery<T>(mce);
            }
            catch { }
            return q;
        }
    }
}