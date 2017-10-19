using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Business;
using Business.CommonBusiness;
using Web.FwCore;
using Model.eAita;
using Web.Models;
using Business.CommonBusiness;
using Web.Custom;

namespace Web.Controllers
{
    public class NotificationController : BaseController
    {
        SysTinnhanBusiness SysTinnhanBusiness;

        //
        // GET: /Notification/

        [HttpPost]
        public JsonResult Notice()
        {
            var user = (UserInfoBO)SessionManager.GetUserInfo();
            SysTinnhanBusiness = Get<SysTinnhanBusiness>();
            var lstTinNhan = SysTinnhanBusiness.GetListTinNhanChuaDoc((decimal)user.UserID);
            return Json(lstTinNhan);
        }

    }
}
