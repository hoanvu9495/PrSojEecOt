using Business.Business;
using Model.DBTool;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Areas.DMThaoTacArea.Models;
using Web.Custom;
using Web.FwCore;
using Business.CommonBusiness;
using Web.Common;
using System;

namespace Web.Areas.DMThaoTacArea.Controllers
{
    /// <summary>
    /// written by: namdv
    /// created date: 10/06/2015
    /// reviewed by: namdv
    /// review date: 10/06/2015
    /// </summary>
    public class DMThaoTacController : BaseController
    {

        private DmThaotacBusiness DmThaotacBusiness;
        private DmChucnangBusiness DmChucnangBusiness;
        private VaitroThaotacBusiness VaitroThaotacBusiness;
        /// <summary>
        /// Màn hình danh sách thao tác
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //if (user.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            SessionManager.Remove("ListThaoTac");
            SessionManager.Remove("ListThaoTacSearch");
            List<DmThaoTacBO> lst = FillDataToGrid();
            ViewData["Search"] = "0";
            DMThaoTacIndexViewModel model = new DMThaoTacIndexViewModel();
            List<SelectListItem> lstChucNangCap1 = new List<SelectListItem>();
            lstChucNangCap1 = DmChucnangBusiness.All.ToList()
                        .Select(o => new SelectListItem()
                        {
                            Text = o.TEN_CHUCNANG,
                            Value = o.DM_CHUCNANG_ID.ToString()
                        }).ToList();
            model.ListChucNangCap1 = lstChucNangCap1;
            model.ListChucNangCap2 = new List<SelectListItem>();
            ViewData["CURRENTCHUCNANGCAP1"] = 0;
            ViewData["CURRENTCHUCNANGCAP2"] = 0;
            ViewData["TENTHAOTAC"] = string.Empty;
            model.ListThaoTac = lst.OrderBy(o => o.TRANGTHAI).ToList();
            //SessionManager.SetValue("ListThaoTac", lst.OrderBy(o => o.TRANGTHAI).ToList());

            return View(model);
        }


        

        /// <summary>
        /// Load lại danh sách thao tác sau khi thêm, sửa, xóa
        /// </summary>
        /// <param name="TENCHUCNANG">Tên thao tác đang tìm kiếm</param>
        /// <param name="CHUCNANGCHA">thao tác cha đang tìm kiếm</param>
        /// <returns></returns>
        public PartialViewResult ReloadGrid(string TENTHAOTAC = "", int? CHUCNANGCAP1 = 0, int? CHUCNANGCAP2 = 0)
        {
            ViewData["Search"] = "0";
            List<DmThaoTacBO> lst = FillDataToGrid();
            ViewData["CURRENTCHUCNANGCAP1"] = CHUCNANGCAP1;
            //ViewData["CURRENTCHUCNANGCAP2"] = CHUCNANGCAP2;
            ViewData["TENTHAOTAC"] = TENTHAOTAC;
            SessionManager.SetValue("ListThaoTac", lst.OrderBy(o => o.TRANGTHAI).ToList());
            return PartialView("_ThaoTacSearchResult");
        }

        /// <summary>
        /// Màn hình thêm mới thao tác
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AddFormThaoTac()
        {
            //UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            //if (userInfo.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            AssignUserInfo();
            DmThaotacBusiness = Get<DmThaotacBusiness>();
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DMThaoTacCreateViewModel model = new DMThaoTacCreateViewModel();
            List<SelectListItem> lstChucNangCap1 = DmChucnangBusiness.All.ToList()
                           .Select(o => new SelectListItem()
                           {
                               Text = o.TEN_CHUCNANG,
                               Value = o.DM_CHUCNANG_ID.ToString()
                           }).ToList();
            model.ListChucNangCap1 = lstChucNangCap1;
            model.ListChucNangCap2 = new List<SelectListItem>();
            return PartialView("_CreateActions", model);
        }

        /// <summary>
        /// Màn hình cập nhật thao tác
        /// </summary>
        /// <param name="DM_THAOTAC_ID">Mã tự tăng của thao tác</param>
        /// <returns></returns>
        public PartialViewResult EditFromThaoTac(decimal? DM_THAOTAC_ID)
        {
            //UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            //if (userInfo.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            DmThaotacBusiness = Get<DmThaotacBusiness>();
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DMThaoTacEditViewModel model = new DMThaoTacEditViewModel();


            DmThaoTacBO ThaoTac = DmThaotacBusiness.GetByID((int)DM_THAOTAC_ID);
                if (ThaoTac != null)
                {
                    if (ThaoTac.DM_CHUCNANG_ID.HasValue)
                    {
                        var chucnang = DmChucnangBusiness.Find(ThaoTac.DM_CHUCNANG_ID);
                        int cap1 = 0;
                        int cap2 = 0;
                        cap1 = chucnang.DM_CHUCNANG_ID;
                        List<SelectListItem> lstChucNangCap1 = DmChucnangBusiness.All.ToList()
                           .Select(o => new SelectListItem()
                           {
                               Text = o.TEN_CHUCNANG,
                               Value = o.DM_CHUCNANG_ID.ToString(),
                               Selected = (o.DM_CHUCNANG_ID == cap1)
                           }).ToList();
                        model.ListChucNangCap1 = lstChucNangCap1;                        
                    }
                    model.ThaoTac = ThaoTac;
                
            }
            return PartialView("_EditActions", model);
        }

