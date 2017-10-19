using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Business.Business;
using Business.CommonBusiness;
using Model.eAita;
using Web.Common;
using Web.Custom;
using Web.FwCore;

namespace Web.Areas.DoanVaoDoanRaArea.Controllers
{
    public class HoChieuController : BaseController
    {
        //
        // GET: /DoanVaoDoanRaArea/HoChieu/

        private Htqt_HoChieuBusiness hoChieuBusiness;
        protected UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
        private HscbFilesBusiness HscbFilesBusiness;
        protected TaiLieuDinhKemBusiness taiLieuDinhKemBusiness;
        private string EXTENSION = WebConfigurationManager.AppSettings["VANBANDI_FileAllowUpload"];
        private string path = WebConfigurationManager.AppSettings["FileUpload"];
        private string MaxSize = WebConfigurationManager.AppSettings["VANBANDI_MaxSizeUpload"];
        UploadFileTool tool = new UploadFileTool();


        #region Execute
        public ActionResult Index()
        {
            ViewData["Search"] = "0";
            SessionManager.SetValue("ListHoChieu", FillAllRecord());
            return View();
        }
        public List<Htqt_HoChieuBO> FillAllRecord()
        {
            hoChieuBusiness = Get<Htqt_HoChieuBusiness>();
            return hoChieuBusiness.GetListAllHoChieu();
        }
       // [HttpPost]
        public PartialViewResult FindHoChieu(FormCollection collection)
        {
            hoChieuBusiness = Get<Htqt_HoChieuBusiness>();
            List<Htqt_HoChieuBO> result = (List<Htqt_HoChieuBO>)SessionManager.GetValue("ListHoChieu");
            string TENCANBO = collection["TENCANBO"];
            if (!string.IsNullOrEmpty(collection["TENCANBO_SEARCH_LEFT"]))
            {
                result = result.Where(h => h.SOHOCHIEU.ToLower().Trim().Contains(collection["TENCANBO_SEARCH_LEFT"].ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(collection["SOHOCHIEU"]))
            {
                result = result.Where(h => h.SOHOCHIEU.ToLower().Trim().Contains(collection["SOHOCHIEU"].ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(TENCANBO))
            {
                result = result.Where(h => h.HOTEN.ConvertToVN().ToLower().Trim().Contains(TENCANBO.ConvertToVN().ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty((collection["fromdate_NGAYBATDAU"])))
            {
                var from_NGAYBATDAU = collection["fromdate_NGAYBATDAU"].ToDateTime();
                result = result.Where(h => h.NGAYCAP >= from_NGAYBATDAU).ToList();
            }
            if (!string.IsNullOrEmpty((collection["todate_NGAYBATDAU"])))
            {
                var to_NGAYBATDAU = collection["todate_NGAYBATDAU"].ToDateTime();
                result = result.Where(h => h.NGAYCAP <= to_NGAYBATDAU).ToList();
            }
            if (!string.IsNullOrEmpty((collection["fromdate_NGAYKETTHUC"])))
            {
                var from_NGAYKETTHUC = collection["fromdate_NGAYKETTHUC"].ToDateTime();
                result = result.Where(h => h.NGAYHETHAN >= from_NGAYKETTHUC).ToList();
            }
            if (!string.IsNullOrEmpty((collection["todate_NGAYKETTHUC"])))
            {
                var to_NGAYKETTHUC = collection["todate_NGAYKETTHUC"].ToDateTime();
                result = result.Where(h => h.NGAYHETHAN <= to_NGAYKETTHUC).ToList();
            }

            ViewData["Search"] = "1";
            SessionManager.SetValue("ListHoChieuSearch", result);
            return PartialView("_HoChieuSearchResult");
        }
        public ActionResult ThemHoChieu()
        {
            return View("_ThemHoChieu");
        }

        public ActionResult CapNhatHoChieu(int ID)
        {
            hoChieuBusiness = Get<Htqt_HoChieuBusiness>();
            var ListJobAvailbale = hoChieuBusiness.GetListJobAvailable(userInfo.Username);
            if (!ListJobAvailbale.Contains(ID))
            {
                return RedirectToAction("UnAuthor", "Home", new { area = "" });
            }
            Htqt_HoChieuBO result;
            result = hoChieuBusiness.GetHoSoIDCanBo(ID) != "" ? hoChieuBusiness.GetDetailHoChieu(ID) : hoChieuBusiness.GetcChiTietHoChieu(ID);
            if (result!=null)
            {
                HscbFilesBusiness = Get<HscbFilesBusiness>();
                var files = HscbFilesBusiness.GetListFile(ID, LOAITAILIEU.HOCHIEU);
                result.ListFile = files;
            }
            return View("_CapNhatHoChieu", result);
        }

        public ActionResult ChiTietHoChieu(int ID)
        {
            hoChieuBusiness = Get<Htqt_HoChieuBusiness>();
            var ListJobAvailbale = hoChieuBusiness.GetListJobAvailable(userInfo.Username);
            if (!ListJobAvailbale.Contains(ID))
            {
                return RedirectToAction("UnAuthor", "Home", new { area = "" });
            }
            Htqt_HoChieuBO result;
            ViewData["CheckHoChieu"] = hoChieuBusiness.GetHoSoIDCanBo(ID);
            result = hoChieuBusiness.GetHoSoIDCanBo(ID) != "" ? hoChieuBusiness.GetDetailHoChieu(ID) : hoChieuBusiness.GetcChiTietHoChieu(ID);

            if (result != null)
            {
                HscbFilesBusiness = Get<HscbFilesBusiness>();
                var files = HscbFilesBusiness.GetListFile(ID, LOAITAILIEU.HOCHIEU);
                result.ListFile = files;
            }
            return View("_ChiTietHoChieu", result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveHoChieu(HTQT_HOCHIEU HoChieu, FormCollection collection, IEnumerable<HttpPostedFileBase> filebase, string[] filename)
        {
            FileUpload file = new FileUpload();
            hoChieuBusiness = Get<Htqt_HoChieuBusiness>();

            var Folder_File = new string[filename.Count()];
            int size = filebase.Count();
            for (int i = 0; i < size; i++)
            {
                Folder_File[i] = "0";
            }


            if (!string.IsNullOrEmpty(collection["HOCHIEU_ID"]))
            {
                if (int.Parse(collection["HOCHIEU_ID"]) > 0)
                {
                    var result = hoChieuBusiness.Find(int.Parse(collection["HOCHIEU_ID"]));
                    result.SOHOCHIEU = HoChieu.SOHOCHIEU.Trim();
                    result.USER_ID = int.Parse(collection["USER_ID_HIDDEN"]);
                    result.LOAIHOCHIEU = HoChieu.LOAIHOCHIEU;
                    result.NGAYCAP = collection["_NGAYCAP"].ToDataTime();
                    result.NGAYHETHAN = collection["_NGAYHETHAN"].ToDataTime();
                    result.NGAYTAO = collection["NGAYTAO"].ToDataTime();
                    result.NGUOITAO = collection["NGUOITAO"];
                    result.NGAYSUA = DateTime.Now;
                    result.NGUOISUA = userInfo.Username;
                    result.HAS_FILE = tool.UploadCustomFile(filebase, true, EXTENSION, path, int.Parse(MaxSize), Folder_File, filename, int.Parse(collection["HOCHIEU_ID"]), LOAITAILIEU.HOCHIEU, "HỘ CHIẾU");
                    file.SaveFiles(filebase, collection["HOCHIEU_ID"].ToIntOrZero(), collection["HSCB_PATH"], (long)userInfo.UserID, LOAITAILIEU.HOCHIEU, QLFileUploadConstant.FILES_CONG_CHUC, collection["ValidFileExtensions"], long.Parse(collection["HSCB_MAXSIZE"]), filename);
                    hoChieuBusiness.Save(result);
                }
            }
            else
            {
                HoChieu.SOHOCHIEU = HoChieu.SOHOCHIEU.Trim();
                HoChieu.USER_ID = int.Parse(collection["USER_ID_HIDDEN"]);
                HoChieu.NGAYCAP = collection["_NGAYCAP"].ToDataTime();
                HoChieu.NGAYHETHAN = collection["_NGAYHETHAN"].ToDataTime();
                HoChieu.NGAYTAO = DateTime.Now;
                HoChieu.NGUOITAO = userInfo.Username;
                HoChieu.HAS_FILE = tool.UploadCustomFile(filebase, true, EXTENSION, path, int.Parse(MaxSize), Folder_File, filename, HoChieu.ID, LOAITAILIEU.HOCHIEU, "HỘ CHIẾU");
                hoChieuBusiness.Save(HoChieu);
                file.SaveFiles(filebase, HoChieu.ID, collection["HSCB_PATH"], (long)userInfo.UserID, LOAITAILIEU.HOCHIEU, QLFileUploadConstant.FILES_CONG_CHUC, collection["ValidFileExtensions"], long.Parse(collection["HSCB_MAXSIZE"]), filename);
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public JsonResult DeleteHoChieu(int? ID)
        {
            if (ID > 0)
            {
                //thực hiện xóa thông tin hộ chiếu
                hoChieuBusiness = Get<Htqt_HoChieuBusiness>();
                hoChieuBusiness.Delete(ID);
                hoChieuBusiness.Save();
            }
            return Json(true);
        }

        public PartialViewResult ReloadGrid(string SOHOCHIEU = "")
        {
            ViewData["Search"] = "0";
            ViewData["HoChieu"] = SOHOCHIEU;
            SessionManager.SetValue("ListHoChieu", FillAllRecord());
            return PartialView("_HoChieuSearchResult");
        }
        public String validateSoHoChieu(string SO_HO_CHIEU, int? HOCHIEU_ID)
        {
            List<Htqt_HoChieuBO> result = (List<Htqt_HoChieuBO>)SessionManager.GetValue("ListHoChieu");
            
            if (result != null)
            {
                if (HOCHIEU_ID != null)
                {
                    if (HOCHIEU_ID > 0)
                    {
                        if (!string.IsNullOrEmpty(SO_HO_CHIEU))
                        {
                            result = result.Where(d => d.SOHOCHIEU.ToLower().Trim() == (SO_HO_CHIEU.ToLower().Trim()) && d.ID != HOCHIEU_ID).ToList();
                        }
                        if (result.Count > 0)
                        {
                            return "existed";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(SO_HO_CHIEU))
                    {
                        result = result.Where(x => x.SOHOCHIEU.ToLower().Trim() == SO_HO_CHIEU.ToLower().Trim()).ToList();
                        if (result.Count > 0)
                        {
                            return "existed";
                        }
                    }
                }
            }
            return "not exist";
        }

        public String validateTenCanBo(long? USER_ID, int? HOCHIEU_ID)
        {
            List<Htqt_HoChieuBO> result = (List<Htqt_HoChieuBO>)SessionManager.GetValue("ListHoChieu");

            if (result != null)
            {
                if (HOCHIEU_ID != null)
                {
                    if (HOCHIEU_ID > 0)
                    {
                        if (USER_ID != null)
                        {
                            result = result.Where(d => d.USER_ID==USER_ID && d.ID != HOCHIEU_ID).ToList();
                        }
                        if (result.Count > 0)
                        {
                            return "existed";
                        }
                    }
                }
                else
                {
                    if (USER_ID != null)
                    {
                        result = result.Where(d => d.USER_ID == USER_ID).ToList();
                        if (result.Count > 0)
                        {
                            return "existed";
                        }
                    }
                }
            }
            return "not exist";
        }
        public ActionResult MenuLeft()
        {
            return PartialView("_MenuLeft");
        }
        #endregion Execute
    }
}
