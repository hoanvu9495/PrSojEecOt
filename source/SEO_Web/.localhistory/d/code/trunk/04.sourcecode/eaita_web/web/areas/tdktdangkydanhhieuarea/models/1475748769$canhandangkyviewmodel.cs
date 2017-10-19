﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.CommonBusiness;
namespace Web.Areas.TDKTDangKyDanhHieuArea.Models
{
    public class CaNhanDangKyViewModel
    {
        public List<SelectListItem> LstPhongTraoThiDua { get; set; }
        public List<SelectListItem> LstDanhHieuCaNhan { get; set; }
        public TDKT_CANHANDANGKY DonDangKyCaNhan { get; set; }
        public List<TDKT_CANHANDANGKY> LstDonDangKyCaNhan { get; set; }
        public int PhongTraoId { get; set; }
        public List<TDKT_DANHHIEUCANHAN> LstDanhHieuCaNhanModel { get; set; }
        public List<TDKT_DIEUKIENDANHHIEUCANHAN> LstCondDateTime { get; set; }
        public List<TDKTDanhHieuCaNhanBO> LstCondDanhHieu { get; set; }
    }
}