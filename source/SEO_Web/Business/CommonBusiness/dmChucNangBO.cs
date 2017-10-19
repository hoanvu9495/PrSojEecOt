using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class dmChucNangBO
    {
        public int? CHUCNANG_CHA { get; set; }
        public int DM_CHUCNANG_ID { get; set; }
        public int? IS_HIDDEN { get; set; }
        public string MA_CHUCNANG { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string NGUOISUA { get; set; }
        public string NGUOITAO { get; set; }
        public string TEN_CHUCNANG { get; set; }
        public int? TRANGTHAI { get; set; }
        public int? TT_HIENTHI { get; set; }
        public string URL { get; set; }
        public int? VAITROID { get; set; }
        public bool selected { get; set; }
        public string display { get; set; }
        public List<DmThaoTacBO> ListThaoTac { get; set; }
        public List<dmChucNangBO> ListChildren { get; set; }
    }
}
