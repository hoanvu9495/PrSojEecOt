using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Custom;
using Web.SSO;

namespace Web.FwCore.Factory
{
    public class VtAuthAttribute : AuthorizeAttribute
    {
        private bool baseValidate;
        private bool customValidate;
        public const string TOKEN_KEY = "AuthenticationToken";

        public VtAuthAttribute():base()
        {
            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            baseValidate = base.AuthorizeCore(httpContext);

            if (!baseValidate && ConfigurationManager.AppSettings["SSO"] == "true")
            {
                SSOLoginService sso = new SSOLoginService();
                sso.ValidateSSO();
            }

            if (baseValidate)
            {
                customValidate = Authorize.ValidateRequest(httpContext);
            }
            return baseValidate && customValidate;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (baseValidate && !customValidate)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}