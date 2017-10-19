using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class NltthLichsuNangluongBO
    {
        public string TENKEHOACHTANGLUONG { get; set; }
        public int KEHOACH_ID { get; set; }
        public string TUBAC { get; set; }
        public string TUHESO { get; set; }
        public string LENBAC { get; set; }
        public string LENHESO { get; set; }
        public DateTime? NGAYTANGLUONG { get; set; }
        public long USER_ID { get; set; }
        public string TENNHANVIEN { get; set; }
        public bool? DUOCTANGLUONG { get; set; }
    }
}
