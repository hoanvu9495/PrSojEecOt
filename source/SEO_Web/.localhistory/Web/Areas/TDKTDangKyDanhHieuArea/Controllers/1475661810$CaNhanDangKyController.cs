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
        public ActionResult Index()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.LstDonDangKyCaNhan = TDKTCaNhanDangKyBusiness.All.Where(x => x.USER_ID == (long)UserInfo.UserID).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
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
    }
}
