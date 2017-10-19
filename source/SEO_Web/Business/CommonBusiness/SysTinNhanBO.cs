using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.CommonBusiness;
using Model.DBTool;
namespace Business.CommonBusiness
{
    public class SysTinNhanBO
    {
        public long ID { set; get; }
        public string TIEUDE { set; get; }
        public string NOIDUNG { get; set; }
        public string URL { set; get; }
        public string TEN_NGUOIGUI { set; get; }
        public string NGAYGUI_TEXT { set; get; }
        public bool IS_READ { set; get; }
        public DateTime? NGAY_GUI { set; get; }
    }
}
