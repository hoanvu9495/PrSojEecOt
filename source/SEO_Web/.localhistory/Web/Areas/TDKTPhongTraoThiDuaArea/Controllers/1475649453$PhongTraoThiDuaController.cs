using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Custom;

namespace Web.Areas.TDKTPhongTraoThiDuaArea.Controllers
{
    public class PhongTraoThiDuaController : BaseController
    {
        //
        // GET: /TDKTPhongTraoThiDuaArea/PhongTraoThiDua/
        private TDKTPhongTraoThiDuaBusiness TDKTPhongTraoThiDuaBusiness;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult Save(FormCollection coll)
        {
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            TDKT_PHONGTRAO_THIDUA model = new TDKT_PHONGTRAO_THIDUA();
            model.KE_HOACH_THI_DUA = coll["KE_HOACH_THI_DUA"];
            model.NOI_DUNG = coll["NOIDUNG"];
        }
    }
}
