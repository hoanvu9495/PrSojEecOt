using Business.Business;
using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Web.Areas.DMChucNangArea.Models;
using Web.Common;
using Web.Custom;
using Web.FwCore;

namespace Web.Areas.DMChucNangArea.Controllers
{
    /// <summary>
    /// written by: namdv
    /// created date: 08/06/2015
    /// reviewed by: namdv
    /// review date: 10/06/2015
    /// </summary>
    public class DMChucNangController : BaseController
    {
        private DmChucnangBusiness DmChucNangBusiness;
        //private NguoidungChucnangBusiness NguoidungChucnangBusiness;
        private VaitroChucnangBusiness VaitroChucnangBusiness;
        private VaitroThaotacBusiness VaitroThaotacBusiness;

        /// <summary>
        /// Màn hình danh sách chức năng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //if (!Ultilities.IsInActivities(user.ListThaoTac, "/DMChucNangArea/DMChucNang/Index"))
            //{
            //    return RedirectToAction("UnAuthor", "Home", new { area = "" });
            //}
            SessionManager.Remove("ListChucNang");
            SessionManager.Remove("ListChucNangSearch");
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            List<DMChucNangVM> lst = DmChucNangBusiness.GetAllVM();
            ViewData["Search"] = "0";
            DMChucNangIndexViewModel model = new DMChucNangIndexViewModel();
            //Todo: Đưa vào Session hoặc biến static, chỉ khi có thêm chức năng cấp 1 thì mới cập nhật lại
            //List<SelectListItem> lstChucNangCha = DmChucNangBusiness.All.Where(o => o.CHUCNANG_CHA == null).ToList()
            //              .Select(o => new SelectListItem()
            //              {
            //                  Text = o.TEN_CHUCNANG.ToString(),
            //                  Value = o.DM_CHUCNANG_ID.ToString()
            //              }).ToList();
            //model.ListChucNangCha = lstChucNangCha;
            model.ListChucNang = lst.OrderBy(o => o.TRANGTHAI).ToList();
            ViewData["CURRENTCHUCNANGCHA"] = 0;
            ViewData["TENCHUCNANG"] = string.Empty;
            //SessionManager.SetValue("ListChucNang", lst.OrderBy(o => o.TRANGTHAI).ToList());

            return View(model);
        }

        public JsonResult ReloadDataTable()
        {
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            List<DMChucNangVM> lst = DmChucNangBusiness.GetAllVM();
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Kiểm tra mã chức năng đã tồn tại chưa
        /// </summary>
        /// <param name="CHUCNANG_ID"></param>
        /// <param name="MACHUCNANG"></param>
        /// <returns></returns>
        public int FindMa(int? CHUCNANG_ID, string MACHUCNANG)
        {
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            int result = DmChucNangBusiness.FindMa(CHUCNANG_ID, MACHUCNANG);
            return result;
        }
        /// <summary>
        /// Tìm kiếm chức năng theo tên chức năng và chức năng cha
        /// </summary>
        /// <param name="TENCHUCNANG">Tên chứ năng (keyword)</param>
        /// <param name="CHUCNANGCHA">Chức năng cha</param>
        /// <returns></returns>
        public PartialViewResult FindChucNang(string TENCHUCNANG, int? CHUCNANGCHA = 0)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (user.CoSoID == null)
            {
                throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            }
            List<DM_CHUCNANG> ChucNangResult = (List<DM_CHUCNANG>)SessionManager.GetValue("ListChucNang");
            if (ChucNangResult != null)
            {
                string tenchucnang = TENCHUCNANG.Trim().ToLower();
                if (!string.IsNullOrEmpty(tenchucnang))
                {
                    ChucNangResult = ChucNangResult.Where(o => o.TEN_CHUCNANG.ToLower().ConvertToFTS().Contains(tenchucnang.Trim().ToLower().ConvertToFTS())).ToList();
                }
                //if (CHUCNANGCHA > 0)
                //{
                //    ChucNangResult = ChucNangResult.Where(o => o.CHUCNANG_CHA == CHUCNANGCHA || o.DM_CHUCNANG_ID == CHUCNANGCHA).ToList();
                //}
            }
            ViewData["Search"] = "1";
            ViewData["CURRENTCHUCNANGCHA"] = CHUCNANGCHA;
            ViewData["TENCHUCNANG"] = TENCHUCNANG;
            SessionManager.SetValue("ListChucNangSearch", ChucNangResult.OrderBy(o => o.TRANGTHAI).ToList());
            return PartialView("_ChucNangSearchResult");
        }


