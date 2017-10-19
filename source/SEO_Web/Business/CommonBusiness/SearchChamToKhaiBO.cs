using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class SearchChamToKhaiBO
    {
        public int IDDOTNOP { get; set; }
        public string MaSoThue { get; set; }
        public string TenDoanhNghiep { get; set; }
        public string TenTo { get; set; }
        public string KyNop { get; set; }
        public bool CanhCao { get; set; }
        public bool GiamNhe { get; set; }
        public DateTime? FROM_HanNop { get; set; }
        public DateTime? TO_HanNop { get; set; }
    }
}
