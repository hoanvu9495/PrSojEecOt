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
using System.Collections.Generic;

namespace Web.Areas.TDKTDanhHieuThiDuaArea.Controllers
{
    public class QuanLyDanhHieuCaNhanController : BaseController
    {
        TdktDanhhieucanhanBusiness TdktDanhhieucanhanBusiness;
        TdktDieukiendanhhieucanhanBusiness TdktDieukiendanhhieucanhanBusiness;
        private TdktDanhHieuCaNhanConditionBusiness TdktDanhHieuCaNhanConditionBusiness;

        public ActionResult Index()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            var DanhHieus = TdktDanhhieucanhanBusiness.All.Where(x => x.COSO_ID == user.CoSoID).ToList();
            ViewData["Search"] = "0";
            SessionManager.SetValue("DanhHieus", DanhHieus);
            return View();
        }
        public PartialViewResult FindDanhHieu(FormCollection coll)
        {
            List<TDKT_DANHHIEUCANHAN> DanhHieus = (List<TDKT_DANHHIEUCANHAN>)SessionManager.GetValue("DanhHieus");
            if(!string.IsNullOrEmpty(coll["YEAR"])){
                int Year = coll["YEAR"].Trim().ToIntOrZero();
                DanhHieus = DanhHieus.Where(x => x.YEAR == Year).ToList();
            }
            if (!string.IsNullOrEmpty(coll["DANHHIEUTHIDUA"]))
            {
                var TenDanhHieu = Ultilities.RemoveSign4VietnameseString(coll["DANHHIEUTHIDUA"].Trim());
                DanhHieus = DanhHieus.Where(x => Ultilities.RemoveSign4VietnameseString(x.DANHHIEUTHIDUA.ToLower()).Contains(TenDanhHieu)).ToList();
            }
            ViewData["Search"] = "1";
            SessionManager.SetValue("DanhHieusSearch", DanhHieus);
            return PartialView("_DanhHieuResult");
        }
        /// <summary>
        /// Khởi tạo form tạo mới danh hiệu cá nhân
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            DanhHieuCaNhanViewModel model = new DanhHieuCaNhanViewModel();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            model.ListDanhHieu = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DANHHIEUTHIDUA,
                    Value = x.ID.ToString()
                }
                ).ToList();
            return View(model);
        }
        /// <summary>
        /// Hàm tạo mới danh hiệu thi đua cá nhân
        /// </summary>
        /// <param name="DataPost"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult CreateDanhHieuCaNhan(FormCollection DataPost)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();

            var data = DataPost;
            int COUNT = DataPost["COUNTMAXCONDITION"].ToIntOrZero();
            string DANHHIEUTHIDUA = DataPost["DANHHIEUTHIDUA"];
            string MOTA = DataPost["MOTA"];

            TDKT_DANHHIEUCANHAN danhhieu = new TDKT_DANHHIEUCANHAN();
            danhhieu.DANHHIEUTHIDUA = DANHHIEUTHIDUA;
            danhhieu.MOTA = MOTA;
            danhhieu.YEAR = DataPost["YEAR"].ToIntOrZero();
            danhhieu.TYLE = DataPost["TYLE"].ToFloatOrZero();
            danhhieu.TYLE_DANHHIEU_ID = DataPost["TYLE_DANHHIEU_ID"].ToIntOrZero();
            danhhieu.TONG_SO_XET_CHON = DataPost["TONG_SO_XET_CHON"].ToIntOrZero();
            danhhieu.PHAN_THUONG = DataPost["PHAN_THUONG"];
            danhhieu.COSO_ID = user.CoSoID;

            if (!string.IsNullOrEmpty(DataPost["YEUCAULANHDAOCUC"]))
            {
                danhhieu.LANHDAOCUCXETDUYET = true;
            }
            else
            {
                danhhieu.LANHDAOCUCXETDUYET = false;
            }
            if (!string.IsNullOrEmpty(DataPost["YEUCAUBENNGOAI"]))
            {
                danhhieu.YEUCAUXETDUYETNGOAICUC = true;
            }
            else
            {
                danhhieu.YEUCAUXETDUYETNGOAICUC = false;
            }
            TdktDanhhieucanhanBusiness.Save(danhhieu);

            for (int i = 0; i < COUNT; i++)
            {
                string tmp_key = "SONAMCONGTAC_" + i.ToString();
                //if (!string.IsNullOrEmpty(DataPost[tmp_key]))
                //{
                int SONAMCONGTAC = DataPost[tmp_key].ToIntOrZero();
                string tmp_key_snn = "SONGAYNGHI_" + i.ToString();
                string tmp_key_skyt = "SOSANGKIEN_" + i.ToString();
                //if (!string.IsNullOrEmpty(DataPost[tmp_key_snn]))
                //{
                int SONGAYNGHI = DataPost[tmp_key_snn].ToIntOrZero();
                int SOSANGKIEN = DataPost[tmp_key_skyt].ToIntOrZero();
                TDKT_DIEUKIENDANHHIEUCANHAN model = new TDKT_DIEUKIENDANHHIEUCANHAN();
                model.DANHHIEUCANHAN_ID = danhhieu.ID;
                model.SONAMCONGTAC = SONAMCONGTAC;
                model.SONGAYNGHI = SONGAYNGHI;
                model.SOSANGKIEN = SOSANGKIEN;
                TdktDieukiendanhhieucanhanBusiness.Save(model);

                string danhhieu_ids = DataPost["DANHHIEU_IDS_" + i.ToString()];
                if (!string.IsNullOrEmpty(danhhieu_ids))
                {
                    var LstDanhHieuId = danhhieu_ids.ToListInt(',');
                    if (!string.IsNullOrEmpty(DataPost["SOLUONGDANHHIEU_" + i.ToString()]))
                    {
                        var LstSoLuong = DataPost["SOLUONGDANHHIEU_" + i.ToString()].ToListInt(',');
                        var numberOfDh = LstDanhHieuId.Count();
                        for (var index = 0; index < numberOfDh; index++)
                        {
                            if (LstDanhHieuId[index] > 0 && LstSoLuong[index] > 0)
                            {
                                string tmp_key_is_lientuc = "DATDANHHIEULIENTUC_" + index.ToString();
                                string tmp_key_is_lienke = "THOIDIEM_LIENKE_" + index.ToString();
                                TDKT_DANH_HIEU_CA_NHAN_CONDITION tmpObj = new TDKT_DANH_HIEU_CA_NHAN_CONDITION();
                                if (!string.IsNullOrEmpty(DataPost[tmp_key_is_lientuc]))
                                {
                                    tmpObj.IS_LIEN_TUC = true;
                                }
                                if (!string.IsNullOrEmpty(DataPost[tmp_key_is_lienke]))
                                {
                                    tmpObj.IS_LIEN_KE = true;
                                }
                                tmpObj.DANH_HIEU_ID = danhhieu.ID;
                                tmpObj.COND_DANH_HIEU_ID = LstDanhHieuId[index];
                                tmpObj.COND_SO_LUONG = LstSoLuong[index];
                                tmpObj.DIEUKIEN_ID = model.ID;
                                TdktDanhHieuCaNhanConditionBusiness.Save(tmpObj);
                            }
                        }
                    }
                }
                //}
                //}
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Khởi tạo form update danh hiệu thi đua cá nhân
        /// </summary>
        /// <param name="DataPost"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult UpdateDanhHieuCaNhan(FormCollection DataPost)
        {
            var test = DataPost;
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();
            TDKT_DANHHIEUCANHAN danhhieuObj = TdktDanhhieucanhanBusiness.Find(DataPost["ID"].ToIntOrZero());
            danhhieuObj.DANHHIEUTHIDUA = DataPost["DANHHIEUTHIDUA"];
            danhhieuObj.MOTA = DataPost["MOTA"];
            TdktDanhhieucanhanBusiness.Save(danhhieuObj);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function view detail danh hieu thi dua khen thuong ca nhan
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ViewDetail(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();

            DanhHieuCaNhanViewModel model = new DanhHieuCaNhanViewModel();
            TDKT_DANHHIEUCANHAN danhhieucanhan = TdktDanhhieucanhanBusiness.Find(ID);
            model.DanhHieuCaNhan = danhhieucanhan;
            model.LstCondDateTime = TdktDieukiendanhhieucanhanBusiness.All.Where(x => x.DANHHIEUCANHAN_ID == ID).ToList();
            model.LstCondDanhHieu = TdktDanhHieuCaNhanConditionBusiness.getConditionDanhHieuCaNhan(ID);
            if (danhhieucanhan.TYLE_DANHHIEU_ID > 0)
            {
                model.DanhHieuTyLe = TdktDanhhieucanhanBusiness.Find(danhhieucanhan.TYLE_DANHHIEU_ID);
            }
            return View(model);
        }
        /// <summary>
        /// Function edit danh hieu thi dua khen thuong ca nhan
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Edit(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();

            DanhHieuCaNhanViewModel model = new DanhHieuCaNhanViewModel();
            TDKT_DANHHIEUCANHAN danhhieucanhan = TdktDanhhieucanhanBusiness.Find(ID);
            model.DanhHieuCaNhan = danhhieucanhan;
            model.LstCondDateTime = TdktDieukiendanhhieucanhanBusiness.All.Where(x => x.DANHHIEUCANHAN_ID == ID).ToList();
            model.LstCondDanhHieu = TdktDanhHieuCaNhanConditionBusiness.getConditionDanhHieuCaNhan(ID);
            model.ListDanhHieu = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DANHHIEUTHIDUA,
                    Value = x.ID.ToString()
                }
                ).ToList();
            return View(model);
        }

        public JsonResult Delete(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDanhhieucanhanBusiness.Delete(ID);
            TdktDanhhieucanhanBusiness.Save();
            return Json(new { Type = "SUCCESS", Message = "ok" });
        }
        public PartialViewResult RenderMenu()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            
            PermissionModel model = new PermissionModel();
            model.HasRoleTaoMoiDanhHieuThiDuaCaNhan = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleTaoMoiDanhHieuThiDuaCaNhan);
            model.HasRoleTaoMoiDanhHieuThiDuaTapThe = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleTaoMoiDanhHieuThiDuaTapThe);
            model.HasRoleDangKyThiDuaTapThe = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleDangKyThiDuaTapThe);
            model.HasRoleDangKyThiDuaCaNhan = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleDangKyThiDuaCaNhan);
            model.HasRoleTrinhLanhDaoDonVi = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleTrinhLanhDaoDonVi);
            model.HasRoleLanhDaoDonVi = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleLanhDaoDonVi);
            // xét duyệt
            model.HasRoleGiaoChuyenVienDonVi = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleGiaoChuyenVienDonVi);
            model.HasRoleThamDinhHoSoDonVi = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleThamDinhHoSoDonVi);
            model.HasRoleCucTruong = Ultilities.IsInActivities(UserInfo.ListThaoTac, TDKTConstant.HasRoleLanhDaoCucPheDuyet);
            
            SessionManager.SetValue("PermissionModel", model);
            return PartialView("_MenuLeft", model);
        }
    }
}
