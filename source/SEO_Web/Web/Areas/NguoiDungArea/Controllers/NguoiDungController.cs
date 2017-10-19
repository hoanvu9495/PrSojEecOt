using Business.Business;
using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Areas.NguoiDungArea.Models;
using Web.Common;
using Web.Custom;
using System.IO;
using Web.FwCore;
using Microsoft.Office.Interop.Excel;
using Business.CommonHelper;
using Web.FwCore.Factory;

namespace Web.Areas.NguoiDungArea.Controllers
{
    public class NguoiDungController : BaseController
    {
        //
        // GET: /NguoiDungArea/NguoiDung/
        #region
        private DmNguoidungBusiness DmNguoidungBusiness;
        private TinhBusiness TinhBusiness;
        private CCTCThanhPhanBusiness CCTCThanhPhanBusiness;
        private DmVaitroBusiness DmVaitroBusiness;
        private DmChucnangBusiness DmChucnangBusiness;


        private NguoiDungVaiTroBusiness NguoiDungVaiTroBusiness;
        private VaitroChucnangBusiness VaitroChucnangBusiness;
        #endregion
        public ActionResult Index()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //if (!Ultilities.IsInActivities(user.ListThaoTac, "/NguoiDungArea/NguoiDung/Index"))
            //{
            //    return RedirectToAction("UnAuthor", "Home", new { area = "" });
            //}
            TinhBusiness = Get<TinhBusiness>();
            DmVaitroBusiness = Get<DmVaitroBusiness>();

            DmNguoidungBusiness = Get<DmNguoidungBusiness>();

            NguoiDungBO NguoiDungBO = new NguoiDungBO();
            NguoiDungIndexViewModel model = new NguoiDungIndexViewModel();
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            model.TreeDonVi = CCTCThanhPhanBusiness.GetTree((int)user.PhongBanID);
            List<SelectListItem> listPhongBan = new List<SelectListItem>();
            var getChildPhongBan = CCTCThanhPhanBusiness.GetDSChild((int)user.PhongBanID);
            var pageListNguoiDung = DmNguoidungBusiness.GetDSByPhongBanID(model.TreeDonVi.ID, getChildPhongBan);
            model.ListNguoiDung = new PageListResultBO<Business.CommonBusiness.NguoiDungBO>();
            model.ListNguoiDung.ListItem = pageListNguoiDung.ToList();
            model.ListNguoiDung.Count = pageListNguoiDung.TotalItemCount;
            model.ListNguoiDung.TotalPage = pageListNguoiDung.PageCount;


            List<SelectListItem> ListTinhThanh = new List<SelectListItem>();
            List<SelectListItem> ListVaiTro = new List<SelectListItem>();
            List<SelectListItem> ListDonVi = new List<SelectListItem>();
            List<SelectListItem> ListCoSo = new List<SelectListItem>();
            List<SelectListItem> LstCapCoSo = new List<SelectListItem>();
        


            model.ListVaiTro = ListVaiTro;
            model.ListCoSo = ListCoSo;
            model.ListDonVi = ListDonVi;
            model.ListCapCoSo = LstCapCoSo;

