﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.eAita;

namespace Web.Areas.TDKTPhongTraoThiDuaArea.Models
{
    public class PhongTraoThiDuaViewModel
    {
        public TDKT_PHONGTRAO_THIDUA PhongTraoThiDua { get; set; }
        public List<TAILIEUDINHKEM> LstTaiLieu { get; set; }
        public List<TDKT_PHONGTRAO_THIDUA> LstPhongTrao { get; set; }
    }
}