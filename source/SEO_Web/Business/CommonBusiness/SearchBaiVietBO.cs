using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class SearchBaiVietBO
    {
        public int IDParent { get; set; }
        public string TieuDe { get; set; }
        public DateTime? StartNgayTao { get; set; }
        public DateTime? EndNgayTao { get; set; }
    }
}