            return View(model);
        }
        public JsonResult changeDepartment(int department_id, string nguoidungids)
        {
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            if (string.IsNullOrEmpty(nguoidungids))
            {
                return Json(new { Type = "Error", Message = "Bạn chưa chọn người dùng" });
            }
            else
            {
                if (department_id <= 0)
                {
                    return Json(new { Type = "Error", Message = "Bạn chưa chọn phòng ban cần chuyển" });
                }
                else
                {
                    List<long> LstUserId = nguoidungids.ToListLong(',');
                    List<DM_NGUOIDUNG> LstNguoiDung = DmNguoidungBusiness.All.Where(x => LstUserId.Contains(x.DM_NGUOIDUNG_ID)).ToList();
                    foreach (var nguoidung in LstNguoiDung)
                    {
                        nguoidung.DM_PHONGBAN_ID = department_id;
                        DmNguoidungBusiness.Save(nguoidung);
                    }
                }
            }
            return Json(new { Type = "Success", Message = "ok" });
        }
        /// <summary>
        /// Kiểm trả Email đã tồn tại
        /// </summary>
        /// <param name="EMAIL"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult isEmailExist(string EMAIL, decimal userID = 0)
        {
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            return Json(DmNguoidungBusiness.isEmailExist(EMAIL, userID));
        }
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetVaiTro()
        {
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            List<DM_VAITRO> listVaiTro = new List<DM_VAITRO>();
            var result = DmVaitroBusiness.GetListVaiTro().Select(x => new { x.DM_VAITRO_ID, x.TEN_VAITRO }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


       

        /// <summary>
        /// Search Autocompelete
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PartialViewResult FindSearch(int ID)
        {
            List<NguoiDungBO> NguoiDungResult = (List<NguoiDungBO>)SessionManager.GetValue("ListNguoiDung");
            if (NguoiDungResult != null && NguoiDungResult.Count > 0)
            {
                NguoiDungResult = NguoiDungResult.Where(x => x.DM_NGUOIDUNG_ID == ID).ToList();
            }
            ViewData["Search"] = "1";
            SessionManager.SetValue("ListNguoiDungSearch", NguoiDungResult);
            return PartialView("_NguoiDungSearchResult");
        }
        public PartialViewResult Search(string FTS_INDEX)
        {
            List<NguoiDungBO> NguoiDungResult = (List<NguoiDungBO>)SessionManager.GetValue("ListNguoiDung");
            if (NguoiDungResult != null && NguoiDungResult.Count > 0)
            {
                if (!string.IsNullOrEmpty(FTS_INDEX))
                {
                    NguoiDungResult = NguoiDungResult.Where(x => x.FTS.ToLower().Contains(FTS_INDEX.ToLower().ConvertToFTS())).ToList();
                }
            }
            ViewData["Search"] = "1";
            SessionManager.SetValue("ListNguoiDungSearch", NguoiDungResult);
            return PartialView("_NguoiDungSearchResult");
        }

        [HttpPost]
        public JsonResult GetDSByPhongBan(int id, int page = 1)
        {
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            var getChildPhongBan = CCTCThanhPhanBusiness.GetDSChild(id);
            var listResult = DmNguoidungBusiness.GetDSByPhongBanID(id, getChildPhongBan, 20, page);
            var modelResult = new PageListResultBO<NguoiDungBO>();
            modelResult.ListItem = listResult.ToList();
            modelResult.TotalPage = listResult.PageCount;
            modelResult.Count = listResult.TotalItemCount;
            return Json(modelResult);
        }
        /// <summary>
        /// Tìm kiếm người dùng
        /// </summary>
        /// <param name="TENDANGNHAP"></param>
        /// <param name="COSO_ID"></param>
        /// <param name="DM_VAITRO_ID"></param>
        /// <param name="DM_DONVI_ID"></param>
        /// <param name="CAPCOSO"></param>
        /// <returns></returns>
        public PartialViewResult FindNguoiDung(string FTS = "", int? COSO_ID = 0, int? DM_VAITRO_ID = 0, string DM_DONVI_ID = "", string CAPCOSO = "")
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();

            List<NguoiDungBO> NguoiDungResult = (List<NguoiDungBO>)SessionManager.GetValue("ListNguoiDung");
            if (NguoiDungResult != null)
            {
               
                if (!string.IsNullOrEmpty(FTS.Trim()))
                {
                    NguoiDungResult = NguoiDungResult.Where(o => o.FTS.ToLower().Contains(FTS.Trim().ToLower().ConvertToFTS())).ToList();
                }
                if (COSO_ID.HasValue && COSO_ID.Value > 0)
                {
                    NguoiDungResult = NguoiDungResult.Where(o => o.COSO_ID == COSO_ID).ToList();
                }
                if (!string.IsNullOrEmpty(DM_DONVI_ID))
                {
                    var listDonVi = DM_DONVI_ID.ToListInt(',');
                    if (listDonVi != null && listDonVi.Count > 0)
                    {
                        NguoiDungResult = NguoiDungResult.Where(o => listDonVi.Contains(o.DM_DONVI_ID.Value)).ToList();
                    }
                }
                if (DM_VAITRO_ID.HasValue && DM_VAITRO_ID.Value > 0)
                {
                    NguoiDungResult = NguoiDungResult.Where(x => x.ListVaiTro.Contains(DM_VAITRO_ID.Value)).ToList();
                }

            }

            ViewData["Search"] = "1";
            SessionManager.SetValue("ListNguoiDungSearch", NguoiDungResult.OrderBy(o => o.TRANGTHAI).ToList());
            return PartialView("_NguoiDungSearchResult");
        }
        /// <summary>
        /// Màn hình thêm mới người dùng
        /// </summary>
        /// <returns></returns>
        public PartialViewResult CreateUser(int id)
        {
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            TinhBusiness = Get<TinhBusiness>();

            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            NguoiDungCreateViewModel model = new NguoiDungCreateViewModel();

            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            model.DonVi = CCTCThanhPhanBusiness.Find(id);

            model.ListVaiTro = DmVaitroBusiness.DSVaiTro(null);
            

            ////NẾU LÀ CẤP CAO NHẤP HIỂN THỊ RA TẤT CẢ
            //if (checkLever)
            //{
            //    model.ListCapCoSo = ListMaxCapCoSo(user.CAPCOSO_ID);
            //}
            //else
            //{
            //    model.ListCapCoSo = ListCapCoSo(user.CAPCOSO_ID);
            //}

            return PartialView("_CreateUser", model);
        }

        #region Get list phân cấp đơn vị
        private string genPreChar(int solan)
        {
            var str = new StringBuilder();
            if (solan > 0)
            {
                for (int i = 0; i < solan; i++)
                {
                    str = str.Append("--");
                }
                return str.ToString();
            }
            return string.Empty;
        }

      
      



        #endregion
        /// <summary>
        /// Add form thêm mới đơn vị
        /// </summary>
        /// <returns></returns>



        /// <summary>
        /// Màn hình reset mật khẩu
        /// </summary>
        /// <param name="DM_NGUOIDUNG_ID"></param>
        /// <returns></returns>
        public PartialViewResult ResetFormMK(decimal? DM_NGUOIDUNG_ID)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            int loaitruong = userInfo.RoleID;
            //List<SelectListItem> lstTrangThai = GetStatus();


            NguoiDungBO nguoidung = DmNguoidungBusiness.GetByID((int)DM_NGUOIDUNG_ID);

            //if (nguoidung != null)
            //{
            //    ViewData["lstEditNguoiDung"] = nguoidung;
            //    ViewData["TrangThai"] = lstTrangThai;
            //    ViewData["HieuLuc"] = nguoidung.TRANGTHAI;
            //}

            return PartialView("_ResetPass", nguoidung);
        }

      
        /// <summary>
        /// Màn hình cập nhập người dùng
        /// </summary>
        /// <param name="DM_NGUOIDUNG_ID"></param>
        /// <returns></returns>
        public ActionResult EditUser(decimal? DM_NGUOIDUNG_ID)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            TinhBusiness = Get<TinhBusiness>();
            DmVaitroBusiness = Get<DmVaitroBusiness>();
    
            NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();

            int loaitruong = userInfo.RoleID;
            NguoiDungEditViewModel model = new NguoiDungEditViewModel();
            List<SelectListItem> ListVaiTro = new List<SelectListItem>();
            List<SelectListItem> ListPhongBan = new List<SelectListItem>();


            List<int> ListVaiTroChecked = new List<int>();

            NguoiDungBO nguoidung = DmNguoidungBusiness.GetByID((int)DM_NGUOIDUNG_ID);
            if (nguoidung != null)
            {
                ListVaiTroChecked = NguoiDungVaiTroBusiness.GetListByNguoiDung((int)DM_NGUOIDUNG_ID).Select(x => x.VAITRO_ID.Value).ToList();
                ListVaiTro = DmVaitroBusiness.DSVaiTro(ListVaiTroChecked);
                //NẾU LÀ CẤP CAO NHẤP HIỂN THỊ RA TẤT CẢ
            
                model.ListVaiTro = ListVaiTro;
                model.NGUOIDUNG = nguoidung;

                model.ListVaiTroChecked = ListVaiTroChecked;
                CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
                model.DONVI = CCTCThanhPhanBusiness.Find(nguoidung.DM_PHONGBAN_ID);

                return PartialView("_EditUser", model);
            }
            else
            {
                return RedirectToAction("UnAuthor", "Home", new { area = "" });
            }


        }
        /// <summary>
        /// Xóa người dùng
        /// </summary>
        /// <param name="DM_NGUOIDUNG_ID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNguoiDung(int? DM_NGUOIDUNG_ID)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();

            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            if (DM_NGUOIDUNG_ID > 0)
            {
                DmNguoidungBusiness.Delete(DM_NGUOIDUNG_ID);
                DmNguoidungBusiness.Save();
            }

            return Json(new { Type = "SUCCESS", Message = "Xóa thành công" });

        }
        public JsonResult SaveVaiTroNguoiDung(FormCollection col)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            if (!string.IsNullOrEmpty(col["OptionRole"]) && !string.IsNullOrEmpty(col["NGUOIDUNG_ID"]))
            {
                var DM_NGUOIDUNG_ID = int.Parse(col["NGUOIDUNG_ID"]);
                var OptionRole = int.Parse(col["OptionRole"]);// thiết lập vai trò(OptionRole=0 sử dụng vai trò của người dùng )(OptionRole=1 thiết lập phân quyền riêng )
                if (OptionRole == NguoiDungConstant.USE_ROLE_SETTING)//thiết lập phân quyền riêng
                {
                    var NguoiDung = DmNguoidungBusiness.Find(DM_NGUOIDUNG_ID);
                    if (NguoiDung != null)
                    {
                        // Cập nhập lại thiết lập phân quyền riêng
                        NguoiDung.OptionRole = NguoiDungConstant.USE_ROLE_SETTING;
                        DmNguoidungBusiness.Save(NguoiDung);
                    }
                    //tìm kiếm các NGUOIDUNG_RESULT có NGUOIDUNG_ID = NGUOIDUNG_ID trả về
                    var ListChucNang = DmChucnangBusiness.All.ToList();

                    // xóa hết các NGUOIDUNG_ThaoTac trả về có trong bảng NGUOIDUNG_THAOTAC có vai trò chức năng được tìm thấy theo NGUOIDUNG_CHUCNANG
                  
                    foreach (var item in ListChucNang)
                    {
                        //short? macdinh = null;
                        //Check những thao tác được chọn bên cột trái và cột phải
                        var ALL_VAL_SELECT_TT_LEFT = col["ALL_VAL_SELECT_TT_LEFT_" + item.DM_CHUCNANG_ID];
                        var ALL_VAL_SELECT_TT_RIGHT = col["ALL_VAL_SELECT_TT_RIGHT_" + item.DM_CHUCNANG_ID];
                        var ListThaoTac_Left = ALL_VAL_SELECT_TT_LEFT.ToListInt(',');
                        var ListThaoTac_Right = ALL_VAL_SELECT_TT_RIGHT.ToListInt(',');
                        List<int> ListThaoTac = new List<int>();
                        ListThaoTac.AddRange(ListThaoTac_Left);
                        ListThaoTac.AddRange(ListThaoTac_Right);
                      
                    }


                }
                else
                {
                    var NguoiDung = DmNguoidungBusiness.Find(DM_NGUOIDUNG_ID);
                    if (NguoiDung != null)
                    {
                        // Cập nhập lại thiết lập chọn vai trò mặc định
                        NguoiDung.OptionRole = NguoiDungConstant.USE_ROLE_DEFAULT;
                        DmNguoidungBusiness.Save(NguoiDung);
                    }
                }
            }
            return Json("succ");
        }
 
        /// <summary>
        /// Lưu người dùng
        /// </summary>
        /// <param name="NguoiDungBO"></param>
        /// <returns></returns>        
        public ActionResult SaveNguoiDung(NguoiDungBO NguoiDungBO, List<string> CREATE_VAITRO_ALL, HttpPostedFileBase AVATAR)
        {
            NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();

            //DmDonviQlBusiness = Get<DmDonviQlBusiness>();
            NguoiDungBO.FTS = NguoiDungBO.HOTEN.ConvertToFTS() + " " + NguoiDungBO.TENDANGNHAP;
            if (NguoiDungBO.DM_NGUOIDUNG_ID > 0)
            {
                var user = DmNguoidungBusiness.GetByID((int)NguoiDungBO.DM_NGUOIDUNG_ID);
                if (user != null)
                {
                    NguoiDungBO.PASS_OLD = user.MATKHAU;
                }
                //#region Cập nhập chức vụ của công chức, viên chức ở hồ sơ tương ứng với chức vụ của người dùng
                //var NguoiDungCanBo = HscbNguoiDungCanBoBusiness.GetDataByUserID((int)NguoiDungBO.DM_NGUOIDUNG_ID);
                //if (NguoiDungCanBo != null)
                //{
                //    var HoSo = HscbCongChucVienChucBusiness.Find(NguoiDungCanBo.HOSO_ID);
                //    if (HoSo != null)
                //    {
                //        HoSo.DM_CHUCVU_ID = NguoiDungBO.CHUCVU_ID;
                //        HscbCongChucVienChucBusiness.Save(HoSo);
                //    }
                //}


                //#endregion
            }
            if (CREATE_VAITRO_ALL != null)
            {
                if (CREATE_VAITRO_ALL.Any())
                {
                    var lstVaiTro = CREATE_VAITRO_ALL.Select(x => x.ToIntOrZero()).ToList<int>();
                    if (lstVaiTro != null && lstVaiTro.Count > 0)
                    {
                        NguoiDungBO.ListVaiTro = lstVaiTro;
                    }
                }
            }

            if (AVATAR != null)
            {
                string pathfolder = "Upload\\User\\" + NguoiDungBO.DM_NGUOIDUNG_ID + "\\";
                if (!System.IO.Directory.Exists(pathfolder))
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~"), pathfolder));
                }

                if (!string.IsNullOrEmpty(NguoiDungBO.ANH_DAIDIEN))
                {
                    string pathFile = Path.Combine(Server.MapPath("~"), NguoiDungBO.ANH_DAIDIEN);
                    if (System.IO.File.Exists(pathFile))
                    {
                        System.IO.File.Delete(pathFile);
                    }
                }
                var arrFile = AVATAR.FileName.Split('.');
                string pathSaveLogo = pathfolder + "Avatar." + arrFile[arrFile.Count() - 1];
                string logoPath = Path.Combine(Server.MapPath("~"), pathSaveLogo);
                AVATAR.SaveAs(logoPath);
                NguoiDungBO.ANH_DAIDIEN = pathSaveLogo;
            }

            DmNguoidungBusiness.Save(NguoiDungBO);

            //return Json(true);
            return Redirect("/NguoiDungArea/NguoiDung/Index");
        }
        //[ValidateAntiForgeryToken]
        //public JsonResult EditNguoiDung(NguoiDungBO NguoiDungBO)
        //{
        //    UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();


        //    DmNguoidungBusiness = Get<DmNguoidungBusiness>();
        //    var nguoidung = DmNguoidungBusiness.GetByID((int)NguoiDungBO.DM_NGUOIDUNG_ID);

        //    //DmDonviQlBusiness = Get<DmDonviQlBusiness>();
        //    NguoiDungBO.RoleUser = user.RoleID;
        //    NguoiDungBO.TRANGTHAI = 1;
        //    DmNguoidungBusiness.SaveResetMK(NguoiDungBO);
        //    return Json("Sửa người dùng thành công");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ResetPassword(FormCollection form)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            var resultModel = new JsonResultBO();
            var id = form["DM_NGUOIDUNG_ID"].ToIntOrZero();
            if (id > 0)
            {
                DmNguoidungBusiness = Get<DmNguoidungBusiness>();
                var nguoidung = DmNguoidungBusiness.GetByID(id);
                nguoidung.MATKHAU = form["MATKHAUMOI"];
                DmNguoidungBusiness.Save(nguoidung);
                resultModel.Status = true;
            }
            else
            {
                resultModel.Status = false;
                resultModel.Message = "Người dùng không tồn tại";
            }


            return Json(resultModel);
        }
        public JsonResult ResetPasswordNew(string CurrentPass, string NewPass)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            var resultModel = new JsonResultBO();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            var nguoidung = DmNguoidungBusiness.All.Where(x => x.DM_NGUOIDUNG_ID == user.UserID).FirstOrDefault();
            if(nguoidung.MATKHAU == VtEncodeData.Encode_Data(CurrentPass + nguoidung.MAHOA_MK)){
                nguoidung.MATKHAU = VtEncodeData.Encode_Data(NewPass + nguoidung.MAHOA_MK);
                DmNguoidungBusiness.Save(nguoidung);
                resultModel.Status = true;
            }else{
                resultModel.Status = false;
            }
            return Json(resultModel);
        }

        private List<SelectListItem> GetStatus(int? status = -1)
        {
            List<SelectListItem> lstTrangThai = new List<SelectListItem>();
            lstTrangThai.Add(new SelectListItem() { Value = "1", Text = "Hiệu lực", Selected = (status == 1) });
            lstTrangThai.Add(new SelectListItem() { Value = "0", Text = "Không hiệu lực", Selected = (status == 0) });

            return lstTrangThai;

        }

        /// <summary>
        /// màn hình thiết lập phân quyền 
        /// </summary>
        /// <param name="NGUOIDUNG_ID">mã tự tăng của bảng NGUOIDUNG</param>
        /// <returns></returns>
        public PartialViewResult PhanQuyenNguoiDung(int NGUOIDUNG_ID)
        {
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            PhanQuyenNguoiDungViewModel model = new PhanQuyenNguoiDungViewModel();
            model.ListChucNang = DmChucnangBusiness.GetListChucNang2NguoiDung(NGUOIDUNG_ID);
            model.NguoiDung = DmNguoidungBusiness.Find(NGUOIDUNG_ID);
            return PartialView("_PhanQuyen", model);
        }
      

        public JsonResult TenDangNhapExit(string TenDangNhap, decimal NguoiDungId = 0)
        {
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            string mess = "Tên đăng nhập đã tồn tại";
            var result = new { error = DmNguoidungBusiness.isTenDangNhapExist(TenDangNhap, NguoiDungId), message = mess };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

     

        public ActionResult UploadNguoiDung()
        {
            return View();
        }
        //public JsonResult ReviewExelUser()
        //{
        //    Application app = null;
        //    Workbook workbook = null;
        //    CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
        //    DmChucVuBusiness = Get<DmChucVuBusiness>();
        //    NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();
        //    DmNguoidungBusiness = Get<DmNguoidungBusiness>();
        //    DmVaitroBusiness = Get<DmVaitroBusiness>();
        //    var lstUser = DmNguoidungBusiness.All.ToList();
        //    List<CCTC_THANHPHAN> lstCoCau = CCTCThanhPhanBusiness.All.ToList();
        //    List<DM_CHUCVU> lstChucVu = DmChucVuBusiness.All.ToList();
        //    List<DM_VAITRO> lstVaiTro = DmVaitroBusiness.All.ToList();
        //    int excelProductIndex = 0;
        //    int countError = 0;
        //    try
        //    {
        //        var sentFile = Request.Files[0];
        //        var savePath = Path.Combine(Server.MapPath("~/TemplateExcel/"), sentFile.FileName);
        //        if (System.IO.File.Exists(savePath))
        //            System.IO.File.Delete(savePath);
        //        sentFile.SaveAs(savePath);
        //        app = new Microsoft.Office.Interop.Excel.Application();
        //        workbook = app.Workbooks.Open(savePath);
        //        Worksheet worksheet = workbook.ActiveSheet;
        //        Range range = worksheet.UsedRange;
        //        List<UserImportModel> listUserImport = new List<UserImportModel>();
                
        //        #region Khởi tạo
        //        string logErrorStr = "";
                
        //        // end
        //        for (int row = 4; row <= range.Rows.Count; row++)
        //        {
        //            StringBuilder logError = new StringBuilder();
        //            UserImportModel userModel = new UserImportModel();
        //            //sanphamBO.excelProductIndex = excelProductIndex;
        //            userModel.excIsValidProduct = true;
                    
        //            string username = ((Range)range.Cells[row, 2]).Text;
        //            if (string.IsNullOrEmpty(username))
        //            {
        //                userModel.excIsNullUsername = true;
        //                userModel.excIsValidProduct = false;
        //                logError.Append("Dòng " + row + " tên đăng nhập bị bỏ trống\n");
        //                logErrorStr += logError.ToString();
        //                countError = countError + 1;
        //                continue;
        //            }
                    
        //            DM_NGUOIDUNG nguoidungObj =
        //                lstUser.Where(
        //                    x => x.TENDANGNHAP == username)
        //                    .FirstOrDefault();
        //            if (nguoidungObj != null)
        //            {
        //                userModel.excIsUsernameExistedInDb = true;
        //                userModel.excIsValidProduct = false;
        //                logError.Append("Dòng " + row + " tên đăng nhập đã tồn tại trong csdl\n");
        //                logErrorStr += logError.ToString();
        //                countError = countError + 1;
        //                continue;
        //            }
        //            else
        //            {
        //                userModel.TENDANGNHAP = username;
        //            }
        //            string HoTen = ((Range)range.Cells[row, 3]).Text;
        //            if (string.IsNullOrEmpty(HoTen))
        //            {
        //                userModel.excHoTenEmpty = true;
        //                userModel.excIsValidProduct = false;
        //                logError.Append("Dòng " + row + " họ tên bị bỏ trống\n");
        //                logErrorStr += logError.ToString();
        //                countError = countError + 1;
        //                continue;
        //            }
        //            else
        //            {
        //                userModel.HOTEN = HoTen;
        //            }

        //            string Department = ((Range)range.Cells[row, 4]).Text;
        //            if (!string.IsNullOrEmpty(Department))
        //            {
        //                Department = Department.Trim().ToLower();
        //                var CoCauObj = lstCoCau.Where(x => x.CODE.ToLower() == Department).FirstOrDefault();
        //                if (CoCauObj != null)
        //                {
        //                    userModel.DM_PHONGBAN_ID = CoCauObj.ID;
        //                    userModel.PHONGBAN = CoCauObj.NAME;
        //                }
        //            }
                    
        //            string ChucVu = ((Range)range.Cells[row, 5]).Text;
        //            if (!string.IsNullOrEmpty(ChucVu))
        //            {
        //                ChucVu = ChucVu.ToLower();
        //                var ChucVuObj = lstChucVu.Where(x => x.TEN_CHUCVU.ToLower() == ChucVu).FirstOrDefault();
        //                if (ChucVuObj != null)
        //                {
        //                    userModel.CHUCVU_ID = ChucVuObj.ID;
        //                    userModel.CHUCVU = ChucVuObj.TEN_CHUCVU;
        //                }
        //            }
        //            string SoDienThoai = ((Range)range.Cells[row, 6]).Text;
        //            if (!string.IsNullOrEmpty(SoDienThoai))
        //            {
        //                SoDienThoai = SoDienThoai.ToLower();
        //                userModel.DIENTHOAI = SoDienThoai;
        //            }

        //            string eMail = ((Range)range.Cells[row, 7]).Text;
        //            if (!string.IsNullOrEmpty(eMail))
        //            {
        //                userModel.EMAIL = eMail;
        //            }
        //            string Vaitro = ((Range)range.Cells[row, 8]).Text;
        //            if (!string.IsNullOrEmpty(Vaitro))
        //            {
        //                Vaitro = Vaitro.Trim().ToLower();
        //                var VaiTroObj = lstVaiTro.Where(x => x.MA_VAITRO.ToLower() == Vaitro).FirstOrDefault();
        //                if (VaiTroObj != null)
        //                {
        //                    userModel.VAITRO_ID = VaiTroObj.DM_VAITRO_ID;
        //                    userModel.VAITRO = VaiTroObj.TEN_VAITRO;
        //                }
                        
        //            }
        //            userModel.excelProductIndex = excelProductIndex;
        //            listUserImport.Add(userModel);
        //            excelProductIndex += 1;
        //        #endregion
                   
        //        }
        //        string reviewLogPath = GetPathReviewLogExcel(logErrorStr, "Log after upload");
        //        SessionManager.SetValue("listUserImport", listUserImport);
        //        var jsonResult = Json(new { success = true, list = listUserImport, successCount = (excelProductIndex - countError), failAddCount = countError, logPath = reviewLogPath }, JsonRequestBehavior.AllowGet);
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (workbook != null)
        //            workbook.Close(0);
        //        if (app != null)
        //            app.Quit();
        //        return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        if (workbook != null)
        //            workbook.Close(0);
        //        if (app != null)
        //            app.Quit();
        //    }
        //}
        [HttpPost]
        public JsonResult userMultipleInsert(List<UserImportModel> lstUser)
        {
            try
            {
                DmNguoidungBusiness = Get<DmNguoidungBusiness>();
                NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();
                var listUserImport = (List<UserImportModel>)SessionManager.GetValue("listUserImport");
                foreach (var item in listUserImport)
                {
                    var user = listUserImport.Where(x => x.excelProductIndex == item.excelProductIndex).FirstOrDefault();
                    DM_NGUOIDUNG nguoidung = new DM_NGUOIDUNG();
                    nguoidung.TENDANGNHAP = user.TENDANGNHAP;
                    nguoidung.HOTEN = user.HOTEN;
                    nguoidung.TRANGTHAI = 1;
                    nguoidung.MATKHAU = "vnio2017";
                    nguoidung.MAHOA_MK = MaHoaMatKhau.GenerateRandomString(5);
                    nguoidung.MATKHAU = VtEncodeData.Encode_Data(nguoidung.MATKHAU + nguoidung.MAHOA_MK);
                    nguoidung.DM_PHONGBAN_ID = user.DM_PHONGBAN_ID;
                    nguoidung.CHUCVU_ID = user.CHUCVU_ID;
                    nguoidung.DIENTHOAI = user.DIENTHOAI;
                    nguoidung.EMAIL = user.EMAIL;
                    nguoidung.DM_VAITRO_ID = user.VAITRO_ID;
                    DmNguoidungBusiness.Save(nguoidung);

                    NGUOIDUNG_VAITRO userVaitro = new NGUOIDUNG_VAITRO();
                    userVaitro.NGUOIDUNG_ID = nguoidung.DM_NGUOIDUNG_ID;
                    userVaitro.VAITRO_ID = user.VAITRO_ID;
                    NguoiDungVaiTroBusiness.Save(userVaitro);


                }
                return Json(new { success = true, totalProductAdded = lstUser.Count, totalProductAddedFail = 0, linkLog = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        private string GetPathReviewLogExcel(string errMessage, string name)
        {
            if (!String.IsNullOrEmpty(errMessage))
            {
                string fName = name;
                string logPath = Server.MapPath("~/Content/Export/LogExcelError/" + fName + ".txt");
                if (System.IO.File.Exists(logPath))
                    System.IO.File.Delete(logPath);
                System.IO.FileStream fs = new System.IO.FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(errMessage);
                sw.Flush();
                fs.Close();
                return logPath;
            }
            return null;
        }
        [HttpGet]
        public FileResult DownLoadFileLog(string path, string name)
        {
            var fileByte = System.IO.File.ReadAllBytes(path);
            return File(fileByte, "text/plain", name + ".txt");
        }
    }
    
    public class UserData
    {
        public string DONVI { get; set; }
        public string VAITRO { get; set; }
        public string COSO { get; set; }
        public string CHUCVU { get; set; }
    }
}
