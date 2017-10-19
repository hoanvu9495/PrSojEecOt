﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Areas.TDKTDangKyDanhHieuArea.Models;
using Web.Areas.TDKTPhongTraoThiDuaArea.Models;
using Web.Custom;
using Business.CommonBusiness;
using Web.FwCore;
using Web.Common;
using Novacode;
using System.Text.RegularExpressions;

namespace Web.Areas.TDKTDangKyDanhHieuArea.Controllers
{
    public class DanhSachThiDuaDonViController : BaseController
    {
        //
        // GET: /TDKTDangKyDanhHieuArea/DanhSachThiDuaDonVi/
        private TDKTPhongTraoThiDuaBusiness TDKTPhongTraoThiDuaBusiness;
        private TDKTCaNhanDangKyBusiness TDKTCaNhanDangKyBusiness;
        public ActionResult Index()
        {
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            PhongTraoThiDuaViewModel model = new PhongTraoThiDuaViewModel();
            model.LstPhongTrao = TDKTPhongTraoThiDuaBusiness.All.ToList();
            return View(model);
        }
        public ActionResult ViewReport(int ID)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            TDKTPhongTraoThiDuaBusiness = Get<TDKTPhongTraoThiDuaBusiness>();
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            model.LstDonDangKyCaNhan = TDKTCaNhanDangKyBusiness.All.Where(x => x.TRANGTHAI == DonDangKyThiDuaCaNhanConstant.TT_DATONGHOPGUI && x.PHONG_TRAO_ID == ID).ToList();
            List<SelectListItem> LstPhongTraoThiDua = TDKTPhongTraoThiDuaBusiness.All.ToList().Select(
                x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.KE_HOACH_THI_DUA
                }
                ).ToList();
            model.LstPhongTraoThiDua = LstPhongTraoThiDua;
            model.PhongTraoId = ID;
            return View(model);
        }
        public PartialViewResult ShowCondition(int ID)
        {
            TDKTCaNhanDangKyBusiness = Get<TDKTCaNhanDangKyBusiness>();
            CaNhanDangKyViewModel model = new CaNhanDangKyViewModel();
            TDKT_CANHANDANGKY dondangky = TDKTCaNhanDangKyBusiness.Find(ID);
            return PartialView("_ShowCondition");
        }
    }
}
