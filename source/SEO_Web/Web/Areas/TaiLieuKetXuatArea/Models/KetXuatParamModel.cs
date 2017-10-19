using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.TaiLieuKetXuatArea.Models
{
    public class KetXuatParamModel
    {
        public int idDoanhNghiep { get; set; }
        public int idDot { get; set; }
        public List<string> arrTaiLieu { get; set; }
    }
}