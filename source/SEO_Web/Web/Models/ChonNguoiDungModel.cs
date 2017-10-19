using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class ChonNguoiDungModel
    {
        public int COSO_ID { get; set; }
        public int DONVI_ID { get; set; }
        public int PHONGBAN_ID { get; set; }
        public string TEXT_ID { get; set; }
        public string VALUE_ID { get; set; }
        public string ID_CLICK { get; set; }
        public int IS_MULTICHOICE { get; set; }
        public string[] IDS { get; set; }
        public string CALLBACK_FUNCTION { get; set; }
        public string TITLE { get; set; }
        public int INDEX { get; set; }
        public string SHOW_CHUC_VU_ID { get; set; }
        public List<DM_NGUOIDUNG> LstNguoiDung { get; set; }
        public List<DM_NGUOIDUNG> LstNguoiDungSearch { get; set; }
        //public List<SelectListItem> LstChucVu { get; set; }
        public Dictionary<int, string> DictChucVu { get; set; }
        public string EXCLUDE_IDS { get; set; }
      
    }
}
