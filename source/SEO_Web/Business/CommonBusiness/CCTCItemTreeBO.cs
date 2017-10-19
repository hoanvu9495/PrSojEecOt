using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class CCTCItemTreeBO
    {
        public int ID { get; set; }
        public bool? IS_DELETE { get; set; }
        public int? ITEM_LEVEL { get; set; }
        public string NAME { get; set; }
        public DateTime? NGAYSUA { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public int? NGUOISUA { get; set; }
        public int? NGUOITAO { get; set; }
        public int? PARENT_ID { get; set; }
        public int TYPE { get; set; }
        public string CODE { get; set; }
        public int TYPE_NAME { get; set; }
        public List<CCTCItemTreeBO> Child { get; set; }
    }
}
