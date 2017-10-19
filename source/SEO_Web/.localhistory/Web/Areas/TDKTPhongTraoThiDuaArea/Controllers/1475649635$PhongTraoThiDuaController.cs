using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Custom;
using Business.CommonBusiness;

namespace Web.Areas.TDKTPhongTraoThiDuaArea.Controllers
{
    public class PhongTraoThiDuaController : BaseController
    {
        //
        // GET: /TDKTPhongTraoThiDuaArea/PhongTraoThiDua/
        private TDKTPhongTraoThiDuaBusiness TDKTPhongTraoThiDuaBusiness;
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
            upload.UploadCustomFile(filebase, true, FileAllowUpload, URLPath, MaxFileSizeUpload.ToIntOrZero(), null, filename, vbdModel.ID, LOAITAILIEU.VANBANDEN, "Văn bản đến");
        }
    }
}
