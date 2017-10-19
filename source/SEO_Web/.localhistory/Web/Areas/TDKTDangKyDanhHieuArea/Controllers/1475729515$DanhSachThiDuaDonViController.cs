using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Areas.TDKTDangKyDanhHieuArea.Models;
using Web.Areas.TDKTPhongTraoThiDuaArea.Models;
using Web.Custom;
using Business.CommonBusiness;
using Web.FwCore;
using Web.Common;
using Novacode;
using System.Text.RegularExpressions;

namespace Web.Areas.TDKTDangKyDanhHieuArea.Controllers
{
    public class DanhSachThiDuaDonViController : BaseController
    {
        //
        // GET: /TDKTDangKyDanhHieuArea/DanhSachThiDuaDonVi/
        private TDKTPhongTraoThiDuaBusiness TDKTPhongTraoThiDuaBusiness;
        public ActionResult Index()
        {
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            PhongTraoThiDuaViewModel model = new PhongTraoThiDuaViewModel();
            model.LstPhongTrao = TDKTPhongTraoThiDuaBusiness.All.ToList();
            return View(model);
        }
        public ActionResult ViewReport(int ID)
        {

        }

    }
}
