using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class DmCapCoSoBO
    {
        public string CAPCOSO { get; set; }
        public int ID { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string NGUOISUA { get; set; }
        public string NGUOITAO { get; set; }
        public int? PARENT_CAPCOSO { get; set; }
        public bool? TRANGTHAI { get; set; }
        public string PARENT_NAME { get; set; }

    }
}
