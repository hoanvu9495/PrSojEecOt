using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Areas.TDKTDangKyDanhHieuArea.Models;
using Web.Custom;
using Business.CommonBusiness;
using Web.FwCore;
using Web.Common;

namespace Web.Areas.TDKTDangKyDanhHieuArea.Controllers
{
    public class CaNhanDangKyController : BaseController
    {
        //
        // GET: /TDKTDangKyDanhHieuArea/CaNhanDangKy/
        private TdktDanhhieucanhanBusiness TdktDanhhieucanhanBusiness;
        private DmDonViBusiness DmDonViBusiness;
        private TDKTCaNhanDangKyBusiness TDKTCaNhanDangKyBusiness;
        private TDKTPhongTraoThiDuaBusiness TDKTPhongTraoThiDuaBusiness;

        public ActionResult TongHop()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.LstDonDangKyCaNhan = TDKTCaNhanDangKyBusiness.All.Where(x => x.TRANGTHAI == DonDangKyThiDuaCaNhanConstant.TT_DAGUI).ToList();
            List<SelectListItem> LstPhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.KE_HOACH_THI_DUA
                }
                ).ToList();
            model.LstPhongTraoThiDua = LstPhongTraoThiDua;
            return View(model);
        }
        public ActionResult Index()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.LstDonDangKyCaNhan = TDKTCaNhanDangKyBusiness.All.Where(x => x.USER_ID == (long)UserInfo.UserID).ToList();
            List<SelectListItem> LstPhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.KE_HOACH_THI_DUA
                }
                ).ToList();
            model.LstPhongTraoThiDua = LstPhongTraoThiDua;
            return View(model);
        }
        public ActionResult Create()
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            List<SelectListItem> LstPhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.KE_HOACH_THI_DUA
                }
                ).ToList();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA
                }
                ).ToList();
            model.LstDanhHieuCaNhan = LstDanhHieuCaNhan;
            model.LstPhongTraoThiDua = LstPhongTraoThiDua;
            return View(model);
        }
        public ActionResult Edit(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            TDKT_CANHANDANGKY dondangky = TDKTCaNhanDangKyBusiness.Find(ID);
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();

            model.DonDangKyCaNhan = dondangky;
            List<SelectListItem> LstPhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.KE_HOACH_THI_DUA,
                    Selected = (dondangky.PHONG_TRAO_ID == x.ID)
                }
                ).ToList();
            List<int> LstDanhHieuDangKyIds = dondangky.DANHHIEU_IDS.ToListInt(',');
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA,
                    Selected = LstDanhHieuDangKyIds.Contains(x.ID)
                }
                ).ToList();
            model.LstDanhHieuCaNhan = LstDanhHieuCaNhan;
            model.LstPhongTraoThiDua = LstPhongTraoThiDua;
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult Save(FormCollection coll)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            DmDonViBusiness = Get<DmDonViBusiness>();
            TDKT_CANHANDANGKY model = new TDKT_CANHANDANGKY();
            model.DANHHIEU_IDS = coll["DANHHIEU_IDS"];
            model.TUDANHGIA = coll["TUDANHGIA"];
            model.PHONG_TRAO_ID = coll["PHONG_TRAO_ID"].ToIntOrZero();
            model.USER_ID = (long)UserInfo.UserID;
            model.NGAYDANGKY = DateTime.Now;
            model.HO_TEN = UserInfo.Fullname;
            model.TRANGTHAI = DonDangKyThiDuaCaNhanConstant.TT_MOITAO;
            DM_DONVI donvi = DmDonViBusiness.Find(UserInfo.DonViID);
            if (donvi != null)
            {
                model.DON_VI = donvi.TEN_DONVI;
            }
            else
            {
                model.DON_VI = "";
            }            
            TDKTCaNhanDangKyBusiness.Save(model);
            return RedirectToAction("View", new { ID = model.ID });
        }
        public ActionResult View(int ID)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            TDKT_CANHANDANGKY dangky = TDKTCaNhanDangKyBusiness.Find(ID);
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.DonDangKyCaNhan = dangky;
            List<int> DanhHieuIds = dangky.DANHHIEU_IDS.ToListInt(',');
            model.LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.Where(x => DanhHieuIds.Contains(x.ID)).ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA
                }
                ).ToList();
            return View(model);
        }
        public JsonResult Process(int ID, int TRANGTHAI)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            TDKT_CANHANDANGKY dondangky = TDKTCaNhanDangKyBusiness.Find(ID);
            if (dondangky != null)
            {
                if (dondangky.TRANGTHAI != TRANGTHAI)
                {
                    dondangky.TRANGTHAI = TRANGTHAI;
                    TDKTCaNhanDangKyBusiness.Save(dondangky);
                }
            }
            return Json(new { Type = "SUCCESS", Message = "ok" });
        }
    }
}
