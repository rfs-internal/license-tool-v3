using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICSLicenseMaintV2
{
    public interface IPermissionAuthorized
    {
        bool IsAuthorized(string userId);
    }

    public class PermissionAuthorized : IPermissionAuthorized
    {
        public bool IsAuthorized(string userId)
        {
            using (var context = new ICSLicenses())
            {
                return context.Permissions.Where(p => p.Userid == userId).Any();
            }
        }
    }

    public class PermissionsAuthorize : AuthorizeAttribute
    {
        private readonly IPermissionAuthorized _PermissionAuthorized;
        public PermissionsAuthorize()
        {
            _PermissionAuthorized = new PermissionAuthorized();
        }
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            return _PermissionAuthorized.IsAuthorized(httpContext.User.Identity.Name);
        }
    }
}