using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.CommonBusiness;
using Business.Business;
using Web.Common;
using Web.Custom;
using Web.FwCore;
using Model.DBTool;
using Web.Areas.BaiVietArea.Models;


namespace Web.Areas.BaiVietArea.Controllers
{
    public class BaiVietController : BaseController
    {
        //
        // GET: /BaiVietArea/BaiViet/

        #region KhaiBao
        private SpinBaiVietBusiness SpinBaiVietBusiness;
        private SpinBaiVietGroupBusiness SpinBaiVietGroupBusiness;

        #endregion

        public ActionResult Index()
        {
            AssignUserInfo();
            var modelresult = new IndexBaiVietVM();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            SessionManager.SetValue("SearchBaiViet", null);
            modelresult.ListBaiViet = GetData();
            return View(modelresult);
        }



        [HttpPost]
        public JsonResult reloadPage(int page)
        {
            return Json(GetData(page));
        }

        public PageListResultBO<BaiVietBO> GetData(int pageindex = 1)
        {
            AssignUserInfo();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var searchModel = SessionManager.GetValue("SearchBaiViet") as SearchBaiVietBO;

            var ListBaiViet = SpinBaiVietBusiness.GetByUser(searchModel, currentUserId, pageindex);
            return ListBaiViet;
        }

        public PartialViewResult Create()
        {
            return PartialView("_ThemMoiBaiVietPartial");
        }

        public PartialViewResult CauHinhSpin(int id)
        {
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            var model = SpinBaiVietGroupBusiness.GetTuDienSpin(id);
            return PartialView("_QuanLySpinPartial", model);
        }
        public PartialViewResult Edit(int id)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var model = SpinBaiVietBusiness.Find(id);
            return PartialView("_CapNhatBaiVietPartial", model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Insert(FormCollection form)
        {
            var resultModel = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            AssignUserInfo();
            var bv = new SPIN_BAIVIET();
            bv.TIEUDE = form["TIEUDEBaiViet"];
            bv.NOIDUNG = form["NOIDUNGBaiViet"];
            bv.NGAYTAO = DateTime.Now;
            bv.NGUOITAO = currentUserId;
            bv.NGAYSUA = DateTime.Now;
            bv.NGUOISUA = currentUserId;
            bv.IS_ORIGIN = true;
            bv.USER_ID = currentUserId;
            SpinBaiVietBusiness.Save(bv);
            return Json(resultModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditBaiViet(FormCollection form)
        {
            var resultModel = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            AssignUserInfo();
            SPIN_BAIVIET bv = null;
            var id = form["editID"].ToIntOrZero();
            if (id > 0)
            {
                bv = SpinBaiVietBusiness.Find(id);
            }
            if (bv == null)
            {
                bv = new SPIN_BAIVIET();
                bv.NGAYTAO = DateTime.Now;
                bv.NGUOITAO = currentUserId;
            }

            bv.TIEUDE = form["TIEUDEBaiViet"];
            bv.NOIDUNG = form["NOIDUNGBaiViet"];

            bv.NGAYSUA = DateTime.Now;
            bv.NGUOISUA = currentUserId;
            bv.IS_ORIGIN = true;
            bv.USER_ID = currentUserId;
            SpinBaiVietBusiness.Save(bv);
            return Json(resultModel);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            if (bv != null)
            {
                var listExtend = SpinBaiVietBusiness.GetListExtend(id);
                if (listExtend.Any())
                {
                    SpinBaiVietBusiness.DeleteAll(listExtend);
                    SpinBaiVietBusiness.Save();
                }
                SpinBaiVietBusiness.Delete(id);
                SpinBaiVietBusiness.Save();
            }
            else
            {
                model.Status = false;
                model.Message = "Không tìm thấy bài viết";
            }
            return Json(model);
        }

        public ActionResult Detail(int id)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            var model = new DetailBaiVietVM();
            model.BaiViet = SpinBaiVietBusiness.GetByID(id);
            return View(model);

        }

        [HttpPost]
        public JsonResult SearchBaiViet(FormCollection form)
        {
            var searchModel = new SearchBaiVietBO();
            searchModel.TieuDe = form["TIEUDE_SEARCH"];
            searchModel.StartNgayTao = form["StartDate_Search"].ToDataTime();
            searchModel.EndNgayTao = form["EndtDate_Search"].ToDataTime();
            SessionManager.SetValue("SearchBaiViet", searchModel);
            return Json(GetData());
        }
    }
}
