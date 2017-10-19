using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
namespace Web.Areas.TDKTDangKyDanhHieuArea.Models
{
    public class CaNhanDangKyViewModel
    {
        public List<SelectListItem> LstDanhHieuCaNhan { get; set; }
        public TDKT_CANHANDANGKY DonDangKyCaNhan { get; set; }
        public List<TDKT_CANHANDANGKY> LstDonDangKyCaNhan { get; set; }
    }
}