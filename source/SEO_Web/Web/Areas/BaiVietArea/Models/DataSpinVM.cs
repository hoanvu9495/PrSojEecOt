using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;
using Business.CommonBusiness;
namespace Web.Areas.BaiVietArea.Models
{
    public class DataSpinVM
    {
        public List<GroupTuDienBO> DataSpin { get; set; }
        public BaiVietBO BaiViet { get; set; }
    }
}