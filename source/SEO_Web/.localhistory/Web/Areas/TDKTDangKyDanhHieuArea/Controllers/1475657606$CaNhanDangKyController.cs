using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Areas.TDKTDangKyDanhHieuArea.Models;
using Web.Custom;

namespace Web.Areas.TDKTDangKyDanhHieuArea.Controllers
{
    public class CaNhanDangKyController : BaseController
    {
        //
        // GET: /TDKTDangKyDanhHieuArea/CaNhanDangKy/
        private TdktDanhhieucanhanBusiness TdktDanhhieucanhanBusiness;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.Tolist().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA
                }
                ).ToList();
            model.LstDanhHieuCaNhan = LstDanhHieuCaNhan;
            return View(model);
        }

    }
}
