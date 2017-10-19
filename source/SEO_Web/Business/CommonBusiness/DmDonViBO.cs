using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class DmDonViBO
    {
        public int ID { get; set; }
        public int? COSO_ID { get; set; }
        public string TEN_DONVI { get; set; }
        public string COSO { get; set; }
        public bool? TRANGTHAI { get; set; }
        public int? CAPCOSO_ID { get; set; }
        public string CAPCOSO { get; set; }
        public int? CAPCOSO_PARENT { get; set; }
        public string MADONVI { get; set; }
    }
}
