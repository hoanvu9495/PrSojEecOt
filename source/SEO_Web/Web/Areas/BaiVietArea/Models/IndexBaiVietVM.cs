using Business.CommonBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.BaiVietArea.Models
{
    public class IndexBaiVietVM
    {
        public PageListResultBO<BaiVietBO> ListBaiViet { get; set; }

    }
}