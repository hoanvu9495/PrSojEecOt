using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business.CommonBusiness;
using Model.DBTool;


namespace Web.Areas.BaiVietArea.Models
{
    public class ImportSpinVM
    {
        public List<GroupTuDienBO> LstTuSpin { get; set; }
        public BaiVietBO BaiViet { get; set; }
        public List<BaiVietGroupTuBO> lstBaiViet { get; set; }
    }
}