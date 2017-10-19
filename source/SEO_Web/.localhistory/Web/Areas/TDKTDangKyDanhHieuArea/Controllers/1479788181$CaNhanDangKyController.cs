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
using Novacode;
using System.Text.RegularExpressions;
using System.Web.Configuration;
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
        private TaiLieuDinhKemBusiness TaiLieuDinhKemBusiness;

        private string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
        private string FileAllowUpload = WebConfigurationManager.AppSettings["TDKT_FileAllowUpload"];
        private string MaxFileSizeUpload = WebConfigurationManager.AppSettings["TDKT_MaxSizeUpload"];

        public ActionResult TongHop()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.LstDonDangKyCaNhan = TDKTCaNhanDangKyBusiness.All.Where(x => x.TRANGTHAI >= DonDangKyThiDuaCaNhanConstant.TT_DAGUI && x.DON_VI_ID == UserInfo.DonViID).ToList();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA
                }
                ).ToList();
            model.LstDanhHieuCaNhan = LstDanhHieuCaNhan;
            return View(model);
        }
        public ActionResult Index()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.LstDonDangKyCaNhan = TDKTCaNhanDangKyBusiness.All.Where(x => x.USER_ID == (long)UserInfo.UserID).ToList();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA
                }
                ).ToList();
            model.LstDanhHieuCaNhan = LstDanhHieuCaNhan;
            return View(model);
        }
        public ActionResult Create()
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            //List<SelectListItem> LstPhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.All.ToList().Select(
            //    x => new SelectListItem
            //    {
            //        Value = x.ID.ToString(),
            //        Text = x.KE_HOACH_THI_DUA
            //    }
            //    ).ToList();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.Where(x => x.YEAR == DateTime.Now.Year).ToList().Select(
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
           //model.PHONG_TRAO_ID = coll["PHONG_TRAO_ID"].ToIntOrZero();
            model.USER_ID = (long)UserInfo.UserID;
            model.NGAYDANGKY = DateTime.Now;
            model.HO_TEN = UserInfo.Fullname;
            model.TRANGTHAI = DonDangKyThiDuaCaNhanConstant.TT_MOITAO;
            model.DON_VI_ID = UserInfo.DonViID;
            model.COSO_ID = UserInfo.CoSoID;
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
        public ActionResult ViewDetail(int ID)
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
        public JsonResult TrinhLanhDao(string IDS)
        {
            List<int> LstIDs = IDS.ToListInt(',');
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            List<TDKT_CANHANDANGKY> dondangkys = TDKTCaNhanDangKyBusiness.All.Where(x => LstIDs.Contains(x.ID)).ToList();
            foreach (var dondangky in dondangkys)
            {
                if (dondangky != null)
                {
                    if (dondangky.TRANGTHAI != DonDangKyThiDuaCaNhanConstant.TT_DATONGHOPGUI)
                    {
                        dondangky.TRANGTHAI = DonDangKyThiDuaCaNhanConstant.TT_DATONGHOPGUI;
                        TDKTCaNhanDangKyBusiness.Save(dondangky);
                    }
                }
            }
            
            return Json(new { Type = "SUCCESS", Message = "ok" });
        }

        public ActionResult UpdateHoSoCaNhan(int ID)
        {
            TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            DonDangKyCaNhan model = new DonDangKyCaNhan();

            TDKT_CANHANDANGKY dondangky = TDKTCaNhanDangKyBusiness.Find(ID);
            model.DonDangKy = dondangky;
            List<TAILIEUDINHKEM> LstBaoCaoThanhTich = TaiLieuDinhKemBusiness.GetDataByItemID(ID, LOAITAILIEU.TDKT_BAOCAOTHANHTICH);
            if (LstBaoCaoThanhTich.Count > 0)
            {
                model.HAS_UPDATE_BAOCAO = true;
                model.BaoCaoThanhTich = LstBaoCaoThanhTich.OrderByDescending(x => x.TAILIEU_ID).FirstOrDefault();
            }
            else
            {
                model.HAS_UPDATE_BAOCAO = false;
            }
            model.ListTaiLieuDinhKem = LstBaoCaoThanhTich;
            return View(model);
        }
        /// <summary>
        /// Id = user id
        /// Chức năng render ra báo cáo thành tích cá nhân
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public FileContentResult RenderBaoCaoCaNhan(int ID)
        {
            string template_path = Server.MapPath("~/TemplateFile/ThiDuaKhenThuong/Mau_13_Bao_cao_thanh_tich_ca_nhan.doc");
            var rbytes = System.IO.File.ReadAllBytes(template_path);
            return File(rbytes, "application/word", "Báo cáo thành tích cá nhân.doc");
        }
        public ActionResult CapNhatHoSo(IEnumerable<HttpPostedFileBase> filebase, string[] filename, FormCollection dataPost)
        {
            var ID = dataPost["DONDANGKYCANHAN_ID"].ToIntOrZero();
            UploadFileTool upload = new UploadFileTool();
            var result = upload.UploadCustomFile(filebase, true, FileAllowUpload, URLPath, MaxFileSizeUpload.ToIntOrZero(), null, filename, (long)ID, LOAITAILIEU.TDKT_BAOCAOTHANHTICH, "Báo cáo thành tích");
            return Redirect("/TDKTDangKyDanhHieuArea/CaNhanDangKy/UpdateHoSoCaNhan/" + ID);
        }
        public bool ResetConfig(int ID)
        {
            TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            List<TAILIEUDINHKEM> lstTaiLieu = TaiLieuDinhKemBusiness.All.Where(x => x.ITEM_ID == ID && x.LOAI_TAILIEU == LOAITAILIEU.TDKT_BAOCAOTHANHTICH).ToList();
            foreach (var item in lstTaiLieu)
            {
                TaiLieuDinhKemBusiness.Delete(item.TAILIEU_ID);
                TaiLieuDinhKemBusiness.Save();
            }
            return true;
        }
        public bool Confirm(int ID)
        {
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            TDKT_CANHANDANGKY dondangky = TDKTCaNhanDangKyBusiness.Find(ID);
            if (dondangky.TRANGTHAI == DonDangKyThiDuaCaNhanConstant.TT_YEUCAUHOANTHIENHOSO)
            {
                dondangky.TRANGTHAI = DonDangKyThiDuaCaNhanConstant.TT_CAPNHATVATRINH;
                TDKTCaNhanDangKyBusiness.Save(dondangky);
            }
            return true;
        }
    }
}
