using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Custom;
using Business.CommonBusiness;
using Web.Common;
using Web.Areas.TDKTPhongTraoThiDuaArea.Models;
using System.Web.Configuration;

namespace Web.Areas.TDKTPhongTraoThiDuaArea.Controllers
{
    public class PhongTraoThiDuaController : BaseController
    {
        //
        // GET: /TDKTPhongTraoThiDuaArea/PhongTraoThiDua/
        private TDKTPhongTraoThiDuaBusiness TDKTPhongTraoThiDuaBusiness;
        private TaiLieuDinhKemBusiness TaiLieuDinhKemBusiness;
        private string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
        private string FileAllowUpload = WebConfigurationManager.AppSettings["TDKT_FileAllowUpload"];
        private string MaxFileSizeUpload = WebConfigurationManager.AppSettings["TDKT_MaxSizeUpload"];
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult Save(IEnumerable<HttpPostedFileBase> filebase, string[] filename, FormCollection coll)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            TDKT_PHONGTRAO_THIDUA model = new TDKT_PHONGTRAO_THIDUA();
            model.KE_HOACH_THI_DUA = coll["KE_HOACH_THI_DUA"];
            model.NOI_DUNG = coll["NOI_DUNG"];
            model.NGUOITAO = user.UserID;
            model.NGAYTAO = DateTime.Now;
            TDKTPhongTraoThiDuaBusiness.Save(model);
            UploadFileTool upload = new UploadFileTool();
            upload.UploadCustomFile(filebase, true, FileAllowUpload, URLPath, MaxFileSizeUpload.ToIntOrZero(), null, filename, vbdModel.ID, LOAITAILIEU.ThiDuaKhenThuong, "Thi đua khen thưởng");
            return RedirectToAction("View", new {ID, model.ID});
        }
        public ActionResult View(int ID)
        {
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();

            PhongTraoThiDuaViewModel model = new PhongTraoThiDuaViewModel();
            TDKT_PHONGTRAO_THIDUA PhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.Find(ID);
            model.PhongTraoThiDua = PhongTraoThiDua;
            model.LstTaiLieu = TaiLieuDinhKemBusiness.GetDataByItemID(PhongTraoThiDua.ID, LOAITAILIEU.ThiDuaKhenThuong);
            return View(model);
        }
    }
}
