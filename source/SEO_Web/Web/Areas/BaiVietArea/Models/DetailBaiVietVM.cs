﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business.CommonBusiness;
using Model.DBTool;
namespace Web.Areas.BaiVietArea.Models
{
    public class DetailBaiVietVM
    {
        public BaiVietBO BaiViet { get; set; }

        public PageListResultBO<SPIN_BAIVIET> LstBaiVietExtend { get; set; }
    }
}