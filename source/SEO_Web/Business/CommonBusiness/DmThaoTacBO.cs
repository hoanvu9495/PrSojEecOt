using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class DmThaoTacBO
    {
        public int? DM_CHUCNANG_ID { get; set; }
        public long DM_THAOTAC_ID { get; set; }
        public string MENU_LINK { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string NGUOISUA { get; set; }
        public string NGUOITAO { get; set; }
        public string TEN_THAOTAC { get; set; }
        public string THAOTAC { get; set; }
        public string TenChucNang { get; set; }
        public int? TRANGTHAI { get; set; }
        public bool Checked { get; set; }
        public int? TT_HIENTHI { get; set; }
        public bool? IS_HIENTHI { get; set; }
    }
}
