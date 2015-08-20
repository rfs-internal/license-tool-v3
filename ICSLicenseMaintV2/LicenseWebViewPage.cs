using ICSLicenseMaintV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICSLicenseMaintV2
{
    public class LicenseWebViewPage<TModel> : WebViewPage<TModel>
    {
        private readonly IPermissionAuthorized _permissionAuthorized;
        public LicenseWebViewPage()
        {
            _permissionAuthorized = new PermissionAuthorized();
        }

        public bool IsAuthorized
        {
            get
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                {
                    return _permissionAuthorized.IsAuthorized(HttpContext.Current.User.Identity.Name);
                }
                return false;
            }
        }

        public IEnumerable<AlertModel> Alerts
        {
            get
            {
                var alerts = (HttpContext.Current.Session["ALERTS"] ?? new List<AlertModel>()) as List<AlertModel>;
                HttpContext.Current.Session["ALERTS"] = null;
                return alerts;
            }
        }

        public override void Execute()
        {
        }
    }
}