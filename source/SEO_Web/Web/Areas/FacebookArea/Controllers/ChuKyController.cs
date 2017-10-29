using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.FwCore;
using Web.Common;
using Web.Custom;
using Business.Business;
using Business.CommonBusiness;
using Model.DBTool;

namespace Web.Areas.FacebookArea.Controllers
{
    public class ChuKyController : BaseController
    {
        //
        // GET: /FacebookArea/ChuKy/
        #region KhaiBao

        private FbChuKyBusiness FbChuKyBusiness;
        #endregion
        public ActionResult Index()
        {
            FbChuKyBusiness = Get<FbChuKyBusiness>();
            AssignUserInfo();
            var lstChuKy = FbChuKyBusiness.GetListByUser(currentUserId);
            return View(lstChuKy);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveChuKy(string chuky, bool isActive)
        {
            var model = new JsonResultBO(true);
            AssignUserInfo();
            FbChuKyBusiness = Get<FbChuKyBusiness>();
            var ck = new FB_CHUKY();
            ck.USER_ID = currentUserId;
            ck.IS_MAIN = isActive;
            ck.CHUKY = chuky;
            FbChuKyBusiness.Save(ck);
            if (ck.IS_MAIN.GetValueOrDefault(false))
            {
                FbChuKyBusiness.HuyChuKyChinh(ck.ID, currentUserId);
            }
            return Json(model);
        }

        public PartialViewResult ChonChuKy()
        {
            AssignUserInfo();
            FbChuKyBusiness = Get<FbChuKyBusiness>();
            var listChuKy = FbChuKyBusiness.GetListByUser(currentUserId);
            return PartialView("_ChonChuKyPartial", listChuKy);
        }

    }
}
