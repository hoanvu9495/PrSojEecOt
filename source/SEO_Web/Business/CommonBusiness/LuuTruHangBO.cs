
using System;
namespace Business.CommonBusiness
{
    public class LuuTruHangBO
    {
        public int? ID { get; set; }
        public int? KE_ID { get; set; }
        public string TENHANG { get; set; }
        public bool? TRANGTHAI { get; set; }
        public string TENKE { get; set; }
        public string TENPHONG { get; set; }
        public string TENTANG { get; set; }
        public string TENTOANHA { get; set; }
        public int? TOANHA_ID { get; set; }
        public int? TANG_ID { get; set; }
        public int? PHONG_ID { get; set; }
        public long? NGUOITAO { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public int? COSO_ID { get; set; }
        public bool? IS_DELETE { get; set; }
        public string MAHANG { get; set; }
        public string MOTA { get; set; }

    }
}
