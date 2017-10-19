using Business.Business;
using Business.CommonBusiness;
using Model.eAita;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Areas.QLHopDong68Area.Models;
using Web.Common;
using Web.Custom;
using Web.FwCore;

namespace Web.Areas.QLHopDong68Area.Controllers
{
    public class QLHopDong68Controller : BaseController
    {
        //
        // GET: /QLHopDongLaoDongArea/QLHopDongLaoDong/

        private HscbHopDongLD68Business HscbHopDongLD68Business;

        public ActionResult Index()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (!Ultilities.IsInActivities(user.ListThaoTac, "/QLHopDong68Area/QLHopDong68/Index"))
            {
                return RedirectToAction("UnAuthor", "Home", new { area = "" });
            }
            HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();

            SessionManager.Remove("ListHopDong68");
            SessionManager.Remove("ListHopDong68Search");

            List<HscbHopDongLaoDongBO> lstHopDong = FillDataToGrid();

            SessionManager.SetValue("ListHopDong68", lstHopDong);
            ViewData["Search"] = "0";
            return View();
        }
        public List<HscbHopDongLaoDongBO> FillDataToGrid()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();
            List<HscbHopDongLaoDongBO> lstHopDong = HscbHopDongLD68Business.GetListHopDong68(user.DonViID);
            return lstHopDong;
        }

        public PartialViewResult FindHopDong(string NGUOI_TUYENDUNG, string NGUOI_DUOCTUYEN, string ListStatus)
        {
            HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();
            List<HscbHopDongLaoDongBO> HopDongResult = (List<HscbHopDongLaoDongBO>)SessionManager.GetValue("ListHopDong68");
            if (HopDongResult != null)
            {
                if (!string.IsNullOrEmpty(NGUOI_TUYENDUNG))
                {
                    HopDongResult = HopDongResult.Where(x => x.NGUOI_TUYENDUNG.ToLower().Contains(NGUOI_TUYENDUNG.ConvertToFTS().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(NGUOI_DUOCTUYEN))
                {
                    HopDongResult = HopDongResult.Where(x => x.NGUOI_DUOCTUYEN.ToLower().Contains(NGUOI_DUOCTUYEN.ConvertToFTS().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(ListStatus))
                {

                    var lst = new List<int>();
                    lst = ListStatus.ToListInt(',');
                    if (lst != null && lst.Count > 0)
                    {
                        HopDongResult = HopDongResult.Where(x => x.TYPE_HOPDONG.HasValue ? lst.Contains(x.TYPE_HOPDONG.Value) : false).ToList();
                    }
                }
                else
                {
                    HopDongResult = new List<HscbHopDongLaoDongBO>();
                }

            }
            ViewData["Search"] = "1";
            ViewData["NGUOI_TUYENDUNG"] = NGUOI_TUYENDUNG;
            ViewData["NGUOI_DUOCTUYEN"] = NGUOI_DUOCTUYEN;
            ViewData["ListStatus"] = ListStatus;

            SessionManager.SetValue("ListHopDong68Search", HopDongResult);
            return PartialView("_QLHopDong68SearchResult");
        }

        public ActionResult CreateHopDong68()
        {
            return View();
        }
        public ActionResult EditHopDong68(int id)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (!Ultilities.IsInActivities(user.ListThaoTac, "/QLHopDong68Area/QLHopDong68/Index"))
            {
                return RedirectToAction("UnAuthor", "Home", new { area = "" });
            }
            List<HscbHopDongLaoDongBO> HoSoResult = (List<HscbHopDongLaoDongBO>)SessionManager.GetValue("ListHopDong68");
            if (HoSoResult == null)
            {
                HoSoResult = FillDataToGrid();
                if (HoSoResult == null)
                {
                    return RedirectToAction("Notfound", "Home", new { area = "" });
                }
                var CHECK_ROLE = HoSoResult.Where(x => x.ID == id).FirstOrDefault();
                if (CHECK_ROLE == null)
                {
                    return RedirectToAction("Notfound", "Home", new { area = "" });
                }
            }
            else
            {
                var CHECK_ROLE = HoSoResult.Where(x => x.ID == id).FirstOrDefault();
                if (CHECK_ROLE == null)
                {
                    return RedirectToAction("Notfound", "Home", new { area = "" });
                }
            }

            HopDong68ViewModel model = new HopDong68ViewModel();
            List<SelectListItem> ListMonth = new List<SelectListItem>();
            HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();
            var HopDong = HscbHopDongLD68Business.Find(id);
            model.HopDongLaoDong = HopDong;
            if (HopDong.TYPE_HOPDONG.HasValue)
            {
                if (HopDong.TYPE_HOPDONG == QLhopDongConstant.HD_MUA_VU)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        ListMonth.Add(new SelectListItem { Value = i.ToString(), Text = i + " tháng", Selected = (i == HopDong.SOTHANG) });
                    }
                }
                else if (HopDong.TYPE_HOPDONG == QLhopDongConstant.HD_XACDINH_THOIHAN)
                {
                    for (int i = 12; i <= 36; i++)
                    {
                        ListMonth.Add(new SelectListItem { Value = i.ToString(), Text = i + " tháng", Selected = (i == HopDong.SOTHANG) });
                    }
                }
            }
            model.ListMonth = ListMonth;


            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(HSCB_HOPDONG_LD_68 HopDong, FormCollection fm)
        {
            HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();

            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            HopDong.LOAI_HOP_DONG = QLhopDongConstant.HOPDONG_68;
            HopDong.LAST_UPDATE = DateTime.Now;
            HopDong.DONVI_ID = user.DonViID;

            #region LƯU THÔNG TIN DATE
            if (!string.IsNullOrEmpty(fm["TUNGAY"]))
            {
                try
                {
                    HopDong.TUNGAY = fm["TUNGAY"].ToDateTime();

                }
                catch
                {

                    HopDong.TUNGAY = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["NGAY_HIEULUC"]))
            {
                try
                {
                    HopDong.NGAY_HIEULUC = fm["NGAY_HIEULUC"].ToDateTime();

                }
                catch
                {

                    HopDong.NGAY_HIEULUC = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["NGAYCAP_CMTND"]))
            {
                try
                {
                    HopDong.NGAYCAP_CMTND = fm["NGAYCAP_CMTND"].ToDateTime();

                }
                catch
                {

                    HopDong.NGAYCAP_CMTND = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["NGAYCAP_SO"]))
            {
                try
                {
                    HopDong.NGAYCAP_SO = fm["NGAYCAP_SO"].ToDateTime();
                }
                catch
                {

                    HopDong.NGAYCAP_SO = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["NGAYLAM"]))
            {
                try
                {
                    HopDong.NGAYLAM = fm["NGAYLAM"].ToDateTime();
                }
                catch
                {

                    HopDong.NGAYLAM = null;
                }
            }

            if (!string.IsNullOrEmpty(fm["NGAYTAO"]))
            {
                try
                {
                    HopDong.NGAYTAO = fm["NGAYTAO"].ToDateTime();
                }
                catch
                {

                    HopDong.NGAYTAO = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["THUVIEC_TUNGAY"]))
            {
                try
                {
                    HopDong.THUVIEC_TUNGAY = fm["THUVIEC_TUNGAY"].ToDateTime();
                }
                catch
                {

                    HopDong.THUVIEC_TUNGAY = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["THUVIEC_DENNGAY"]))
            {
                try
                {
                    HopDong.THUVIEC_DENNGAY = fm["THUVIEC_DENNGAY"].ToDateTime();
                }
                catch
                {

                    HopDong.THUVIEC_DENNGAY = null;
                }
            }

            if (!string.IsNullOrEmpty(fm["SINH_NGAY"]))
            {
                try
                {
                    HopDong.SINH_NGAY = fm["SINH_NGAY"].ToDateTime();
                }
                catch
                {

                    HopDong.SINH_NGAY = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["TRALUONG_VAONGAY"]))
            {
                try
                {
                    HopDong.TRALUONG_VAONGAY = fm["TRALUONG_VAONGAY"].ToDateTime();
                }
                catch
                {

                    HopDong.TRALUONG_VAONGAY = null;
                }
            }
            if (!string.IsNullOrEmpty(fm["DENNGAY"]))
            {
                try
                {
                    HopDong.DENNGAY = fm["DENNGAY"].ToDateTime();
                }
                catch
                {

                    HopDong.DENNGAY = null;
                }
            }
            HopDong.LAST_UPDATE = DateTime.Now;
            HscbHopDongLD68Business.Save(HopDong);
            #endregion
            //HopDong.IS_DELETE = false;
            return Json("SUCCESS");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteHopDong(int id)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (id > 0)
            {
                HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();
                var HopDOng = HscbHopDongLD68Business.Find(id);
                HopDOng.IS_DELETE = true;
                HscbHopDongLD68Business.Save(HopDOng);
            }
            return Json("Xóa hồ sơ thành công", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            HscbHopDongLD68Business = Get<HscbHopDongLD68Business>();
            HopDong68DetailModel model = new HopDong68DetailModel();
            model.HopDongLamViec = new HSCB_HOPDONG_LD_68();
            if (id > 0)
            {
                var HopDong = HscbHopDongLD68Business.Find(id);
                if (HopDong != null)
                {
                    model.HopDongLamViec = HopDong;
                    if (HopDong.TYPE_HOPDONG == QLhopDongConstant.HD_KHONG_XACDINH_THOIHAN)
                    {
                        model.LoaiHopDong = QLhopDongConstant.HD_KHONG_XACDINH_THOIHAN_TEXT;
                    }
                    else if (HopDong.TYPE_HOPDONG == QLhopDongConstant.HD_MUA_VU)
                    {
                        model.LoaiHopDong = QLhopDongConstant.HD_MUA_VU_TEXT;
                    }
                    else if (HopDong.TYPE_HOPDONG == QLhopDongConstant.HD_XACDINH_THOIHAN)
                    {
                        model.LoaiHopDong = QLhopDongConstant.HD_XACDINH_THOIHAN_TEXT;
                    }
                    if (HopDong.SOTHANG.HasValue)
                    {
                        model.SoThang = HopDong.SOTHANG + " tháng";
                    }
                }
                else
                {
                    model.HopDongLamViec = new HSCB_HOPDONG_LD_68();
                }
            }

            return View(model);
        }

    }
}
