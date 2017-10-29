using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Web.FwCore;
using Model.DBTool;
using Web.Common;
using Web.Custom;
using Business.CommonBusiness;
using Business.Business;
using Web.Areas.FacebookArea.Models;

namespace Web.Areas.FacebookArea.Controllers
{
    public class facebookController : BaseController
    {
        //
        // GET: /FacebookArea/facebook/
        #region KhaiBao
        private FbChuKyBusiness FbChuKyBusiness;
        private FbPostBusiness FbPostBusiness;
        #endregion
        public ActionResult Index()
        {
            FbPostBusiness = Get<FbPostBusiness>();
            SessionManager.SetValue("SearchModelBaiVietFB", null);
            var query = GetData();
            return View(query);
        }

        public PageListResultBO<FB_POST> GetData(int pageindex = 1)
        {
            AssignUserInfo();
            FbPostBusiness = Get<FbPostBusiness>();
            var searchModel = SessionManager.GetValue("SearchModelBaiVietFB") as SearchBaiVietBO;
            var model = FbPostBusiness.GetListByUser(searchModel, currentUserId);
            return model;
        }
        [HttpPost]
        public JsonResult reloadPage(int page)
        {
            return Json(GetData(page));
        }

        [HttpGet]
        public PartialViewResult KeThuaBaiViet(int vt)
        {
            FbPostBusiness = Get<FbPostBusiness>();
            var model = new KeThuaBaiVietVM();
            AssignUserInfo();
            model.LstPost = FbPostBusiness.All.Where(x => x.NGUOITAO == currentUserId).ToList();
            model.ViTri = vt;
            return PartialView("_ChonBaiVietPartial", model);
        }

        [HttpGet]
        public PartialViewResult KeThuaHashTag()
        {
            FbPostBusiness = Get<FbPostBusiness>();
            AssignUserInfo();
            var model = FbPostBusiness.All.Where(x => x.NGUOITAO == currentUserId ).ToList();
            model = model.Where(x => x.TUKHOA!="" && x.TUKHOA!=null).ToList();
            return PartialView("_KeThuaHashTagPartial", model);
        }
        [HttpPost]
        public JsonResult SearchFB(FormCollection form)
        {
            var searchModel = new SearchBaiVietBO();
            searchModel.TieuDe = form["TIEUDE_SEARCH"];
            searchModel.StartNgayTao = form["StartDate_Search"].ToDataTime().ToStartDay();
            searchModel.EndNgayTao = form["EndtDate_Search"].ToDataTime().ToEndDay();
            SessionManager.SetValue("SearchModelBaiVietFB", searchModel);
            return Json(GetData());
        }

        [HttpPost]

        public JsonResult Delete(long id)
        {
            var model = new JsonResultBO(true);
            try
            {


                FbPostBusiness = Get<FbPostBusiness>();
                FbPostBusiness.Delete(id);
                FbPostBusiness.Save();

            }
            catch
            {

                model.Status = false;
                model.Message = "Không tìm thấy bài viết";
            }
            return Json(model);
        }
        public PartialViewResult XemBaiViet(long id)
        {
            FbPostBusiness = Get<FbPostBusiness>();
            var post = FbPostBusiness.Find(id);
            return PartialView("_ViewBaiVietPartial", post);
        }

        public ActionResult Edit(long id)
        {
            FbPostBusiness = Get<FbPostBusiness>();
            var bv = FbPostBusiness.Find(id);
            return View(bv);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveEditPost(long id, string tieude, string NoiDung, string keyWord)
        {
            var model = new JsonResultBO(true);
            try
            {



                AssignUserInfo();
                FbPostBusiness = Get<FbPostBusiness>();
                var po = FbPostBusiness.Find(id);
                po.NOIDUNG = NoiDung;
                po.TIEUDE = tieude;
                po.TUKHOA = keyWord;
                po.NGAYSUA = DateTime.Now;
                po.NGUOISUA = currentUserId;
                FbPostBusiness.Save(po);
            }
            catch
            {

                model.Status = false;
                model.Message = "Không cập nhật được bài viết";
            }
            return Json(model);
        }


        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SavePost(string tieude, string NoiDung, string keyWord)
        {
            var model = new JsonResultBO(true);
            AssignUserInfo();
            FbPostBusiness = Get<FbPostBusiness>();
            var po = new FB_POST();
            po.NOIDUNG = NoiDung;
            po.TIEUDE = tieude;
            po.TUKHOA = keyWord;
            po.NGAYTAO = DateTime.Now;
            po.NGAYSUA = DateTime.Now;
            po.NGUOITAO = currentUserId;
            po.NGUOISUA = currentUserId;
            FbPostBusiness.Save(po);
            return Json(model);
        }

        public ActionResult Emotion()
        {
            AssignUserInfo();
            FbChuKyBusiness = Get<FbChuKyBusiness>();
            var model = new TaoBaiVietVM();
            model.ChuKy = FbChuKyBusiness.GetChuKy(currentUserId);
            return View(model);
        }

    }
}
