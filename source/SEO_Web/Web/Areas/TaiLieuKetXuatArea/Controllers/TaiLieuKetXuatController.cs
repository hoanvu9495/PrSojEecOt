using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DBTool;
using Business.CommonBusiness;
using Business.Business;
using Web.Common;
using Web.Custom;
using Web.FwCore;
using Business.CommonConstant;
using System.Web.Configuration;
using System.IO;
using FileIo = System.IO.File;
using Novacode;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Drawing.Imaging;
using System.Xml.Linq;
using Web.Areas.TaiLieuKetXuatArea.Models;
namespace Web.Areas.TaiLieuKetXuatArea.Controllers
{
    public class TaiLieuKetXuatController : BaseController
    {
        //
        // GET: /TaiLieuKetXuatArea/TaiLieuKetXuat/
        #region KhaiBao
        private TblTaiLieuKetXuatBusiness TblTaiLieuKetXuatBusiness;
        
        private TblConfigTaiLieuBusiness TblConfigTaiLieuBusiness;
        private string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
        private string HOST_WEB_UPLOAD = WebConfigurationManager.AppSettings["HOST_WEB_UPLOAD"];
        #endregion
        public ActionResult Index()
        {
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            var listData = TblTaiLieuKetXuatBusiness.GetList();
            foreach (var item in listData)
            {
                var pathDown = Path.Combine(URLPath, item.URL);
                if (System.IO.File.Exists(pathDown))
                {
                    item.PathDownload = Path.Combine(HOST_WEB_UPLOAD, item.URL);
                }
            }
            return View(listData);
        }



        [HttpPost]
        public JsonResult CheckMa(string maTaiLieu, int id = 0)
        {
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            return Json(TblTaiLieuKetXuatBusiness.CheckMaTaiLieu(maTaiLieu, id));
        }
        [HttpPost]
        public JsonResult reloadPage()
        {
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            var listData = TblTaiLieuKetXuatBusiness.GetList();
            foreach (var item in listData)
            {
                var pathDown = Path.Combine(URLPath, item.URL);
                if (System.IO.File.Exists(pathDown))
                {
                    item.PathDownload = Path.Combine(HOST_WEB_UPLOAD, item.URL);
                }
            }
            return Json(listData);
        }
        public PartialViewResult ThemMoi()
        {
            TblTrangThaiKetXuatBusiness = Get<TblTrangThaiKetXuatBusiness>();
            var dsTrangThaiKetXuat = TblTrangThaiKetXuatBusiness.GetDs(null);
            return PartialView("_ThemMoiTaiLieuPartial", dsTrangThaiKetXuat);

        }

        [ValidateAntiForgeryToken]
        public JsonResult InsertTaiLieu(HttpPostedFileBase File, FormCollection form)
        {
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            TblDieuKienKetXuatBusiness = Get<TblDieuKienKetXuatBusiness>();
            var result = new JsonResultBO();
            AssignUserInfo();
            var tailieu = new TBL_TAILIEU_KETXUAT();
            tailieu.TENTAILIEU = form["TENTAILIEU"];
            tailieu.MA_TAILIEU = form["MATAILIEU"];


            tailieu.SOTAILIEU = 0;
            tailieu.NGUOITAO = (int)currentUserId;
            tailieu.NGAYTAO = DateTime.Now;
            result = FileUpload.SaveFile(File, null, ".docx", null, FolderFileConst.TAILIEU_KETXUAT, URLPath);
            if (result.Status)
            {
                tailieu.URL = result.Message;
                TblTaiLieuKetXuatBusiness.Save(tailieu);
                var lstTrangThai = form["TRANGTHAIKETXUAT"].ToListInt(',');
                foreach (var item in lstTrangThai)
                {
                    var tt = new TBL_DIEUKIEN_KETXUAT();
                    tt.ID_TAILIEU = tailieu.ID;
                    tt.ID_TRANGTHAI = item;
                    TblDieuKienKetXuatBusiness.Save(tt);
                }
            }

            return Json(result);
        }

        [ValidateAntiForgeryToken]
        public JsonResult EditTaiLieu(HttpPostedFileBase File, FormCollection form)
        {
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            TblDieuKienKetXuatBusiness = Get<TblDieuKienKetXuatBusiness>();
            var result = new JsonResultBO();
            AssignUserInfo();
            var id = form["ID"].ToIntOrZero();
            if (id > 0)
            {
                var tailieu = TblTaiLieuKetXuatBusiness.Find(id);
                tailieu.TENTAILIEU = form["TENTAILIEU"];
                tailieu.MA_TAILIEU = form["MATAILIEU"];
                var lstTrangThai = form["TRANGTHAIKETXUAT"].ToListInt(',');
                var lstDBStatus = TblDieuKienKetXuatBusiness.GetByTaiLieuID(id);
                TblDieuKienKetXuatBusiness.DeleteAll(lstDBStatus);
                TblDieuKienKetXuatBusiness.Save();
                foreach (var item in lstTrangThai)
                {
                    var tt = new TBL_DIEUKIEN_KETXUAT();
                    tt.ID_TAILIEU = tailieu.ID;
                    tt.ID_TRANGTHAI = item;
                    TblDieuKienKetXuatBusiness.Save(tt);
                }
                tailieu.NGUOISUA = (int)currentUserId;
                tailieu.NGAYSUA = DateTime.Now;
                if (File != null)
                {
                    result = FileUpload.SaveFile(File, null, ".docx", null, FolderFileConst.TAILIEU_KETXUAT, URLPath);
                    if (result.Status)
                    {
                        tailieu.URL = result.Message;
                    }
                }
                TblTaiLieuKetXuatBusiness.Save(tailieu);
                result.Status = true;

            }
            else
            {
                result.Status = false;
                result.Message = "Không tìm thấy thông tin cập nhật";
            }

            return Json(result);
        }



