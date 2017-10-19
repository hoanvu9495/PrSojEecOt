using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;

namespace Web.Areas.TDKTPhongTraoThiDuaArea.Controllers
{
    public class PhongTraoThiDuaController : Controller
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

        }
    }
}
