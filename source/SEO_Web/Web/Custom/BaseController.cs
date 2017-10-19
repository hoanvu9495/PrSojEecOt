using Business.Business;
using Business.CommonBusiness;
using log4net;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Common;
using Web.FwCore;
namespace Web.Custom
{
    public class BaseController : Controller
    {
        protected UserInfoBO currentUser;
        protected int currentUserId;
        private static readonly ILog log = LogManager.GetLogger("BaseController");
        //Context per request
        protected Entities Context;
        protected Dictionary<string, object> ListBusiness = new Dictionary<string, object>();
        /// <summary>
        /// default execute action
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //CAUHINH_HETHONG sc = Get<CauhinhHethongBusiness>().All.Where(o => o.MA_CAU_HINH == "MAINTAIN_TIME_QL").FirstOrDefault();
            //if (sc != null && sc.GIA_TRI != null && !filterContext.HttpContext.Request.IsLocal)
            //{
            //    if (filterContext.ActionDescriptor.ActionName != "ServerMaintenance")
            //    {
            //        this.Response.Redirect("~/Home/ServerMaintenance");
            //    }
            //}
        }
        /// <summary>
        /// Create new context if null
        /// </summary>
        public Entities GetContext()
        {
            if (Context == null)
            {
                Context = new Entities();
            }
            return Context;
        }

        protected void AssignUserInfo()
        {
            currentUser = this.GetUserInfo();
            currentUserId = (int)currentUser.UserID;
            currentUser.CoSoID = currentUser.CoSoID.HasValue ? currentUser.CoSoID.Value : 0;
            currentUser.PhongBanID = currentUser.PhongBanID.HasValue ? currentUser.PhongBanID.Value : 0;
        }

        protected bool CheckIsAdmin(UserInfoBO user)
        {
            if(user != null)
            {
                if (user.Username.Trim().ToLower().Equals("admin"))
                {
                    return true;
                }
            }
            return false;
        }

        public UserInfoBO GetUserInfo()
        {
            return (UserInfoBO)SessionManager.GetUserInfo();
        }

        public virtual B Get<B>()
        {
            string key = typeof(B).ToString();
            if (ListBusiness.ContainsKey(key))
            {
                return (B)ListBusiness[key];
            }
            try
            {
                B res = (B)typeof(B).GetConstructor(new Type[] { typeof(Entities) }).Invoke(new object[] { this.GetContext() });
                PropertyInfo currentUser = res.GetType().GetProperty("CurrentUsername");
                if (currentUser != null && currentUser.GetValue(res, null) == null && GetUserInfo() != null)
                {
                    currentUser.SetValue(res, GetUserInfo().Username, null);
                }
                ListBusiness[key] = res;
                return res;
            }
            catch(Exception ex)
            {
                log.Error("Unable to create Context:" + ex.Message);
                return default(B);
            }
        }

        public void SetViewDataActionAudit(IDictionary<string, object> dic)
        {
            ViewData[CommonKey.AuditActionKey.OldJsonObject] = dic[UIConstant.ACTION_AUDIT_OLD_JSON];
            ViewData[CommonKey.AuditActionKey.NewJsonObject] = dic[UIConstant.ACTION_AUDIT_NEW_JSON];
            ViewData[CommonKey.AuditActionKey.ObjectID] = dic[UIConstant.ACTION_AUDIT_OBJECTID];
            ViewData[CommonKey.AuditActionKey.Description] = dic[UIConstant.ACTION_AUDIT_DESCRIPTION];
        }
        public void SetViewDataActionAudit(string oldJson, string newJson, string objectID, string description)
        {
            ViewData[CommonKey.AuditActionKey.OldJsonObject] = oldJson;
            ViewData[CommonKey.AuditActionKey.NewJsonObject] = newJson;
            ViewData[CommonKey.AuditActionKey.ObjectID] = objectID;
            ViewData[CommonKey.AuditActionKey.Description] = description;
        }
    }
}