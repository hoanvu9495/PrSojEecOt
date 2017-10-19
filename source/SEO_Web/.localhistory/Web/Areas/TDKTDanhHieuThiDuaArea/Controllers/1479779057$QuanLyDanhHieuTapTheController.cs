using System;
using System.Web.Mvc;
using Business.Business;
using Model.eAita;
using Web.FwCore;
using Web.Custom;
using Web.Common;
using System.Linq;
using System.IO;
using System.Net;
using Web.Areas.TDKTDanhHieuThiDuaArea.Models;
using System.Text;
using Elasticsearch.Net;
using Business.CommonBusiness;

namespace Web.Areas.TDKTDanhHieuThiDuaArea.Controllers
{
    public class QuanLyDanhHieuTapTheController : BaseController
    {
        private TdktDanhhieutaptheBusiness TdktDanhhieutaptheBusiness;
        //
        // GET: /TDKTDanhHieuThiDuaArea/QuanLyDanhHieuTapThe/
        public ActionResult Index()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            TdktDanhhieutaptheBusiness = Get<TdktDanhhieutaptheBusiness>();
            DanhHieuTapTheViewModel model = new DanhHieuTapTheViewModel();
            model.LstDanhHieuTapThe = TdktDanhhieutaptheBusiness.All.Where(x => x.COSO_ID == user.CoSoID).ToList();
            ViewData["Search"] = "0";
            SessionManager.SetValue("DanhHieus", model.LstDanhHieuTapThe);
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult CreateDanhHieu(FormCollection DataPost)
        {
            TdktDanhhieutaptheBusiness = Get<TdktDanhhieutaptheBusiness>();
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();

            var data = DataPost;
            int COUNT = DataPost["COUNTMAXCONDITION"].ToIntOrZero();
            string DANHHIEUTHIDUA = DataPost["DANHHIEUTHIDUA"];
            string MOTA = DataPost["MOTA"];

            TDKT_DANHHIEUTAPTHE danhhieu = new TDKT_DANHHIEUTAPTHE();
            danhhieu.DANHHIEUTHIDUA = DANHHIEUTHIDUA;
            danhhieu.MO_TA = MOTA;
            danhhieu.YEAR = DataPost["YEAR"].ToIntOrZero();
            danhhieu.COSO_ID = user.CoSoID;
            
            TdktDanhhieutaptheBusiness.Save(danhhieu);

            return RedirectToAction("Index");
        }
        public ActionResult ViewDetail(int ID)
        {
            TdktDanhhieutaptheBusiness = Get<TdktDanhhieutaptheBusiness>();
            TDKT_DANHHIEUTAPTHE danhhieu = TdktDanhhieutaptheBusiness.All.Where(x => x.ID == ID).FirstOrDefault();
            return View(danhhieu);
        }
    }
}
