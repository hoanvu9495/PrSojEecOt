using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class CoSoBO
    {
        public int COSO_ID { get; set; }
        public string DIACHI { get; set; }
        public string DIENTHOAI { get; set; }
        public string EMAIL { get; set; }
        public string FAX { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public bool? IS_DELETE { get; set; }
        public DateTime? LAST_MODIFIED_DATE { get; set; }
        public string MACOSO { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string NGUOISUA { get; set; }
        public string NGUOITAO { get; set; }
        public string TENCOSO { get; set; }
        public int? TINH_ID { get; set; }
        public string WEBSITE { get; set; }
        public string TENTINH { get; set; }
        public int? CAPCOSO_ID { get; set; }
        public string CAPCOSO { get; set; }
        public int? CAPCOSO_PARENT { get; set; }
        public string TRUCTHUOC_COSO { get; set; }
    }
}