        public JsonResult ReloadDataTable()
        {
            List<DmThaoTacBO> lst = FillDataToGrid();
            return Json(lst,JsonRequestBehavior.AllowGet);
                 
        }

        /// <summary>
        /// Lưu thông tin cập nhật của thao tác (POST METHOD)
        /// </summary>
        /// <param name="ThaoTac">Đối tượng ThaoTac</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public JsonResult SaveThaoTac(FormCollection f)
        {
            UserInfoBO user = ((UserInfoBO)SessionManager.GetUserInfo());

            DmThaotacBusiness = Get<DmThaotacBusiness>();
            DM_THAOTAC dmThaoTac = new DM_THAOTAC();
            if (!string.IsNullOrEmpty(f["DM_THAOTAC_ID"]))
            {
                dmThaoTac.DM_THAOTAC_ID = int.Parse(f["DM_THAOTAC_ID"]);
            }
            dmThaoTac.THAOTAC = f["THAOTAC"];
            dmThaoTac.TEN_THAOTAC = f["TEN_THAOTAC"];

            if (!string.IsNullOrEmpty(f["IS_HIENTHI"]))
            {
                dmThaoTac.IS_HIENTHI = true;
            }
            else
            {
                dmThaoTac.IS_HIENTHI = false;
            }

            dmThaoTac.DM_CHUCNANG_ID = short.Parse(f["CHUCNANGCAP1"]);
            //if (f["CHUCNANGCAP2"].ToShortOrZero() > 0)
            //{
            //    dmThaoTac.DM_CHUCNANG_ID = short.Parse(f["CHUCNANGCAP2"]);
            //}
            //else
            //{
            //    dmThaoTac.DM_CHUCNANG_ID = short.Parse(f["CHUCNANGCAP1"]);
            //}
            int ThemMoiThaoTac = 0;
            if (dmThaoTac.DM_THAOTAC_ID == ThemMoiThaoTac)
            {
                dmThaoTac.NGAYTAO = DateTime.Now;
                dmThaoTac.NGUOITAO = user.Username;
            }
            else
            {
                dmThaoTac.NGAYSUA = DateTime.Now;
                dmThaoTac.NGUOISUA = user.Username;

            }
            if (!string.IsNullOrEmpty(f["TT_HIENTHI"]))
            {
                dmThaoTac.TT_HIENTHI = int.Parse(f["TT_HIENTHI"]);
            }
            if (!string.IsNullOrEmpty(f["TRANGTHAI"]))
            {
                dmThaoTac.TRANGTHAI = short.Parse(f["TRANGTHAI"]);
            }
            if (!string.IsNullOrEmpty(f["MENU_LINK"]))
            {
                dmThaoTac.MENU_LINK = f["MENU_LINK"];
            }
            DmThaotacBusiness.Save(dmThaoTac);
            return Json(true);
        }

        /// <summary>
        /// Xóa thao tác
        /// </summary>
        /// <param name="DM_THAOTAC_ID">Mã tự tăng của thao tác</param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteThaoTac(int? DM_THAOTAC_ID)
        {
            //UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //if (user.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            DmThaotacBusiness = Get<DmThaotacBusiness>();
            //NguoidungThaotacBusiness = Get<NguoidungThaotacBusiness>();
            VaitroThaotacBusiness = Get<VaitroThaotacBusiness>();

            if (DM_THAOTAC_ID > 0)
            {
                //var LstNguoiDungThaoTac = NguoidungThaotacBusiness.All.Where(x => x.DM_THAOTAC_ID == DM_THAOTAC_ID).ToList();
                //foreach (var item in LstNguoiDungThaoTac)
                //{
                //    NguoidungThaotacBusiness.Delete(item.NGUOIDUNG_THAOTAC_ID);
                //    NguoidungThaotacBusiness.Save();
                //}

                var LstVaiTroThaoTac = VaitroThaotacBusiness.All.Where(x => x.DM_THAOTAC_ID == DM_THAOTAC_ID).ToList();
                foreach (var item in LstVaiTroThaoTac)
                {
                    VaitroThaotacBusiness.Delete(item.VAITRO_THAOTAC_ID);
                    VaitroThaotacBusiness.Save();
                }
                DmThaotacBusiness.Delete(DM_THAOTAC_ID);
                DmThaotacBusiness.Save();
            }
            return Json(new { Type = "SUCCESS", Message = "Xóa thao tác thành công!" });

        }

        /// <summary>
        /// Gán dữ liệu cho danh sách thao tác
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private List<DmThaoTacBO> FillDataToGrid(string tenchucnang = "", int? CHUCNANGCAP1 = 0, int? CHUCNANGCAP2 = 0)
        {
            //UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            DmThaotacBusiness = Get<DmThaotacBusiness>();

            return DmThaotacBusiness.GetListBySearchParam();
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult FindData(int CHUCNANGCHA)
        {
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            //var ListChucNang = DmChucnangBusiness.All.Where(x => x.CHUCNANG_CHA == CHUCNANGCHA);
            //var lol = ListChucNang.ToList();
            //if (ListChucNang.Count() > 0)
            //{
            //    var result = ListChucNang.Select(x => new
            //     {
            //         x.DM_CHUCNANG_ID,
            //         x.TEN_CHUCNANG
            //     });
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}
            return Json(false);
        }

    }
}
