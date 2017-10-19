using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Diagnostics;
using Business.Business;
using Model.eAita;
using System.Web;
using Business.CommonBusiness;
using Web.FwCore;
using Web.Common;
using Ninject.Activation;
using System.Web.Configuration;
using System.Net;

namespace Web.Filter
{
    public class ActionAuditAttribute : FilterAttribute, IActionFilter
    {
        ACTION_AUDIT actionAudit;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            actionAudit = new ACTION_AUDIT();

            actionAudit.CONTROLLER = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            actionAudit.ACTION = filterContext.ActionDescriptor.ActionName;
            actionAudit.USER_AGENT = filterContext.HttpContext.Request.UserAgent.Substring(0, 60);

            actionAudit.BEGIN_AUDIT_TIME = DateTime.Now;
            actionAudit.IP = filterContext.HttpContext.Request.UserHostAddress;
            actionAudit.DESCRIPTION = Dns.GetHostName();

            string clientIP = filterContext.HttpContext.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(clientIP))
            {
                actionAudit.IP = clientIP;
            }
            UserInfoBO loggedUser = filterContext.HttpContext.Session[SessionManager.USER_INFO] as UserInfoBO;
            if (loggedUser != null)
            {
                actionAudit.USER_NAME = loggedUser.Username;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            UserInfoBO loggedUser = filterContext.HttpContext.Session[SessionManager.USER_INFO] as UserInfoBO;

            if (actionAudit == null) { return; }

            if (loggedUser != null && string.IsNullOrWhiteSpace(actionAudit.USER_NAME) && !string.IsNullOrWhiteSpace(loggedUser.Username))
            {
                actionAudit.USER_NAME = loggedUser.Username;
            }


            if (actionAudit.USER_NAME == null)
            {
                if (actionAudit.ACTION != "ExecuteChangePass")
                {
                    return;
                }
                else
                {
                    actionAudit.USER_NAME = "Error";
                }
            }

            actionAudit.END_AUDIT_TIME = DateTime.Now;
            LogAdapter.Insert(actionAudit);
        }
    }
}