        /// <summary>
        /// Load lại danh sách chức năng sau khi thêm, sửa, xóa
        /// </summary>
        /// <param name="TENCHUCNANG">Tên chức năng đang tìm kiếm</param>
        /// <param name="CHUCNANGCHA">Chức năng cha đang tìm kiếm</param>
        /// <returns></returns>
        public PartialViewResult ReloadGrid(string TENCHUCNANG, int? CHUCNANGCHA = 0)
        {
            ViewData["Search"] = "0";
            List<DM_CHUCNANG> lst = FillDataToGrid();
            ViewData["CURRENTCHUCNANGCHA"] = CHUCNANGCHA;
            ViewData["TENCHUCNANG"] = TENCHUCNANG;
            SessionManager.SetValue("ListChucNang", lst.OrderBy(o => o.TRANGTHAI).ToList());
            return PartialView("_ChucNangSearchResult");
        }

        /// <summary>
        /// Màn hình thêm mới chức năng
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AddFormChucNang()
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            //if (userInfo.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            DMChucNangCreateViewModel model = new DMChucNangCreateViewModel();
            //List<SelectListItem> lstChucNangCha = DmChucNangBusiness.All.Where(o => o.CHUCNANG_CHA == null).ToList()
            //                          .Select(o => new SelectListItem()
            //                          {
            //                              Text = o.TEN_CHUCNANG.ToString(),
            //                              Value = o.DM_CHUCNANG_ID.ToString()
            //                          }).ToList();
            //model.ListChucNangCha = lstChucNangCha;
            return PartialView("_CreateFunction", model);
        }

        /// <summary>
        /// Màn hình cập nhật chức năng
        /// </summary>
        /// <param name="DM_CHUCNANG_ID">Mã tự tăng của chức năng</param>
        /// <returns></returns>
        public PartialViewResult EditFromChucNang(decimal? DM_CHUCNANG_ID)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
          
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            DMChucNangEditViewModel model = new DMChucNangEditViewModel();

            DM_CHUCNANG ChucNang = DmChucNangBusiness.Find((int)DM_CHUCNANG_ID);
            if (ChucNang != null)
            {
                //List<SelectListItem> lstChucNangCha = DmChucNangBusiness.All.Where(o => o.CHUCNANG_CHA == null).ToList()
                //      .Select(o => new SelectListItem()
                //      {
                //          Text = o.TEN_CHUCNANG.ToString(),
                //          Value = o.DM_CHUCNANG_ID.ToString(),
                //          Selected = (ChucNang.CHUCNANG_CHA.HasValue ? ChucNang.CHUCNANG_CHA == o.DM_CHUCNANG_ID : false)
                //      }).ToList();
                //model.ListChucNangCha = lstChucNangCha;
                model.ChucNang = ChucNang;
            }

            return PartialView("_EditFunction", model);
        }

        /// <summary>
        /// Lưu thông tin cập nhật của chức năng (POST METHOD)
        /// </summary>
        /// <param name="ChucNang">Đối tượng ChucNang</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public JsonResult SaveChucNang(DM_CHUCNANG ChucNang)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //if (user.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            int ThemMoiChucNang = 0;
            if (ChucNang.TT_HIENTHI == 0)
            {
                ChucNang.TT_HIENTHI = null;
            }
            if (ChucNang.DM_CHUCNANG_ID == ThemMoiChucNang)
            {
                ChucNang.NGAYTAO = DateTime.Now;
                ChucNang.NGUOITAO = user.Username;
            }
            else
            {
                ChucNang.NGAYSUA = DateTime.Now;
                ChucNang.NGUOISUA = user.Username;

            }
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            DmChucNangBusiness.Save(ChucNang);
            return Json(true);
        }

        /// <summary>
        /// Xóa chức năng
        /// </summary>
        /// <param name="DM_CHUCNANG_ID">Mã tự tăng của chức năng</param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteChucNang(int? DM_CHUCNANG_ID)
        {
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();

            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (user.CoSoID == null)
            {
                throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            }
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            VaitroThaotacBusiness = Get<VaitroThaotacBusiness>();
            
            if (DM_CHUCNANG_ID > 0)
            {
                //var LstNguoiDungChucNang = NguoidungChucnangBusiness.All.Where(x => x.DM_CHUCNANG_ID == DM_CHUCNANG_ID).ToList();
                //foreach (var chucnang in LstNguoiDungChucNang)
                //{
                //    NguoidungChucnangBusiness.Delete(chucnang.NGUOIDUNG_CHUCNANG_ID);
                //    NguoidungChucnangBusiness.Save();
                //}
                // xóa vai trò chức năng
                var LstVaiTroChucNang = VaitroChucnangBusiness.All.Where(x => x.DM_CHUCNANG_ID == DM_CHUCNANG_ID).ToList();
                var LstVaiTroChucNangIds = LstVaiTroChucNang.Select(x => x.VAITRO_CHUCNANG_ID).ToList();
                var LstVaiTroThaoTac = VaitroThaotacBusiness.All.Where(x => LstVaiTroChucNangIds.Contains(x.VAITRO_CHUCNANG_ID.Value)).ToList();
                foreach (var tmpObj in LstVaiTroThaoTac)
                {
                    VaitroThaotacBusiness.Delete(tmpObj.VAITRO_THAOTAC_ID);
                    VaitroThaotacBusiness.Save();
                }
                foreach (var chucnang in LstVaiTroChucNang)
                {
                    VaitroChucnangBusiness.Delete(chucnang.VAITRO_CHUCNANG_ID);
                    VaitroChucnangBusiness.Save();
                }
                DmChucNangBusiness.Delete(DM_CHUCNANG_ID);
                DmChucNangBusiness.Save();
            }
            return Json(new { Type = "SUCCESS", Message = "Xóa chức năng thành công!" });

        }

        /// <summary>
        /// Gán dữ liệu cho danh sách chức năng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private List<DM_CHUCNANG> FillDataToGrid(string tenchucnang = "", int? chucnangcha = 0)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            DmChucNangBusiness = Get<DmChucnangBusiness>();
            List<DM_CHUCNANG> result = new List<DM_CHUCNANG>();
            result = DmChucNangBusiness.All
                .Where(o => !string.IsNullOrEmpty(tenchucnang) ? o.TEN_CHUCNANG.ToLower().Contains(tenchucnang) : true
                    ).OrderByDescending(x => x.DM_CHUCNANG_ID).ToList();
            return result;
        }

        /// <summary>
        /// xóa ảnh
        /// </summary>
        /// <param name="oldAvatar"></param>
        /// <returns></returns>
        private bool DeleteFile(string oldAvatar)
        {
            var fullPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["HSCBAvatar"] + oldAvatar);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Hàm lưu ảnh
        /// </summary>
        /// <param name="oldAvatar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImage(string oldAvatar = "")
        {
            string imageName = string.Empty;

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["Avatar"];
                if (pic.ContentLength > 0)
                {
                    DeleteFile(oldAvatar);
                    imageName = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                    string folder = System.Configuration.ConfigurationManager.AppSettings["HSCBAvatar"];
                    bool folderExists = Directory.Exists(Server.MapPath(folder));
                    if (!folderExists)
                    {
                        Directory.CreateDirectory(Server.MapPath(folder));
                    }
                    var fileName = imageName;
                    var extent = Path.GetExtension(pic.FileName);
                    var urlPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["HSCBAvatar"] + fileName + extent);
                    imageName = fileName + extent;
                    //imageName = urlPath;
                    var path = urlPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(urlPath);

                    //if (img.Width > 200)
                    //    img.Resize(200, 200);
                    //img.Save(urlPath);
                    //// end resize
                }
            }
            return Json(Convert.ToString(imageName), JsonRequestBehavior.AllowGet);
        }
    }
}
