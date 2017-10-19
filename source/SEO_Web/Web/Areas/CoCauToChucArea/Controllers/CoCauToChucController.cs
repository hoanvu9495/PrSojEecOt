using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Business;
using Business.CommonBusiness;
using Web.FwCore;
using Web.Common;
using Web.Custom;
using Model.DBTool;
using Web.Areas.CoCauToChucArea.Models;


namespace Web.Areas.CoCauToChucArea.Controllers
{
    public class CoCauToChucController : BaseController
    {
        //
        // GET: /CoCauToChucArea/CoCauToChuc/
        #region KhaiBao
        CCTCThanhPhanBusiness CCTCThanhPhanBusiness;
        DmNguoidungBusiness DmNguoidungBusiness;
        DMLoaiDonViBusiness DMLoaiDonViBusiness;
        #endregion
        public ActionResult Index()
        {
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            DMLoaiDonViBusiness = Get<DMLoaiDonViBusiness>();
            CoCauToChucIndexModel model = new CoCauToChucIndexModel();
            var listAll = CCTCThanhPhanBusiness.GetAllByLeVelUp();
            var dataTree = CCTCThanhPhanBusiness.GetTree();
            model.TreeData = dataTree;
            model.DS_TYPE = DMLoaiDonViBusiness.DSLoaiDonVi();
            model.ListCoCau = listAll;
            return View(model);
        }
        [HttpPost]
        public JsonResult CheckCode( string code,int id=0)
        {
            var result = new JsonResultBO();
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            result.Status = CCTCThanhPhanBusiness.ExistCode(code, id);
            return Json(result);
        }
        public JsonResult ReloadPage()
        {
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            var dataTree = CCTCThanhPhanBusiness.GetTree();
            return Json(dataTree, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNode(int id)
        {
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            var node = CCTCThanhPhanBusiness.Find(id);
            return Json(node, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUser(int id)
        {
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            var model = new CoCauToChucNguoiDungModel();
            var lstUser = DmNguoidungBusiness.GetByPhongBan(id);

            var node = CCTCThanhPhanBusiness.Find(id);
            model.Item = node;
            model.ListNguoiDung = lstUser;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDS(int id)
        {
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            var node = CCTCThanhPhanBusiness.All.Where(x => x.ID != id).OrderBy(x => x.ITEM_LEVEL).ThenBy(x => x.ID).ToList();
            return Json(node, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetParent(int id)
        {
            CoCauToChucUpdateModel model = new CoCauToChucUpdateModel();
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            var item = CCTCThanhPhanBusiness.Find(id);
            var list = CCTCThanhPhanBusiness.GetAllByLeVelUp(item.ITEM_LEVEL.ToString().ToIntOrZero()).Where(x => x.ID != id).ToList();
            model.DS_PARENT = list;
            model.Item = item;
            DMLoaiDonViBusiness = Get<DMLoaiDonViBusiness>();
            model.DS_TYPE = DMLoaiDonViBusiness.DSLoaiDonVi(item.TYPE);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(FormCollection form)
        {
            try
            {
                var cocau = new CCTC_THANHPHAN();
                cocau.NAME = form["NAME"];
                cocau.PARENT_ID = form["PARENT"].ToIntOrNULL();
                CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
                var parent = CCTCThanhPhanBusiness.Find(cocau.PARENT_ID);
                var user = (UserInfoBO)SessionManager.GetUserInfo();
                cocau.ITEM_LEVEL = parent.ITEM_LEVEL + 1;
                cocau.TYPE = form["TYPE"].ToIntOrZero();
                cocau.NGUOITAO = (int)user.UserID;
                cocau.NGAYTAO = DateTime.Now;
                cocau.CODE = form["CODE"];
                CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
                CCTCThanhPhanBusiness.Save(cocau);
                return Json(true);
            }
            catch
            {
                return Json(false);

            }

        }
        [HttpPost]
        public JsonResult CheckHasChild(int id)
        {
            CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
            return Json(CCTCThanhPhanBusiness.ExistChild(id));
        }

        public JsonResult Delele(int id)
        {
            try
            {
                CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
                CCTCThanhPhanBusiness.Delete(id);
                CCTCThanhPhanBusiness.Save();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(FormCollection form)
        {
            try
            {
                var id = form["ID_NODE"].ToIntOrZero();
                if (id > 0)
                {
                    CCTCThanhPhanBusiness = Get<CCTCThanhPhanBusiness>();
                    var cocau = CCTCThanhPhanBusiness.Find(id);
                    cocau.NAME = form["NAME"];
                    cocau.PARENT_ID = form["PARENT"].ToIntOrNULL();
                    var user = (UserInfoBO)SessionManager.GetUserInfo();
                    var parent = CCTCThanhPhanBusiness.Find(cocau.PARENT_ID);
                    cocau.ITEM_LEVEL = parent.ITEM_LEVEL + 1;
                    cocau.TYPE = form["TYPE"].ToIntOrZero();
                    cocau.NGUOISUA = (int)user.UserID;
                    cocau.NGAYSUA = DateTime.Now;
                    cocau.CODE = form["CODE"];
                    CCTCThanhPhanBusiness.Save(cocau);
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }

            }
            catch
            {
                return Json(false);

            }

        }
    }
}
