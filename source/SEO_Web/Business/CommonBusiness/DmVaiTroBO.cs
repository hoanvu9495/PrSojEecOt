using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class DmVaiTroBO
    {
        public int? COSO_ID { get; set; }
        public int DM_VAITRO_ID { get; set; }
        public string MA_VAITRO { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string NGUOISUA { get; set; }
        public string NGUOITAO { get; set; }
        public string TEN_VAITRO { get; set; }
        public int? TRANGTHAI { get; set; }
        public string TEN_COSO { get; set; }
        public string CAPCOSO { get; set; }
        public int? CAPCOSO_ID { get; set; }
        public int? CAPCOSO_PARENT { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
