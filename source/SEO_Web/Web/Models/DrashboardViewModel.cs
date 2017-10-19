using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DrashboardViewModel
    {
        public List<DrashboardRowViewModel> ListDrashboardRowViewModel { get; set; }
        //public SYS_CAUHINH_HANGHIENTHI CauHinhHangHienThi { get; set; }
        //public List<SYS_CAUHINHTRANGCHU> ListAllChucNang { get; set; }
    }
    public class DrashboardFunctionViewModel
    {
        public List<int> ListChucNangInRow { get; set; }
        //public List<SYS_CHUCNANGTRANGCHU> ListAllChucNang { get; set; }
        public int ROW { get; set; }
    }
    public class DrashboardRowViewModel
    {
        //public SYS_CAUHINHHIENTHI CauHinhHienThi { get; set; }
        //public List<SYS_CAUHINHTRANGCHU> ListChucNang { get; set; }
        public float Width { get; set; }
    }
   
}