        [HttpGet]
        public ActionResult ConfigTaiLieu(int id)
        {
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            var model = new ConfigTaiLieuVM();
            var TaiLieuDetail = TblTaiLieuKetXuatBusiness.Find(id);
            if (TaiLieuDetail != null)
            {
                model.TaiLieu = TaiLieuDetail;
                string template_path = Path.Combine(URLPath, TaiLieuDetail.URL);
                model.HtmlString = DocxProvider.GetHTMLString(template_path, TaiLieuDetail.TENTAILIEU);
                return View(model);

            }
            else
            {
                return RedirectToAction("NotFound404", "Home", new { @area = "" });
            }


        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = new JsonResultBO();
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            var tailieu = TblTaiLieuKetXuatBusiness.Find(id);
            if (tailieu != null)
            {
                tailieu.IS_DELETE = true;
                TblTaiLieuKetXuatBusiness.Save(tailieu);
                result.Status = true;
            }
            else
            {
                result.Status = false;
                result.Message = "Không tìm thấy thông tin";
            }

            return Json(result);


        }
        public PartialViewResult Edit(int id)
        {
            var model = new EditTailieuVM();
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            model.TaiLieu = TblTaiLieuKetXuatBusiness.Find(id);
            TblTrangThaiKetXuatBusiness = Get<TblTrangThaiKetXuatBusiness>();
            TblDieuKienKetXuatBusiness = Get<TblDieuKienKetXuatBusiness>();
            var listStatus = TblDieuKienKetXuatBusiness.GetByTaiLieuID(id);
            model.DsTrangThai = TblTrangThaiKetXuatBusiness.GetDs(listStatus);
            return PartialView("_CapNhatTaiLieuPartial", model);
        }

        [HttpPost]
        public PartialViewResult settingfield(string name, int tailieuid)
        {
            var configmodel = new ConfigFieldVM();
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            configmodel.Tailieu = TblTaiLieuKetXuatBusiness.Find(tailieuid);
            TblConfigTaiLieuBusiness = Get<TblConfigTaiLieuBusiness>();
            configmodel.ConfigField = TblConfigTaiLieuBusiness.GetByKey(name, tailieuid);
            if (configmodel.ConfigField == null)
            {
                configmodel.ConfigField = new TBL_CONFIG_TAILIEU();
                configmodel.ConfigField.FIELD_KEY = name;
            }
            var type = typeof(DataExportTHUEBO).GetProperties();
            var DSdataColum = type.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name,
            }).ToList();
            configmodel.DsColumn = DSdataColum;
            return PartialView("_settingFieldPartial", configmodel);
        }

        [HttpPost]
        public JsonResult savesettingfield(TBL_CONFIG_TAILIEU configTaiLieu)
        {
            TblConfigTaiLieuBusiness = Get<TblConfigTaiLieuBusiness>();
            if (configTaiLieu.ID > 0)
            {
                var dbConfig = TblConfigTaiLieuBusiness.Find(configTaiLieu.ID);
                dbConfig.COLUM_MIX = configTaiLieu.COLUM_MIX;
                TblConfigTaiLieuBusiness.Save(dbConfig);
            }
            else
            {
                TblConfigTaiLieuBusiness.Save(configTaiLieu);
            }
            var modelresult = new JsonResultBO();
            modelresult.Status = true;
            return Json(modelresult);
        }
        [HttpPost]
        public JsonResult KetXuat(int idDoanhNghiep, int idDot, List<string> arrTaiLieu)
        {
            var model = new JsonResultBO(true);
            var user = SessionManager.GetUserInfo() as UserInfoBO;
            TblDataKetXuatBusiness = Get<TblDataKetXuatBusiness>();
            TblTaiLieuKetXuatBusiness = Get<TblTaiLieuKetXuatBusiness>();
            TblSoKetXuatBusiness = Get<TblSoKetXuatBusiness>();
            TblDoanhNghiepBusiness = Get<TblDoanhNghiepBusiness>();
            var doanhnghiep = TblDoanhNghiepBusiness.GetVMByID(idDoanhNghiep);
            var dataexport = TblDataKetXuatBusiness.GetDataExport(doanhnghiep, idDot);
            foreach (var item in arrTaiLieu)
            {
                var tailieu = TblTaiLieuKetXuatBusiness.GetByMa(item);
                if (tailieu != null)
                {
                    dataexport.SoTaiLieu = TblTaiLieuKetXuatBusiness.GetSoTaiLieu(tailieu.ID);
                    var result = DocxProvider.ExportToWord(tailieu.ID, dataexport, tailieu.TENTAILIEU + '-' + doanhnghiep.MASOTHUE + ".docx", user.Fullname);
                    var sotailieu = TblTaiLieuKetXuatBusiness.GetSoTaiLieu(tailieu.ID);
                    if (result.Status)
                    {
                        TblTaiLieuKetXuatBusiness.TangSoTaiLieu(tailieu.ID);
                        var kx = new TBL_SOKETXUAT();
                        kx.MATAILIEU = item;
                        kx.ID_TAILIEU = tailieu.ID;
                        kx.ID_DOTNOP = idDot;
                        kx.ID_DOANHNGHIEP = doanhnghiep.ID;
                        kx.SOTTT = sotailieu;
                        kx.NGAYTAO = DateTime.Now;
                        kx.NGUOITAO = (int)user.UserID;
                        kx.URL = result.Message;
                        TblSoKetXuatBusiness.Save(kx);
                    }
                  
                }

            }



            return Json(model);

        }
    }


}
