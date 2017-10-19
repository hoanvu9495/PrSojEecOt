using Business.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Custom;
using Business.CommonBusiness;
using Model.eAita;
using Web.FwCore;
using Web.Models;
using Web.Common;
using System.Web.Configuration;
using System.Collections;
using Business.CommonHelper;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace Web.Controllers
{
    public class CommonController : BaseController
    {
        DmNguoidungBusiness NguoiDungBs;
        ChatNoidungBusiness ChatNoidungBusiness;
        ChatGroupBusiness ChatGroupBusiness;
        ChatGroupUserBusiness ChatGroupUserBusiness;
        CosoBusiness CoSoBs;
        DmDonViBusiness DonViBs;
        DmPhongbanBusiness PhongBanBs;
        HscbNguoiDungCanBoBusiness HscbNguoiDungCanBoBs;
        HscbCongChucVienChucBusiness HscbCCVCBs;
        DmChucVuBusiness DmChucVuBs;
        HscbNhanSuLogBusiness HscbNhanSuLogBusiness;
        HscbCongChucVienChucBusiness HscbCongChucVienChucBusiness;
        HscbChuyenNgachBusiness HscbChuyenNgachBusiness;
        HscbNangNgachBusiness HscbNangNgachBusiness;
        HscbBoNhiemLaiCongChucBusiness HscbBoNhiemLaiCongChucBusiness;
        HscbDanhGiaCanBoBusiness HscbDanhGiaCanBoBusiness;
        HscbThoiViecBusiness HscbThoiViecBusiness;
        HscbCCVCTapSuBusiness HscbCCVCTapSuBusiness;
        HscbFilesBusiness HscbFilesBusiness;
        DmChucnangBusiness DmChucnangBusiness;
        WfQuytrinhBusiness WfQuytrinhBusiness;
        WfBuocchuyentrangthaiBusiness WfBuocchuyentrangthaiBusiness;
        ThuMucLuuTruBusiness ThuMucLuuTruBusiness;
        private string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
        private List<string> arrFolder = new List<string>();
        private List<THUMUC_LUUTRU> lstPath = new List<THUMUC_LUUTRU>();
        [AllowAnonymous]
        //[HttpGet]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult LoadDiaDiem(int TINH_ID, int HUYEN_ID, int XA_ID, int TYPE)
        {
            //Load Huyện theo Tỉnh
            if (TYPE == 1)
            {
                HuyenBusiness HuyenBusiness = Get<HuyenBusiness>();
                // lấy danh sách tỉnh
                var huyenItems = HuyenBusiness.All.Where(h => h.TINH_ID == TINH_ID);
                if (huyenItems.Count() > 0) // kiểm tra dữ liệu
                {
                    // lấy dữ liệu ra và trả về dưới dạng Json
                    var result = huyenItems.Select(h => new
                    {
                        h.HUYEN_ID,
                        h.TENHUYEN
                    });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // không có dữ liệu, trả về lỗi
                    return Json(false);
                }
            }
            //Load Xã theo Huyện
            if (TYPE == 2)
            {
                XaBusiness XaBusiness = Get<XaBusiness>();
                // lấy danh sách xã
                var xaItems = XaBusiness.All.Where(h => h.HUYEN_ID == HUYEN_ID);
                if (xaItems.Count() > 0) // kiểm tra dữ liệu
                {
                    // lấy dữ liệu ra và trả về dưới dạng Json
                    var result = xaItems.Select(h => new
                    {
                        h.XA_ID,
                        h.TENXA
                    });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // không có dữ liệu, trả về lỗi
                    return Json(false);
                }
            }
            return Json(true);
        }
        //[AllowAnonymous]
        //[HttpPost]
        //public JsonResult LoadDonVi(int TINH_ID, int HUYEN_ID, int XA_ID)
        //{
        //    CosoBusiness cosoBusiness = Get<CosoBusiness>();
        //    var ds_coso = cosoBusiness.All
        //        .Where(c => c.TINH_ID == TINH_ID && c.HUYEN_ID == HUYEN_ID && c.XA_ID == XA_ID);
        //    if (ds_coso.Count() > 0)
        //    {
        //        var result = ds_coso.Select(c => new
        //        {
        //            c.COSO_ID,
        //            c.TENCOSO
        //        });
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }
        //}
        /// <summary>
        /// Load danh sách đơn vị trung ương hoặc bv trung ương
        /// </summary>
        /// <param name="HINHTHUC"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //public JsonResult LoadDonViTW(short? HINHTHUC)
        //{
        //    CosoBusiness cosoBusiness = Get<CosoBusiness>();
        //    var ds_coso = cosoBusiness.All.Where(x => x.HINHTHUC == HINHTHUC && x.IS_ACTIVE == 0);
        //    if (ds_coso.Count() > 0)
        //    {
        //        var result = ds_coso.Select(c => new
        //        {
        //            c.COSO_ID,
        //            c.TENCOSO
        //        });
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">TRA VE DS CHUCNANG CON HOAC CHUC NANG CHA</param>
        /// <returns></returns>
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[HttpGet]
        [HttpPost]
        public JsonResult LoadChucNang(int? parent, decimal? role)
        {
            if (parent.HasValue && role.HasValue)
            {
                DmChucnangBusiness DmChucnangBusiness = Get<DmChucnangBusiness>();
                // LOAD CAC CHUC NANG CON
                var CN_result = Get<VaitroChucnangBusiness>().All.Where(x => x.DM_CHUCNANG.CHUCNANG_CHA == parent && x.TRANGTHAI == 1 && x.DM_VAITRO_ID == role && x.DM_CHUCNANG.TRANGTHAI == 1).Select(o => new ChucNangBO
                {
                    DM_CHUCNANG_ID = o.DM_CHUCNANG_ID.Value,
                    TEN_CHUCNANG = o.DM_CHUCNANG.TEN_CHUCNANG,
                    URL = o.DM_CHUCNANG.URL,
                    TT_HIENTHI = o.DM_CHUCNANG.TT_HIENTHI,
                    CHUCNANG_CHA = o.DM_CHUCNANG.CHUCNANG_CHA,
                }).OrderBy(o => o.TT_HIENTHI).ThenBy(o => o.DM_CHUCNANG_ID).ToList();
                if (CN_result.Count > 0)
                {
                    return Json(CN_result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // LAY CHUC NANG CHA
                    var result = DmChucnangBusiness.All.Where(x => x.DM_CHUCNANG_ID == parent && x.TRANGTHAI == 1).Select(x => new
                    {
                        x.DM_CHUCNANG_ID,
                        x.CHUCNANG_CHA,
                        x.TEN_CHUCNANG,
                        x.URL
                    });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false);
        }
        public JsonResult LoadDiaDiemTinh(int TINH_ID, int HUYEN_ID, int TYPE)
        {
            //Load Huyện theo Tỉnh
            if (TYPE == 1)
            {
                HuyenBusiness HuyenBusiness = Get<HuyenBusiness>();
                // lấy danh sách tỉnh
                var huyenItems = HuyenBusiness.All.Where(h => h.TINH_ID == TINH_ID);
                if (huyenItems.Count() > 0) // kiểm tra dữ liệu
                {
                    // lấy dữ liệu ra và trả về dưới dạng Json
                    var result = huyenItems.Select(h => new
                    {
                        h.HUYEN_ID,
                        h.TENHUYEN
                    });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // không có dữ liệu, trả về lỗi
                    return Json(false);
                }
            }
            return Json(true);
        }
        [AllowAnonymous]
        public PartialViewResult ListPhongBan(int COSO_ID, int DONVI_ID, string TEXT_ID, string VALUE_ID, string IS_MULTICHOICE, string IDS, string KEYWORD, string CALLBACK_FUNCTION, string INDEX, string EXCLUDE_IDS)
        {
            CoSoBs = Get<CosoBusiness>();
            List<COSO> LstCoSo = CoSoBs.All.Where(x => ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            DonViBs = Get<DmDonViBusiness>();
            List<DM_DONVI> LstDonVi = DonViBs.All.Where(
                x => ((DONVI_ID > 0) ? x.ID == DONVI_ID : true)
                     && ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            PhongBanBs = Get<DmPhongbanBusiness>();
            List<DM_PHONGBAN> LstPhongBan = PhongBanBs.All.Where(x => ((DONVI_ID > 0) ? x.DM_DONVI_ID == DONVI_ID : true)).ToList();
            string[] wIds = IDS.Split(',');
            ChonNguoiDungModel model = new ChonNguoiDungModel();
            model.TEXT_ID = TEXT_ID;
            model.VALUE_ID = VALUE_ID;
            model.IS_MULTICHOICE = IS_MULTICHOICE.ToIntOrZero();
            model.IDS = wIds;
            model.CALLBACK_FUNCTION = CALLBACK_FUNCTION;
            model.INDEX = INDEX.ToIntOrZero();
            return PartialView("_SearchPhongBanResult", model);
        }
        [AllowAnonymous]
        public PartialViewResult ListDepartment(int COSO_ID, int DONVI_ID, string TEXT_ID, string VALUE_ID, string IS_MULTICHOICE, string IDS, string KEYWORD, string CALLBACK_FUNCTION, string INDEX, string EXCLUDE_IDS)
        {
            CoSoBs = Get<CosoBusiness>();
            List<COSO> LstCoSo = CoSoBs.All.Where(x => ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            DonViBs = Get<DmDonViBusiness>();
            List<DM_DONVI> LstDonVi = DonViBs.All.Where(
                x => ((DONVI_ID > 0) ? x.ID == DONVI_ID : true)
                     && ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            PhongBanBs = Get<DmPhongbanBusiness>();
            List<DM_PHONGBAN> LstPhongBan = PhongBanBs.All.Where(x => ((DONVI_ID > 0) ? x.DM_DONVI_ID == DONVI_ID : true)).ToList();
            List<DM_PHONGBAN> LstPhongBanSearch = LstPhongBan.Where(x => ((KEYWORD.Length > 0) ? x.TENPHONGBAN.ToLower().Contains(KEYWORD.Trim().ToLower()) : true)).ToList();
            if (LstPhongBanSearch.Count > 0)
            {
                string[] IDS_Search = new string[LstPhongBanSearch.Count()];
                int tmp_index = 0;
                foreach (var tmp_dep in LstPhongBanSearch)
                {
                    IDS_Search[tmp_index] = tmp_dep.DM_PHONGBAN_ID.ToString();
                    tmp_index = tmp_index + 1;
                }
                SessionManager.SetValue("LstPhongBanSearch", IDS_Search);
            }
            SessionManager.SetValue("LstCoSo", LstCoSo);
            SessionManager.SetValue("LstDonVi", LstDonVi);
            SessionManager.SetValue("LstPhongBan", LstPhongBan);
            string[] wIds = IDS.Split(',');
            ChonNguoiDungModel model = new ChonNguoiDungModel();
            model.TEXT_ID = TEXT_ID;
            model.VALUE_ID = VALUE_ID;
            model.IS_MULTICHOICE = IS_MULTICHOICE.ToIntOrZero();
            model.IDS = wIds;
            model.CALLBACK_FUNCTION = CALLBACK_FUNCTION;
            model.INDEX = INDEX.ToIntOrZero();
            model.LstPhongBanSearch = LstPhongBanSearch;
            return PartialView("_ChoosePhongBanResultV1", model);
        }
        /// <summary>
        /// Function list user to choice
        /// </summary>
        /// <param name="COSO_ID">Cơ sở id</param>
        /// <param name="DONVI_ID">Đơn vị id</param>
        /// <param name="PHONGBAN_ID">Phòng ban id</param>
        /// <param name="TEXT_ID">Id của thẻ hiển thị kết quả chọn</param>
        /// <param name="VALUE_ID">id của thẻ input hidden lưu các id được chọn</param>
        /// <param name="IS_MULTICHOICE">Có cho phép chọn nhiều hay ko</param>
        /// <param name="IDS">Các id đã chọn</param>
        /// <param name="KEYWORD">Keyword tìm kiếm nhân sự</param>
        /// <param name="CALLBACK_FUNCTION">Name of callback function </param>
        /// <param name="INDEX">Index phân biệt trên 1 trang được gọi nhiều lần</param>
        /// <param name="SHOW_CHUC_VU_ID">Có cho phép hiển thị chức vụ hay ko</param>
        /// <param name="EXCLUDE_IDS">Exclude ids</param>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult ListNhanSu(int COSO_ID, int DONVI_ID, int PHONGBAN_ID, string TEXT_ID, string VALUE_ID, string IS_MULTICHOICE, string IDS, string KEYWORD, string CALLBACK_FUNCTION, string INDEX, string SHOW_CHUC_VU_ID, string EXCLUDE_IDS)
        {
            CoSoBs = Get<CosoBusiness>();
            List<COSO> LstCoSo = CoSoBs.All.Where(x => ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            DonViBs = Get<DmDonViBusiness>();
            List<DM_DONVI> LstDonVi = DonViBs.All.Where(
                x => ((DONVI_ID > 0) ? x.ID == DONVI_ID : true)
                     && ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            PhongBanBs = Get<DmPhongbanBusiness>();
            List<DM_PHONGBAN> LstPhongBan = PhongBanBs.All.Where(x => ((DONVI_ID > 0) ? x.DM_DONVI_ID == DONVI_ID : true) && ((PHONGBAN_ID > 0) ? x.DM_PHONGBAN_ID == PHONGBAN_ID : true)).ToList();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            List<DM_NGUOIDUNG> NguoiDungs = NguoiDungBs.All.Where(x => ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)
                && ((DONVI_ID > 0) ? x.DM_DONVI_ID == DONVI_ID : true)
                //&& ((KEYWORD.Length > 0) ? x.TENDANGNHAP.ToLower().Contains(KEYWORD.Trim().ToLower()) : true)
                && ((PHONGBAN_ID > 0) ? x.DM_PHONGBAN_ID == PHONGBAN_ID : true)).ToList();
            List<DM_NGUOIDUNG> NguoiDungSearch = NguoiDungs.Where(x => ((KEYWORD.Length > 0) ? x.TENDANGNHAP.ToLower().Contains(KEYWORD.Trim().ToLower()) : true)).ToList();
            if (NguoiDungSearch.Count() > 0)
            {
                EXCLUDE_IDS = EXCLUDE_IDS.Trim();
                if (!string.IsNullOrEmpty(EXCLUDE_IDS))
                {
                    string[] LstExcludeIds = EXCLUDE_IDS.Split(',');
                    NguoiDungSearch = NguoiDungSearch.Where(x => (!LstExcludeIds.Contains(x.DM_NGUOIDUNG_ID.ToString()))).ToList();
                }
                if (NguoiDungSearch.Count() > 0)
                {
                    string[] IDS_Search = new string[NguoiDungSearch.Count()];
                    int tmp_index = 0;
                    foreach (var tmp_user in NguoiDungSearch)
                    {
                        IDS_Search[tmp_index] = tmp_user.DM_NGUOIDUNG_ID.ToString();
                        tmp_index = tmp_index + 1;
                    }
                    SessionManager.SetValue("LstNguoiDungSearch", IDS_Search);
                }
                else
                {
                    SessionManager.SetValue("LstNguoiDungSearch", null);
                }
            }
            else
            {
                SessionManager.SetValue("LstNguoiDungSearch", null);
            }
            string[] wIds = IDS.Split(',');
            SessionManager.SetValue("LstCoSo", LstCoSo);
            SessionManager.SetValue("LstDonVi", LstDonVi);
            SessionManager.SetValue("LstPhongBan", LstPhongBan);
            ChonNguoiDungModel model = new ChonNguoiDungModel();
            model.TEXT_ID = TEXT_ID;
            model.VALUE_ID = VALUE_ID;
            model.IS_MULTICHOICE = IS_MULTICHOICE.ToIntOrZero();
            model.IDS = wIds;
            model.CALLBACK_FUNCTION = CALLBACK_FUNCTION;
            model.INDEX = INDEX.ToIntOrZero();
            model.LstNguoiDung = NguoiDungs;
            model.LstNguoiDungSearch = NguoiDungSearch;
            model.SHOW_CHUC_VU_ID = SHOW_CHUC_VU_ID.Trim();
            Dictionary<int, string> tmp_dict = new Dictionary<int, string>();
            if (!string.IsNullOrEmpty(model.SHOW_CHUC_VU_ID))
            {
                List<SelectListItem> ListChucVu = new List<SelectListItem>();
                HscbNguoiDungCanBoBs = Get<HscbNguoiDungCanBoBusiness>();
                HscbCCVCBs = Get<HscbCongChucVienChucBusiness>();
                DmChucVuBs = Get<DmChucVuBusiness>();
                foreach (var item in NguoiDungSearch)
                {
                    HSCB_NGUOIDUNG_CANBO NguoiDungCanBoObj = HscbNguoiDungCanBoBs.All.Where(x => x.NGUOIDUNG_ID == item.DM_NGUOIDUNG_ID).FirstOrDefault();
                    if (NguoiDungCanBoObj == null)
                    {
                        tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, "");
                    }
                    else
                    {
                        int HOSO_ID = NguoiDungCanBoObj.HOSO_ID;
                        HSCB_CONGCHUC_VIENCHUC HscbCCVC = HscbCCVCBs.Find(HOSO_ID);
                        if (HscbCCVC == null)
                        {
                            tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, "");
                        }
                        else
                        {
                            if (HscbCCVC.DM_CHUCVU_ID > 0 && HscbCCVC.DM_CHUCVU_ID != null)
                            {
                                DM_CHUCVU ChucVuObj = DmChucVuBs.Find(HscbCCVC.DM_CHUCVU_ID);
                                tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, ChucVuObj == null ? "" : ChucVuObj.TEN_CHUCVU);
                            }
                            else
                            {
                                tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, "");
                            }
                        }
                    }
                }
            }
            model.DictChucVu = tmp_dict;
            return PartialView("_SearchNhanSuResult", model);
        }
        [AllowAnonymous]
        public PartialViewResult ListUser(int COSO_ID, int DONVI_ID, int PHONGBAN_ID, string TEXT_ID, string VALUE_ID, string IS_MULTICHOICE, string IDS, string KEYWORD, string CALLBACK_FUNCTION, string INDEX, string SHOW_CHUC_VU_ID, string EXCLUDE_IDS)
        {
            CoSoBs = Get<CosoBusiness>();
            List<COSO> LstCoSo = CoSoBs.All.Where(x => ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            DonViBs = Get<DmDonViBusiness>();
            List<DM_DONVI> LstDonVi = DonViBs.All.Where(
                x => ((DONVI_ID > 0) ? x.ID == DONVI_ID : true)
                     && ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)).ToList();
            PhongBanBs = Get<DmPhongbanBusiness>();
            List<DM_PHONGBAN> LstPhongBan = PhongBanBs.All.Where(x => ((DONVI_ID > 0) ? x.DM_DONVI_ID == DONVI_ID : true) && ((PHONGBAN_ID > 0) ? x.DM_PHONGBAN_ID == PHONGBAN_ID : true)).ToList();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            List<DM_NGUOIDUNG> NguoiDungs = NguoiDungBs.All.Where(x => ((COSO_ID > 0) ? x.COSO_ID == COSO_ID : true)
                && ((DONVI_ID > 0) ? x.DM_DONVI_ID == DONVI_ID : true)
                //&& ((KEYWORD.Length > 0) ? x.TENDANGNHAP.ToLower().Contains(KEYWORD.Trim().ToLower()) : true)
                && ((PHONGBAN_ID > 0) ? x.DM_PHONGBAN_ID == PHONGBAN_ID : true)).ToList();
            KEYWORD = Ultilities.RemoveSign4VietnameseString(KEYWORD.Trim().ToLower());
            List<DM_NGUOIDUNG> NguoiDungSearch = NguoiDungs.Where(x =>
                ((KEYWORD.Length > 0) ?
                (
                x.TENDANGNHAP.ToLower().Contains(KEYWORD)
                || Ultilities.RemoveSign4VietnameseString(x.HOTEN.ToLower()).Contains(KEYWORD)
                ) : true)
                ).ToList();
            if (NguoiDungSearch.Count() > 0)
            {
                EXCLUDE_IDS = EXCLUDE_IDS.Trim();
                if (!string.IsNullOrEmpty(EXCLUDE_IDS))
                {
                    string[] LstExcludeIds = EXCLUDE_IDS.Split(',');
                    NguoiDungSearch = NguoiDungSearch.Where(x =>
                        (!LstExcludeIds.Contains(x.DM_NGUOIDUNG_ID.ToString()))
                        ).ToList();
                }
                if (NguoiDungSearch.Count() > 0)
                {
                    string[] IDS_Search = new string[NguoiDungSearch.Count()];
                    int tmp_index = 0;
                    foreach (var tmp_user in NguoiDungSearch)
                    {
                        IDS_Search[tmp_index] = tmp_user.DM_NGUOIDUNG_ID.ToString();
                        tmp_index = tmp_index + 1;
                    }
                    SessionManager.SetValue("LstNguoiDungSearch", IDS_Search);
                }
                else
                {
                    SessionManager.SetValue("LstNguoiDungSearch", null);
                }
            }
            else
            {
                SessionManager.SetValue("LstNguoiDungSearch", null);
            }
            string[] wIds = IDS.Split(',');
            SessionManager.SetValue("LstCoSo", LstCoSo);
            SessionManager.SetValue("LstDonVi", LstDonVi);
            SessionManager.SetValue("LstPhongBan", LstPhongBan);
            ChonNguoiDungModel model = new ChonNguoiDungModel();
            model.TEXT_ID = TEXT_ID;
            model.VALUE_ID = VALUE_ID;
            model.IS_MULTICHOICE = IS_MULTICHOICE.ToIntOrZero();
            model.IDS = wIds;
            model.CALLBACK_FUNCTION = CALLBACK_FUNCTION;
            model.INDEX = INDEX.ToIntOrZero();
            model.LstNguoiDung = NguoiDungs;
            model.LstNguoiDungSearch = NguoiDungSearch;
            model.SHOW_CHUC_VU_ID = SHOW_CHUC_VU_ID.Trim();
            Dictionary<int, string> tmp_dict = new Dictionary<int, string>();
            if (!string.IsNullOrEmpty(model.SHOW_CHUC_VU_ID))
            {
                List<SelectListItem> ListChucVu = new List<SelectListItem>();
                HscbNguoiDungCanBoBs = Get<HscbNguoiDungCanBoBusiness>();
                HscbCCVCBs = Get<HscbCongChucVienChucBusiness>();
                DmChucVuBs = Get<DmChucVuBusiness>();
                foreach (var item in NguoiDungSearch)
                {
                    HSCB_NGUOIDUNG_CANBO NguoiDungCanBoObj = HscbNguoiDungCanBoBs.All.Where(x => x.NGUOIDUNG_ID == item.DM_NGUOIDUNG_ID).FirstOrDefault();
                    if (NguoiDungCanBoObj == null)
                    {
                        tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, "");
                    }
                    else
                    {
                        int HOSO_ID = NguoiDungCanBoObj.HOSO_ID;
                        HSCB_CONGCHUC_VIENCHUC HscbCCVC = HscbCCVCBs.Find(HOSO_ID);
                        if (HscbCCVC == null)
                        {
                            tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, "");
                        }
                        else
                        {
                            if (HscbCCVC.DM_CHUCVU_ID > 0 && HscbCCVC.DM_CHUCVU_ID != null)
                            {
                                DM_CHUCVU ChucVuObj = DmChucVuBs.Find(HscbCCVC.DM_CHUCVU_ID);
                                tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, ChucVuObj == null ? "" : ChucVuObj.TEN_CHUCVU);
                            }
                            else
                            {
                                tmp_dict.Add((int)item.DM_NGUOIDUNG_ID, "");
                            }
                        }
                    }
                }
            }
            model.DictChucVu = tmp_dict;
            return PartialView("_ChooseUserResultV1", model);
        }
        public PartialViewResult GetTopMenu()
        {
            return PartialView("GetTopMenu");
        }
        public JsonResult DeleteFileNhanSu(int id)
        {
            try
            {
                string URLPath = WebConfigurationManager.AppSettings["HSCBFileUpload"];
                HscbFilesBusiness = Get<HscbFilesBusiness>();
                if (id > 0 && id.GetType() == typeof(int))
                {
                    var fileResult = HscbFilesBusiness.Find(id);
                    if (fileResult != null)
                    {
                        string filepath = fileResult.DUONGDAN_FILE;
                        bool fileExists = System.IO.File.Exists(filepath);
                        if (fileExists)
                        {
                            System.IO.File.Delete(filepath);
                        }
                        HscbFilesBusiness.Delete(id);
                        HscbFilesBusiness.Save();
                    }
                }
                return Json("1");
            }
            catch
            {
                return Json("0");
            }
        }
        public FileResult DownloadFileNhanSu(int id)
        {
            HscbFilesBusiness = Get<HscbFilesBusiness>();
            var fileResult = HscbFilesBusiness.Find(id);
            if (fileResult != null)
            {
                string filepath = fileResult.DUONGDAN_FILE;
                bool fileExists = System.IO.File.Exists(filepath);
                if (fileExists)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileResult.DUONGDAN_FILE);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileResult.TENTAILIEU + fileResult.DINHDANG_FILE);
                }
            }
            //string URLPath = WebConfigurationManager.AppSettings["HSCBFileUpload"];
            //HscbFilesBusiness = Get<HscbFilesBusiness>();
            //if (id > 0 && id.GetType() == typeof(int))
            //{
            //    var file = HscbFilesBusiness.Find(id);
            //    if (file != null)
            //    {
            //        string contentType = file.DINHDANG_FILE;
            //        string path = URLPath + file.DUONGDAN_FILE;
            //        var filename = path.Split('\\');
            //        string fileSave = filename[filename.Count() - 1];
            //        return File(path, contentType, fileSave);
            //    }
            //}
            throw new BusinessException("Xin lỗi, Không tìm thấy file yêu cầu!");
            //return null;
        }
        public JsonResult DeleteFile(int id)
        {
            try
            {
                string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
                var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
                if (id > 0 && id.GetType() == typeof(int))
                {
                    TAILIEUDINHKEM taikieu = TaiLieuDinhKemBusiness.Find(id);
                    if (taikieu != null)
                    {
                        string contentType = taikieu.DINHDANG_FILE;
                        string path = URLPath + taikieu.DUONGDAN_FILE;
                        System.IO.File.Delete(path);
                    }
                    TaiLieuDinhKemBusiness.Delete(id);
                    TaiLieuDinhKemBusiness.Save();
                }
                return Json("1");
            }
            catch
            {
                return Json("0");
            }
        }
        public FileResult DownloadFile(int id)
        {
            string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
            var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            if (id > 0 && id.GetType() == typeof(int))
            {
                TAILIEUDINHKEM taikieu = TaiLieuDinhKemBusiness.Find(id);
                if (taikieu != null)
                {
                    string contentType = taikieu.DINHDANG_FILE;
                    string path = URLPath + taikieu.DUONGDAN_FILE;
                    var filename = path.Split('\\');
                    string fileSave = filename[filename.Count() - 1];
                    return File(path, contentType, fileSave);
                }
            }
            return null;
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult LayNguoiDungByEmail(string email)
        {
            var dmNguoidungBussiness = Get<DmNguoidungBusiness>();
            DM_NGUOIDUNG nd = dmNguoidungBussiness.All.Where(ng => ng.EMAIL.ToLower() == email.ToLower()).FirstOrDefault();
            if (nd != null)
            {
                var key = MaHoaMatKhau.Encode_Data(nd.DM_NGUOIDUNG_ID + CreateStringRandom() + "BTNQUOCGIA");
                return Json(new { key = key }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        private string CreateStringRandom()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SendMail(string to, string subject, string captcha, bool isHtml = true)
        {
            if (SessionManager.HasValue("ForgetCaptcha"))
            {
                if (string.IsNullOrEmpty(captcha))
                {
                    return Json(new { successful = false, chapchar = true, message = Web.Common.UIConstant.MESSAGE_CONFIMATION_EMPTY }, JsonRequestBehavior.AllowGet);
                }
                string scaptcha = (string)SessionManager.GetValue("ForgetCaptcha");
                if (captcha.Length != 7 || captcha != scaptcha)
                {
                    return Json(new { successful = false, chapchar = true, message = Web.Common.UIConstant.MESSAGE_CONFIMATION_NOT_CORRECT }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { successful = false, chapchar = true, message = Web.Common.UIConstant.MESSAGE_CONFIMATION_EMPTY }, JsonRequestBehavior.AllowGet);
            }
            var fromAddress = new MailAddress(Constant.FROMEMAIL, Constant.NAMEEMAIL);
            var toAddress = new MailAddress(to);
            const string fromPassword = Constant.PASSEMAIL;
            DmNguoidungBusiness BNguoiDung = Get<DmNguoidungBusiness>();
            var objNguoiDung = BNguoiDung.All.Where(o => o.EMAIL.ToLower() == to.ToLower()).FirstOrDefault();
            if (objNguoiDung == null)
                return Json(new { successful = false, chapchar = false, Web.Common.UIConstant.MESSAGE_EMAIL_NOT_EXIST }, JsonRequestBehavior.AllowGet);
            var key = MaHoaMatKhau.Encode_Data(objNguoiDung.DM_NGUOIDUNG_ID + CreateStringRandom() + "CUCTINHOCHOAVN");
            objNguoiDung.CODERESETPASS = key;
            BNguoiDung.Save(objNguoiDung);
            var smtp = new SmtpClient
            {
                Host = Constant.HOST,
                Port = Constant.PORT,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = "Bạn vui lòng ấn vào đường link sau để thay đổi lại mật khẩu của mình: <a href='" + GetSiteRoot() + "/Account/ResetPassword/" + key + "'>Nhấp vào đây</a>",
                IsBodyHtml = isHtml
            })
            {
                smtp.Send(message);
            }
            return Json(new { successful = true }, JsonRequestBehavior.AllowGet);
        }
        public static string GetSiteRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;
            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";
            string sOut = protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + System.Web.HttpContext.Current.Request.ApplicationPath;
            if (sOut.EndsWith("/"))
            {
                sOut = sOut.Substring(0, sOut.Length - 1);
            }
            return sOut;
        }
        [ValidateInput(false)]
        public void InsertChatContent(string msg, string from, string to, long group, int coso)
        {
            ChatNoidungBusiness = Get<ChatNoidungBusiness>();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            var from_user = NguoiDungBs.All.Where(o => o.TENDANGNHAP == from && o.COSO_ID == coso).FirstOrDefault();
            var to_user = NguoiDungBs.All.Where(o => o.TENDANGNHAP == to && o.COSO_ID == coso).FirstOrDefault();
            if (from_user != null && to_user != null)
            {
                CHAT_NOIDUNG noidung = new CHAT_NOIDUNG();
                noidung.NGUOIGUI_ID = from_user.DM_NGUOIDUNG_ID;
                noidung.NGUOINHAN_ID = to_user.DM_NGUOIDUNG_ID;
                noidung.FROMUSER = from_user.TENDANGNHAP;
                noidung.TOUSER = to_user.TENDANGNHAP;
                noidung.FROMFULLNAME = from_user.HOTEN;
                noidung.TOFULLNAME = to_user.HOTEN;
                if (group > 0)
                {
                    noidung.GROUPCHAT_ID = group;
                }
                noidung.COSO_ID = coso;
                noidung.NOIDUNG = msg;
                noidung.NGAYGUI = DateTime.Now;
                noidung.IS_DELETE = false;
                noidung.FTS = msg.ConvertToFTS();
                ChatNoidungBusiness.Save(noidung);
            }
        }
        [ValidateInput(false)]
        public void InsertChatGroupContent(string msg, string from, long group, int coso)
        {
            ChatNoidungBusiness = Get<ChatNoidungBusiness>();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            if (group > 0)
            {
                var from_user = NguoiDungBs.All.Where(o => o.TENDANGNHAP == from && o.COSO_ID == coso).FirstOrDefault();
                if (from_user != null)
                {
                    CHAT_NOIDUNG noidung = new CHAT_NOIDUNG();
                    noidung.NGUOIGUI_ID = from_user.DM_NGUOIDUNG_ID;
                    noidung.FROMUSER = from_user.TENDANGNHAP;
                    noidung.FROMFULLNAME = from_user.HOTEN;
                    noidung.GROUPCHAT_ID = group;
                    noidung.COSO_ID = coso;
                    noidung.NOIDUNG = msg;
                    noidung.NGAYGUI = DateTime.Now;
                    noidung.IS_DELETE = false;
                    noidung.FTS = msg.ConvertToFTS();
                    ChatNoidungBusiness.Save(noidung);
                }
            }
        }
        [ValidateInput(false)]
        public void UpdateGroupName(string groupname, string msg, string from, long group, int coso)
        {
            ChatNoidungBusiness = Get<ChatNoidungBusiness>();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            ChatGroupBusiness = Get<ChatGroupBusiness>();
            if (group > 0)
            {
                var GroupChat = ChatGroupBusiness.Find(group);
                GroupChat.TENNHOM = groupname;
                GroupChat.NGAYSUA = DateTime.Now;
                GroupChat.NGUOISUA = from;
                ChatGroupBusiness.Save(GroupChat);
                var from_user = NguoiDungBs.All.Where(o => o.TENDANGNHAP == from && o.COSO_ID == coso).FirstOrDefault();
                if (from_user != null)
                {
                    CHAT_NOIDUNG noidung = new CHAT_NOIDUNG();
                    noidung.NGUOIGUI_ID = from_user.DM_NGUOIDUNG_ID;
                    noidung.FROMUSER = from_user.TENDANGNHAP;
                    noidung.FROMFULLNAME = from_user.HOTEN;
                    noidung.GROUPCHAT_ID = group;
                    noidung.COSO_ID = coso;
                    noidung.NOIDUNG = msg;
                    noidung.NGAYGUI = DateTime.Now;
                    noidung.IS_DELETE = false;
                    noidung.FTS = msg.ConvertToFTS();
                    ChatNoidungBusiness.Save(noidung);
                }
            }
        }
        //LeftGroupChat
        [ValidateInput(false)]
        public void LeftGroupChat(int coso, long group_id, string username)
        {
            ChatNoidungBusiness = Get<ChatNoidungBusiness>();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            ChatGroupUserBusiness = Get<ChatGroupUserBusiness>();
            if (group_id > 0)
            {
                var user_left = NguoiDungBs.All.Where(o => o.TENDANGNHAP == username && o.COSO_ID == coso).FirstOrDefault();
                var user_group_left = ChatGroupUserBusiness.All.Where(o => o.GROUP_ID == group_id && o.USER_ID == user_left.DM_NGUOIDUNG_ID).FirstOrDefault();
                ChatGroupUserBusiness.Delete(user_group_left.ID);
                ChatGroupUserBusiness.Save();
                if (user_left != null)
                {
                    CHAT_NOIDUNG noidung = new CHAT_NOIDUNG();
                    noidung.NGUOIGUI_ID = user_left.DM_NGUOIDUNG_ID;
                    noidung.FROMUSER = user_left.TENDANGNHAP;
                    noidung.FROMFULLNAME = user_left.HOTEN;
                    noidung.GROUPCHAT_ID = group_id;
                    noidung.COSO_ID = coso;
                    noidung.NOIDUNG = "Đã rời khỏi nhóm chat";
                    noidung.NGAYGUI = DateTime.Now;
                    noidung.IS_DELETE = false;
                    noidung.FTS = noidung.NOIDUNG.ConvertToFTS();
                    ChatNoidungBusiness.Save(noidung);
                }
            }
        }
        [ValidateInput(false)]
        public PartialViewResult Chat(int? cosoId, string fromUser, string toUser, string fromFullName, string toFullName,
           int? soCuaSoChat, int? reload, string chat_id, int? index)
        {
            ChatNoidungBusiness = Get<ChatNoidungBusiness>();
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            ChatViewModel model = new ChatViewModel();
            model.cosoId = cosoId.Value;
            model.fromUser = fromUser;
            model.toUser = toUser;
            model.fromFullName = fromFullName;
            model.toFullName = toFullName;
            model.soCuaSoChat = soCuaSoChat.Value;
            model.currentUserName = user.Username;
            model.chat_id = chat_id;
            model.reload = reload.Value;
            model.index = index.Value;
            model.listChat = ChatNoidungBusiness.GetListChat(fromUser, toUser, model.cosoId, DateTime.Now, 0, 30, 0);
            return PartialView("_Chat", model);
        }
        public JsonResult UploadFileAttachmentChat(IEnumerable<HttpPostedFileBase> attachment, FormCollection col)
        {
            UploadFileTool upload = new UploadFileTool();
            string[] filename = new string[attachment.Count()];
            List<string> OutFilePath;
            List<string> OutFileName;
            List<string> OutFileExt;
            List<long> OutFileID;
            upload.UploadCustomFileAndOutPath(attachment, true, string.Empty, URLPath, 0, null, filename, 0, out OutFilePath, out OutFileName, out OutFileExt, out OutFileID, LOAITAILIEU.CHAT, "Chat-Trao đổi");
            return Json(new { OutFilePath = OutFilePath, OutFileName = OutFileName, OutFileExt = OutFileExt, OutFileID = OutFileID });
        }
        [HttpPost]
        public JsonResult InsertGroupChat(int cosoId, string created_user, string listUserName)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            ChatGroupBusiness = Get<ChatGroupBusiness>();
            ChatGroupUserBusiness = Get<ChatGroupUserBusiness>();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            var list_UserName = listUserName.Split(',');
            CHAT_GROUP group = new CHAT_GROUP();
            if (list_UserName != null && list_UserName.Count() > 0)
            {
                group.COSO_ID = cosoId;
                group.USERCREATE_ID = (long)user.UserID;
                group.NGUOITAO = user.Username;
                group.NGAYTAO = DateTime.Now;
                ChatGroupBusiness.Save(group);
                foreach (var username in list_UserName)
                {
                    var _username = username.Trim();
                    var userInfor = NguoiDungBs.All.Where(o => o.TENDANGNHAP.ToLower() == _username && o.COSO_ID == cosoId).FirstOrDefault();
                    CHAT_GROUP_USER grp_user = new CHAT_GROUP_USER();
                    grp_user.GROUP_ID = group.ID;
                    grp_user.USER_ID = userInfor.DM_NGUOIDUNG_ID;
                    grp_user.USERNAME = userInfor.TENDANGNHAP;
                    grp_user.FULLNAME = userInfor.HOTEN;
                    ChatGroupUserBusiness.Save(grp_user);
                }
            }
            var groupChat_id = string.Format("g_{0}_{1}", cosoId, group.ID);
            return Json(new { groupId = group.ID, groupChat_id = groupChat_id });
        }
        [ValidateInput(false)]
        public PartialViewResult ChatGroup(int? cosoId, string createdUser, long? groupId, string currentUserName,
           int? soCuaSoChat, int? reload, string groupChat_id)
        {
            ChatNoidungBusiness = Get<ChatNoidungBusiness>();
            ChatGroupBusiness = Get<ChatGroupBusiness>();
            ChatGroupUserBusiness = Get<ChatGroupUserBusiness>();
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            var groupChat = ChatGroupBusiness.Find(groupId.Value);
            ChatViewModel model = new ChatViewModel();
            model.listFullName = ChatGroupUserBusiness.GetListUserName(groupId.Value);
            model.groupName = string.IsNullOrEmpty(groupChat.TENNHOM) ? model.listFullName : groupChat.TENNHOM;
            model.cosoId = cosoId.Value;
            model.soCuaSoChat = soCuaSoChat.Value;
            model.currentUserName = currentUserName;
            model.groupChat_id = groupChat_id;
            model.group_id = groupId.Value;
            model.reload = reload.Value;
            //model.index = index.Value;
            model.listChat = ChatNoidungBusiness.GetListChat(string.Empty, string.Empty, model.cosoId, DateTime.Now, groupId.Value, 15, 0);
            return PartialView("_ChatGroup", model);
        }
        //UpdateGroupChat
        [HttpPost]
        public JsonResult UpdateGroupChat(int cosoId, long? group_id, string listUserName)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            ChatGroupBusiness = Get<ChatGroupBusiness>();
            ChatGroupUserBusiness = Get<ChatGroupUserBusiness>();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            var list_UserName = listUserName.Split(',');
            CHAT_GROUP group = ChatGroupBusiness.Find(group_id.Value);
            string user_added = "";
            string fullname_add = "";
            if (list_UserName != null && list_UserName.Count() > 0)
            {
                group.COSO_ID = cosoId;
                group.NGUOISUA = user.Username;
                group.NGAYSUA = DateTime.Now;
                ChatGroupBusiness.Save(group);
                foreach (var username in list_UserName)
                {
                    var _username = username.Trim();
                    var userInfor = NguoiDungBs.All.Where(o => o.TENDANGNHAP.ToLower() == _username && o.COSO_ID == cosoId).FirstOrDefault();
                    var user_in_group = ChatGroupUserBusiness.All.Where(o => o.GROUP_ID == group_id.Value && o.USER_ID == userInfor.DM_NGUOIDUNG_ID).FirstOrDefault();
                    if (user_in_group == null)
                    {
                        CHAT_GROUP_USER grp_user = new CHAT_GROUP_USER();
                        grp_user.GROUP_ID = group.ID;
                        grp_user.USER_ID = userInfor.DM_NGUOIDUNG_ID;
                        grp_user.USERNAME = userInfor.TENDANGNHAP;
                        grp_user.FULLNAME = userInfor.HOTEN;
                        ChatGroupUserBusiness.Save(grp_user);
                        user_added += _username + ",";
                        fullname_add += userInfor.HOTEN + ",";
                    }
                }
            }
            var groupChat_id = string.Format("g_{0}_{1}", cosoId, group.ID);
            return Json(new { groupId = group.ID, groupChat_id = groupChat_id, user_added = user_added, fullname_added = fullname_add });
        }
        public JsonResult UploadFileAttachmentGroupChat(IEnumerable<HttpPostedFileBase> attachment, FormCollection col)
        {
            UploadFileTool upload = new UploadFileTool();
            string[] filename = new string[attachment.Count()];
            List<string> OutFilePath;
            List<string> OutFileName;
            List<string> OutFileExt;
            List<long> OutFileID;
            upload.UploadCustomFileAndOutPath(attachment, true, string.Empty, URLPath, 0, null, filename, 0, out OutFilePath, out OutFileName, out OutFileExt, out OutFileID, LOAITAILIEU.CHAT, "ChatGroup-Trao đổi");
            return Json(new { OutFilePath = OutFilePath, OutFileName = OutFileName, OutFileExt = OutFileExt, OutFileID = OutFileID });
        }
        [HttpPost]
        public void InsertMessage(FormCollection col)
        {
            //Lấy danh sách user nhận tin nhắn
            //dạng string chuyển thành List<string>: List<UserName>
            //các user nhận tin nhắn, ví dụ: namdv, cuc_truong, tulq
            var usernames = col["USERNAMES"];
            if (!string.IsNullOrEmpty(usernames))
            {
                SysTinnhanBusiness SysTinnhanBusiness = Get<SysTinnhanBusiness>();
                List<SYS_TINNHAN> list_msg = new List<SYS_TINNHAN>();
                var list_username = usernames.Split(',');
                if (list_username != null && list_username.Count() > 0)
                {
                    SYS_TINNHAN msg = new SYS_TINNHAN();
                    msg.COSO_ID = col["COSO_ID"].ToIntOrNULL();
                    //Thông tin người gửi tin nhắn
                    msg.FROM_USER_ID = col["FROM_USER_ID"].ToIntOrNULL();
                    msg.FROM_USERNAME = col["FROM_USERNAME"];
                    //Có cấu hình hiển thị dạng cửa sổ popup hay không
                    if (!string.IsNullOrEmpty(col["IS_POPUP"]))
                    {
                        msg.IS_POPUP = bool.Parse(col["IS_POPUP"]);
                    }
                    else
                    {
                        msg.IS_POPUP = false;
                    }
                    //Trong trường hợp không lặp, thì gán luôn msg.IS_REPEAT = false;  và bỏ qua region phía dưới
                    #region Thông tin lặp thông báo
                    if (!string.IsNullOrEmpty(col["IS_REPEAT"]))
                    {
                        msg.IS_REPEAT = bool.Parse(col["IS_REPEAT"]);
                        #region REPEAT_T2 (Thứ 2)
                        if (!string.IsNullOrEmpty(col["REPEAT_T2"]))
                        {
                            msg.REPEAT_T2 = bool.Parse(col["REPEAT_T2"]);
                        }
                        else
                        {
                            msg.REPEAT_T2 = false;
                        }
                        #endregion
                        #region REPEAT_T3 (Thứ 3)
                        if (!string.IsNullOrEmpty(col["REPEAT_T3"]))
                        {
                            msg.REPEAT_T3 = bool.Parse(col["REPEAT_T3"]);
                        }
                        else
                        {
                            msg.REPEAT_T3 = false;
                        }
                        #endregion
                        #region REPEAT_T4 (Thứ 4)
                        if (!string.IsNullOrEmpty(col["REPEAT_T4"]))
                        {
                            msg.REPEAT_T4 = bool.Parse(col["REPEAT_T4"]);
                        }
                        else
                        {
                            msg.REPEAT_T4 = false;
                        }
                        #endregion
                        #region REPEAT_T5 (Thứ 5)
                        if (!string.IsNullOrEmpty(col["REPEAT_T5"]))
                        {
                            msg.REPEAT_T5 = bool.Parse(col["REPEAT_T5"]);
                        }
                        else
                        {
                            msg.REPEAT_T5 = false;
                        }
                        #endregion
                        #region REPEAT_T6 (Thứ 6)
                        if (!string.IsNullOrEmpty(col["REPEAT_T6"]))
                        {
                            msg.REPEAT_T6 = bool.Parse(col["REPEAT_T6"]);
                        }
                        else
                        {
                            msg.REPEAT_T6 = false;
                        }
                        #endregion
                        #region REPEAT_T7 (Thứ 7)
                        if (!string.IsNullOrEmpty(col["REPEAT_T7"]))
                        {
                            msg.REPEAT_T7 = bool.Parse(col["REPEAT_T7"]);
                        }
                        else
                        {
                            msg.REPEAT_T7 = false;
                        }
                        #endregion
                        #region REPEAT_T8 (Chủ nhật)
                        if (!string.IsNullOrEmpty(col["REPEAT_T8"]))
                        {
                            msg.REPEAT_T8 = bool.Parse(col["REPEAT_T8"]);
                        }
                        else
                        {
                            msg.REPEAT_T8 = false;
                        }
                        #endregion
                    }
                    else
                    {
                        msg.IS_REPEAT = false;
                    }
                    #endregion
                    //Ngày, thời gian hiển thị
                    msg.TIME_DISPLAY = col["TIME_DISPLAY"].ToDateTime();
                    //Url chi tiết đi kèm với tin nhắn: ví dụ url đến chi tiết văn bản, chi tiết đơn nâng lương ...
                    msg.URL = col["URL"];
                    //Tiêu đề của tin nhắn
                    msg.TIEUDE = col["TIEUDE"];
                    //Nội dung tin nhắn
                    msg.NOIDUNG = col["NOIDUNG"];
                    msg.NGAYTAO = DateTime.Now;
                    msg.NGAYSUA = DateTime.Now;
                    msg.NGUOITAO = msg.FROM_USERNAME;
                    msg.NGUOISUA = msg.FROM_USERNAME;
                    //Danh sách người nhận
                    foreach (var to_user in list_username)
                    {
                        if (!string.IsNullOrEmpty(usernames))
                        {
                            msg.TO_USERNAME = to_user;
                            list_msg.Add(msg);
                        }
                    }
                    SysTinnhanBusiness.SaveListMsg(list_msg);
                }
            }
        }
        public PartialViewResult GetLog(int ID, int TYPE, string exclude = "")
        {
            HscbNhanSuLogBusiness = Get<HscbNhanSuLogBusiness>();
            var result = new List<int>();
            if (!string.IsNullOrEmpty(exclude))
            {
                var lstExclude = exclude.ToListInt(',');
                if (lstExclude != null && lstExclude.Count > 0)
                {
                    result = lstExclude;
                }
            }
            return PartialView("_ListNhanSu", HscbNhanSuLogBusiness.GetListLogBO(ID, TYPE, result));
        }
        public PartialViewResult GetLogV1(int ID, int TYPE, string exclude = "")
        {
            HscbNhanSuLogBusiness = Get<HscbNhanSuLogBusiness>();
            var result = new List<int>();
            if (!string.IsNullOrEmpty(exclude))
            {
                var lstExclude = exclude.ToListInt(',');
                if (lstExclude != null && lstExclude.Count > 0)
                {
                    result = lstExclude;
                }
            }
            return PartialView("_ListNhanSuV1", HscbNhanSuLogBusiness.GetListLogBOV1(ID, TYPE, result));
        }
        public PartialViewResult ShowTinNhan()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            SysTinnhanBusiness SysTinnhanBusiness = Get<SysTinnhanBusiness>();
            var listTinNhan = SysTinnhanBusiness.GetListTinNhan(user.UserID, 15, 1);
            return PartialView("_ShowTinNhan", listTinNhan);
        }
        public PartialViewResult GetCongViec()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            CongViecDBViewModel model = new CongViecDBViewModel();
            if (user != null)
            {
                CongViecBusiness CongViecBusiness = Get<CongViecBusiness>();
                model.ListCongViecXuLyChinh = CongViecBusiness.GetListCongViecXuLyChinh(user.CoSoID.Value, LOAITAILIEU.CONGVIEC, (long)user.UserID, string.Empty, null, null, null, null, 0, 0, 10);
                model.ListCongViecThamGiaXuLy = CongViecBusiness.GetListCongViecThamGiaXuLy(user.CoSoID.Value, LOAITAILIEU.CONGVIEC, (long)user.UserID, string.Empty, null, null, null, null, 0, 0, 10);
                model.ListCongViecTheoDoi = CongViecBusiness.GetListCongViecTheoDoi(user.CoSoID.Value, LOAITAILIEU.CONGVIEC, (long)user.UserID, string.Empty, null, null, null, null, 0, 0, 10);
            }
            return PartialView("_GetCongViec", model);
        }
        public PartialViewResult GetLeftMenu(string url)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (user.ListCN != null && user.ListCN.Count > 0)
            {
                var cn = user.ListCN.Where(o => o.URL.ToLower() == url.ToLower()).FirstOrDefault();
                if (cn != null)
                {
                    var list_cn = user.ListCN.Where(o => o.CHUCNANG_CHA == cn.CHUCNANG_CHA).OrderBy(o => o.TT_HIENTHI).ToList();
                    return PartialView("_GetLeftMenu", list_cn);
                }
            }
            return PartialView("_GetLeftMenu", new List<ChucNangBO>());
        }
        public PartialViewResult GetMenuChild()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            string currentArea = Request.RequestContext.RouteData.DataTokens["area"].ToString();
            List<ChucNangBO> ListChucNang = new List<ChucNangBO>();
            if (!string.IsNullOrEmpty(currentArea))
            {
                var lst_tt = UserInfo.ListThaoTac.Where(x => x.THAOTAC.ToLower().Contains(currentArea.ToLower())).Select(x => x.DM_CHUCNANG_ID).ToList();
                //var _ChucNangChaID = UserInfo.ListCN.Where(o =>lst_tt.Contains(o.DM_CHUCNANG_ID)).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                var _ChucNangChaID = UserInfo.ListCN.Where(o => o.URL.ToLower().Contains(currentArea.ToLower())).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                if (_ChucNangChaID != null)
                    ListChucNang = UserInfo.ListCN.Where(o => o.CHUCNANG_CHA == _ChucNangChaID).ToList();
            }
            if (UserInfo.RoleID > 0)
            {
                ListChucNang = ListChucNang.Where(x => x.VAITROID == UserInfo.RoleID).ToList();
            }
            return PartialView("_MenuListChild", ListChucNang);
        }
        public PartialViewResult NhanSuMenuChild()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            string currentArea = Request.RequestContext.RouteData.DataTokens["area"].ToString();
            List<ChucNangBO> ListChucNang = new List<ChucNangBO>();
            if (!string.IsNullOrEmpty(currentArea))
            {
                var lst_tt = UserInfo.ListThaoTac.Where(x => x.THAOTAC.ToLower().Contains(currentArea.ToLower())).Select(x => x.DM_CHUCNANG_ID).ToList();
                var _ChucNangChaID = UserInfo.ListCN.Where(o => lst_tt.Contains(o.DM_CHUCNANG_ID)).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                //var _ChucNangChaID = UserInfo.ListCN.Where(o => o.URL.ToLower().Contains(currentArea.ToLower())).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                if (_ChucNangChaID != null)
                    ListChucNang = UserInfo.ListCN.Where(o => o.CHUCNANG_CHA == _ChucNangChaID).OrderBy(o => o.TT_HIENTHI).ToList();
            }
            if (UserInfo.RoleID > 0)
            {
                ListChucNang = ListChucNang.Where(x => x.VAITROID == UserInfo.RoleID).OrderBy(o => o.TT_HIENTHI).ToList();
            }
            return PartialView("_NhanSuMenu", ListChucNang);
        }
        public PartialViewResult GetSearchForm(string ActionFindName, string NameTextSearch, string btnSearch = "btnSearch")
        {
            string currentArea = Request.RequestContext.RouteData.DataTokens["area"].ToString();
            string currentController = Request.RequestContext.RouteData.Values["controller"].ToString();
            string currentAction = Request.RequestContext.RouteData.Values["action"].ToString();
            ViewBag.ActionFindName = ActionFindName;
            ViewBag.NameTextSearch = NameTextSearch;
            ViewBag.btnSearch = btnSearch;
            ViewBag.currentController = currentController;
            ViewBag.currentAction = currentAction;
            ViewBag.currentArea = currentArea;
            return PartialView("_GetSearchForm");
        }
        public PartialViewResult CongViecToolBar(string preTitle, string content, int itemType, long itemID)
        {
            CongViecToolBarViewModel model = new CongViecToolBarViewModel();
            switch (itemType)
            {
                #region Đơn xin nghỉ
                case LOAITAILIEU.DONXINNGHI://Đơn xin nghỉ
                    {
                        NpDonXinNghiBusiness NpDonXinNghiBusiness = Get<NpDonXinNghiBusiness>();
                        var item = NpDonXinNghiBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.LYDO;
                        }
                        break;
                    }
                #endregion
                #region Đăng ký vắng mặt
                case LOAITAILIEU.DANGKYVANGMAT://Đăng ký vắng mặt
                    {
                        VmDonXinVangMatBusiness VmDonXinVangMatBusiness = Get<VmDonXinVangMatBusiness>();
                        var item = VmDonXinVangMatBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TIEUDE;
                        }
                        break;
                    }
                #endregion
                #region Quản lý tài sản
                case LOAITAILIEU.TAISAN://Quản lý tài sản
                    {
                        TsTaiSanBusiness TsTaiSanBusiness = Get<TsTaiSanBusiness>();
                        var item = TsTaiSanBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TENTAISAN;
                        }
                        break;
                    }
                #endregion
                #region Quản lý vi phạm
                case LOAITAILIEU.QUANLYVIPHAM://Quản lý vi phạm
                    {
                        VPViPhamBusiness VPViPhamBusiness = Get<VPViPhamBusiness>();
                        var item = VPViPhamBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TIEUDE;
                            model.Content = item.LYDO;
                        }
                        break;
                    }
                #endregion
                #region Quản lý Đoàn viên
                case LOAITAILIEU.QUANLYDOANVIEN://Quản lý Đoàn viên
                    {
                        QLDoanVienBusiness QLDoanVienBusiness = Get<QLDoanVienBusiness>();
                        var item = QLDoanVienBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.HOVATEN;
                        }
                        break;
                    }
                #endregion
                #region Quản lý Đảng viên
                case LOAITAILIEU.QUANLYDANGVIEN://Quản lý Đảng viên
                    {
                        //QLDangVienBusiness QLDangVienBusiness = Get<QLDangVienBusiness>();
                        //var item = QLDangVienBusiness.Find(itemID);
                        //if (item != null)
                        //{
                        //    model.Title = preTitle + ": " + item.HOTEN;
                        //}
                        break;
                    }
                #endregion
                #region Quản lý Nâng lương trước thời hạn
                case LOAITAILIEU.NANGLUONGTTH://Quản lý Nâng lương trước thời hạn
                    {
                        NltthDonxinNangluongBusiness NltthDonxinNangluongBusiness = Get<NltthDonxinNangluongBusiness>();
                        var item = NltthDonxinNangluongBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TUDANHGIA;
                        }
                        break;
                    }
                #endregion
                #region Quản lý Nâng lương thường xuyên
                case LOAITAILIEU.NLTX_DONNANGLUONG://Quản lý Nâng lương thường xuyên
                    {
                        NLTXDonXinNangLuongBusiness NLTXDonXinNangLuongBusiness = Get<NLTXDonXinNangLuongBusiness>();
                        var item = NLTXDonXinNangLuongBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TIEU_DE;
                        }
                        break;
                    }
                case LOAITAILIEU.KH_NLTX://kế hoạch Nâng lương thường xuyên
                    {
                        NLTXKeHoachNangLuongBusiness NLTXKeHoachNangLuongBusiness = Get<NLTXKeHoachNangLuongBusiness>();
                        var item = NLTXKeHoachNangLuongBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TIEU_DE;
                        }
                        break;
                    }
                #endregion
                #region Quản lý xe
                case LOAITAILIEU.QLXE://Quản lý xe
                    {
                        QlxXeBusiness QlxXeBusiness = Get<QlxXeBusiness>();
                        var item = QlxXeBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.TENXE;
                        }
                        break;
                    }
                #endregion
                #region Quản lý đăng ký phòng họp
                case LOAITAILIEU.QUANLYPHONGHOP://Quản lý phòng họp
                    {
                        PhDangKyPhongHopBusiness PhDangKyPhongHopBusiness = Get<PhDangKyPhongHopBusiness>();
                        var item = PhDangKyPhongHopBusiness.Find(itemID);
                        if (item != null)
                        {
                            model.Title = preTitle + ": " + item.NOIDUNG;
                        }
                        break;
                    }
                #endregion
                default:
                    break;
            }
            model.Content = content;
            model.ItemID = itemID;
            model.ItemType = itemType;
            return PartialView("_CongViecToolBar", model);
        }

        public PartialViewResult GetDetailWorkFlow(int contentType, int vaitroID, int cosoID)
        {
            WfQuytrinhBusiness = Get<WfQuytrinhBusiness>();
            WfBuocchuyentrangthaiBusiness = Get<WfBuocchuyentrangthaiBusiness>();
            DetailWorkFlowModel model = new DetailWorkFlowModel();
            #region Lấy và hiển thị WF_ID cho loại nội dung
            var listWorkFlow = WfQuytrinhBusiness.GetListWorkFlow(cosoID, contentType);
            if (listWorkFlow != null && listWorkFlow.Count > 0)
            {
                model.ListWorkFlow = listWorkFlow;
                if (listWorkFlow.Count == 1)
                {
                    model.ListStepInWF = WfBuocchuyentrangthaiBusiness.GetListStepInWF(listWorkFlow[0].ID);
                }
            }
            #endregion
            model.VaitroID = vaitroID;
            return PartialView("_GetDetailWorkFlow", model);
        }
        [AllowAnonymous]
        [ValidateInput(false)]
        public PartialViewResult GetUserName(int COSO_ID, int DONVI_ID, long USER_ID, string ListRole, bool Is_Multi, string TIEUDE, string SELECTED, string EXISTED)
        {
            TuyChonGuiModel model = new TuyChonGuiModel();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            var lstRole = ListRole.Split(',').ToList();
            var lstSelected = SELECTED.ToListLong(',');
            var lstExisted = EXISTED.ToListLong(',');
            var ListNguoiDung = NguoiDungBs.GetListNguoiDung(COSO_ID, DONVI_ID, lstRole, lstSelected, lstExisted);
            model.IS_MULTI = Is_Multi;
            model.COSO_ID = COSO_ID;
            model.DONVI_ID = DONVI_ID;
            model.USER_ID = USER_ID;
            model.LISTROLE = lstRole;
            model.TIEUDE = TIEUDE;
            model.EXISTED = EXISTED;
            model.lstNguoiDung = ListNguoiDung;
            return PartialView("_ChonNguoiDung", model);
        }

        [AllowAnonymous]
        [ValidateInput(false)]
        public PartialViewResult GetListThuMuc(int COSO_ID, int DONVI_ID, long USER_ID, string TITLE, int ID)
        {
            LuaChonThuMucModel model = new LuaChonThuMucModel();
            ThuMucLuuTruBusiness = Get<ThuMucLuuTruBusiness>();
            var lstThuMuc = ThuMucLuuTruBusiness.GetDataByUser(USER_ID);
            model.COSO_ID = COSO_ID;
            model.DONVI_ID = DONVI_ID;
            model.ListThuMuc = lstThuMuc;
            model.ID = ID;
            model.TITLE = TITLE;
            return PartialView("_ListThuMucResult", model);
        }

        [ValidateInput(false)]
        public JsonResult FolderChecking(string name, int parentID)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //bool isExisted = false;
            ThuMucLuuTruBusiness = Get<ThuMucLuuTruBusiness>();
            if (!string.IsNullOrEmpty(name))
            {
                List<ThuMucLuuTruBO> lstThuMuc = ThuMucLuuTruBusiness.GetThuMucByParent(parentID, user.UserID);
                foreach (var thumuc in lstThuMuc)
                {
                    if (thumuc.TENTHUMUC.ToLower() == name.Trim().ToLower())
                    {
                        return Json("true");
                    }
                }
            }
            return Json("");
        }

        public JsonResult SaveThuMuc(string TENTHUMUC, long PARENT_ID, int NAM)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            ThuMucLuuTruBusiness = Get<ThuMucLuuTruBusiness>();
            THUMUC_LUUTRU PARENT = ThuMucLuuTruBusiness.Find(PARENT_ID);
            if ((PARENT != null && PARENT.DM_NGUOIDUNG_ID == (long)user.UserID) || PARENT_ID == 0)
            {
                THUMUC_LUUTRU THUMUC = new THUMUC_LUUTRU();
                THUMUC.NAM = NAM;
                THUMUC.TENTHUMUC = TENTHUMUC;
                THUMUC.THUMUC_AO = TENTHUMUC;
                THUMUC.PARENT_ID = PARENT_ID;
                DonViBs = Get<DmDonViBusiness>();
                PhongBanBs = Get<DmPhongbanBusiness>();
                CoSoBs = Get<CosoBusiness>();
                DM_PHONGBAN phongban = PhongBanBs.Find(user.PhongBanID);
                DM_DONVI donvi = DonViBs.Find(user.DonViID);
                COSO coso = CoSoBs.Find(user.CoSoID);
                string pathname = coso.TENCOSO;
                FileUltilities file = new FileUltilities();
                List<string> arrFolder = new List<string>();
                if (donvi != null)
                {
                    pathname += "\\" + donvi.TEN_DONVI;
                }
                if (phongban != null)
                {
                    pathname += "\\" + phongban.TENPHONGBAN;
                }
                pathname += "\\" + user.Username + "\\eFile";

                if (THUMUC.PARENT_ID > 0)
                {
                    arrFolder = GetAllParent(THUMUC.PARENT_ID);
                    arrFolder.Reverse();
                    foreach (string s in arrFolder)
                    {
                        pathname += "\\" + s;
                    }
                    file.CreateFolder(URLPath + "\\" + pathname + "\\" + THUMUC.TENTHUMUC.Trim());
                }
                else
                {
                    file.CreateFolder(URLPath + "\\" + pathname + "\\" + THUMUC.TENTHUMUC.Trim());
                }
                THUMUC.DM_NGUOIDUNG_ID = (long)user.UserID;
                THUMUC.IS_DELETE = false;
                THUMUC.NGAYTAO = DateTime.Now;
                THUMUC.IS_APPROVE = false;
                THUMUC.COSO_ID = user.CoSoID;
                THUMUC.DONVI_ID = user.DonViID;
                THUMUC.NEEDREVIEW_BY = (long)user.UserID;
                THUMUC.NEEDREVIEW_DATE = DateTime.Now;
                THUMUC.NGAYTAO = DateTime.Now;
                THUMUC.PHONG_ID = user.PhongBanID;
                THUMUC.IS_PRIVATE = true;
                THUMUC.IS_SHARING = false;
                ThuMucLuuTruBusiness.Save(THUMUC);
                return Json(new { Type = "SUCCESS", Message = "Đã thêm mới thư mục thành công!", ID = THUMUC.ID });
            }
            return Json(new { Type = "ERROR", Message = "Không thể thêm thư mục!" });
        }

        private List<string> GetAllParent(long? ID)
        {
            if (ID > 0)
            {
                ThuMucLuuTruBusiness = Get<ThuMucLuuTruBusiness>();
                THUMUC_LUUTRU thumuc = ThuMucLuuTruBusiness.GetAllParent(ID);
                if (thumuc.PARENT_ID > 0)
                {
                    arrFolder.Add(thumuc.TENTHUMUC);
                    GetAllParent(thumuc.PARENT_ID);
                }
                else
                {
                    arrFolder.Add(thumuc.TENTHUMUC);
                }
            }
            return arrFolder;
        }

        public string GetURLBar(string pID)
        {
            lstPath.Clear();
            int ID = 0;
            int.TryParse(pID, out ID);
            lstPath = this.GetPathURL(ID);
            var allParent = JsonConvert.SerializeObject(lstPath);
            return allParent;
        }
        public List<THUMUC_LUUTRU> GetPathURL(long? ID)
        {
            if (ID > 0)
            {
                ThuMucLuuTruBusiness = Get<ThuMucLuuTruBusiness>();
                THUMUC_LUUTRU thumuc = ThuMucLuuTruBusiness.GetAllParent(ID);
                if (thumuc.PARENT_ID > 0)
                {
                    lstPath.Add(thumuc);
                    GetPathURL(thumuc.PARENT_ID);
                }
                else
                {
                    lstPath.Add(thumuc);
                }
            }
            return lstPath;
        }
        public JsonResult CheckkingFile(long ID)
        {
            var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            TAILIEUDINHKEM taikieu = TaiLieuDinhKemBusiness.Find(ID);
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (taikieu != null)
            {
                if (taikieu.USER_ID != (long)user.UserID)
                {
                    return Json("Khong");
                }
                string path = URLPath + taikieu.DUONGDAN_FILE;
                if (System.IO.File.Exists(path))
                {
                    return Json("Co");
                }
                else
                {
                    return Json("Khong");
                }
            }
            return Json("Khong");
        }
    }
}
