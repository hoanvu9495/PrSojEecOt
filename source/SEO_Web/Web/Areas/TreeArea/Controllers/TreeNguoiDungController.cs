using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.FwCore;
using Web.Custom;
using Web.Common;
using Business.Business;

namespace Web.Areas.TreeArea.Controllers
{
    public class TreeNguoiDungController : BaseController
    {
        //
        // GET: /TreeArea/TreeNguoiDung/
        #region KhaiBao
        DmNguoidungBusiness DmNguoidungBusiness;
        int TYPETREE_MULTI=1;
        int  TYPETREE_SINGLE=2;
        #endregion

        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult GetTreeNguoiDung(int type)
        {
            ViewBag.TypeTree = type;
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            var lstNguoiDung = DmNguoidungBusiness.GetDataTree("a");
            return PartialView("TreeNguoiDungPartial", lstNguoiDung);
        }

    }
}
