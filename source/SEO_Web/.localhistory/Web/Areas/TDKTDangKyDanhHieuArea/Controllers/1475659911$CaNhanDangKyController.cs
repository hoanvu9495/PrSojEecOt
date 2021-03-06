﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Areas.TDKTDangKyDanhHieuArea.Models;
using Web.Custom;
using Business.CommonBusiness;
using Web.FwCore;
using Web.Common;

namespace Web.Areas.TDKTDangKyDanhHieuArea.Controllers
{
    public class CaNhanDangKyController : BaseController
    {
        //
        // GET: /TDKTDangKyDanhHieuArea/CaNhanDangKy/
        private TdktDanhhieucanhanBusiness TdktDanhhieucanhanBusiness;
        private DmDonViBusiness DmDonViBusiness;
        private TDKTCaNhanDangKyBusiness TDKTCaNhanDangKyBusiness;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.DANHHIEUTHIDUA
                }
                ).ToList();
            model.LstDanhHieuCaNhan = LstDanhHieuCaNhan;
            return View(model);
        }
        public ActionResult Save(FormCollection coll)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            DmDonViBusiness = Get<DmDonViBusiness>();
            TDKT_CANHANDANGKY model = new TDKT_CANHANDANGKY();
            model.DANHHIEU_IDS = coll["DANHHIEU_IDS"];
            model.TUDANHGIA = coll["TUDANHGIA"];
            model.USER_ID = (long)UserInfo.UserID;
            model.NGAYDANGKY = DateTime.Now;
            model.HO_TEN = UserInfo.Fullname;
            DM_DONVI donvi = DmDonViBusiness.Find(UserInfo.DonViID);
            model.DON_VI = donvi.TEN_DONVI;
            TDKTCaNhanDangKyBusiness.Save(model);
            return RedirectToAction("View", new { ID = model.ID });
        }
        public ActionResult View(int ID)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();

            TDKT_CANHANDANGKY dangky = TDKTCaNhanDangKyBusiness.Find(ID);
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.DonDangKyCaNhan = dangky;
            List<int> DanhHieuIds = dangky.DANHHIEU_IDS.ToLisInt();
            model.LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness
            return View(model);
        }
    }
}
