using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace Business.CommonBusiness
{
    public class DataExportTHUEBO : DoanhNghiepViewModel
    {
   
        public int? SoTaiLieu { get; set; }
        public string MucPhat { get; set; }
        public int SoTinhTietTangNang { get; set; }
        public int SoTinhTietGiamNhe { get; set; }
        public string TenTo { get; set; }
        public bool ISCanhCao { get; set; }
        public string TienPhatTangNang { get; set; }
        public string TienGiamNhe { get; set; }
        public string TongTienPhat { get; set; }
        public string TongTienBangChu { get; set; }
        public string MucPhatChiTiet { get; set; }


        public DataExportTHUEBO(DoanhNghiepViewModel obj)
            : base(obj)
        {

        }

    }
}
