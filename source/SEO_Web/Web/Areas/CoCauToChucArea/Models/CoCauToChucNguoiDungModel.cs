using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;
using Business.CommonBusiness;

namespace Web.Areas.CoCauToChucArea.Models
{
    public class CoCauToChucNguoiDungModel
    {
        public CCTC_THANHPHAN Item { get; set; }
        public List<DMNguoiDungBO> ListNguoiDung { get; set; }
    }
}