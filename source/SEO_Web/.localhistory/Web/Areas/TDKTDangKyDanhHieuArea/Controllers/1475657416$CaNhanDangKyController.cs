﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.Areas.TDKTDangKyDanhHieuArea.Models;
using Web.Custom;

namespace Web.Areas.TDKTDangKyDanhHieuArea.Controllers
{
    public class CaNhanDangKyController : BaseController
    {
        //
        // GET: /TDKTDangKyDanhHieuArea/CaNhanDangKy/
        private TdktDanhhieucanhanBusiness TdktDanhhieucanhanBusiness;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            List<SelectListItem> LstDanhHieuCaNhan = TdktDanhhieucanhanBusiness.All.Tolist().Select(

                ).ToList();
            return View();
        }

    }
}
