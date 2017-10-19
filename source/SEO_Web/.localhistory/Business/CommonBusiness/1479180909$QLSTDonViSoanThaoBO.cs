using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class DTPLDonViSoanThaoBO
    {
        public int? ID { get; set; }
        public string TIEUDE { get; set; }
        public string NOIDUNG { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public long? NGUOITAO { get; set; }
        public int? TRANGTHAI { get; set; }
        public string NGUOITHAMGIA { get; set; }
        public long? USER_ID { get; set; }
    }
}
