using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class UyQuyenBO
    {
        
        public int ID { get; set; }
        public int MODULE_ID { get; set; }
        public string MODULE { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public int NGUOI_DUOC_UYQUYEN { get; set; }
        public string NGUOI_DUOC_UYQUYEN_NAME { get; set; }
        public int NGUOI_UYQUYEN { get; set; }
        public string NGUOI_UYQUYEN_NAME { get; set; }

        public DateTime? TIME_START { get; set; }
        public DateTime? TIME_END { get; set; }
        public int? NGUOISUA { get; set; }
        public int? NGUOITAO { get; set; }
    }
}
