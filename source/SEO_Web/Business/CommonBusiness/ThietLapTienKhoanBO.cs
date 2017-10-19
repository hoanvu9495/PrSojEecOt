using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class ThietLapTienKhoanBO
    {
        public int? ID { get; set; }
        public int? VAITRO_ID { get; set; }
        public string LOAI_KHOAN { get; set; }
        public int? TIENKHOAN { get; set; }
        public bool? TRANGTHAI { get; set; }
        public int? COSO_ID { get; set; }
        public long? NGUOITAO { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string TENVAITRO { get; set; }
    }
}
