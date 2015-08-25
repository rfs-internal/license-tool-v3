using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2
{
    public class DbContextFactory
    {
        private static ILog Log = LogManager.GetLogger(typeof(ICSLicenses));
        public static ICSLicenses CreateInstance()
        {
            var context = new ICSLicenses();
            if (Log.IsDebugEnabled)
            {
                context.Database.Log = msg => Log.Debug((msg ?? string.Empty).Trim());
            }
            return context;
        }
    }


}