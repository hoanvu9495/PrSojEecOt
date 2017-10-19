using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class NltthDonxinNangluongBO
    {
        public int? DONVI_ID { get; set; }
        public string DONVI { get; set; }
        public int ID { get; set; }
        public string LEN_BAC { get; set; }
        public double LEN_HE_SO { get; set; }
        public DateTime? NGAYCAPNHAT { get; set; }
        public DateTime NGAYTAO { get; set; }
        public DateTime? NGAYTTRINHDON { get; set; }
        public long? NGUOICAPNHAT_ID { get; set; }
        public long NGUOITAO_ID { get; set; }
        public int TIME_NLTTH { get; set; }
        public int? TRANGTHAI { get; set; }
        public string TUDANHGIA { get; set; }
        public long USER_ID { get; set; }
        public string YKIEN_DANHGIA { get; set; }
        public string BACHIENTAI { get; set; }
        public string CHUCVU_CHUCDANH { get; set; }
        public double? HESOHIENTAI { get; set; }
        public string HOTEN { get; set; }
        //bool TRINHTRUONGPHONG, bool GUIVANTHU, bool TRINHCUCTRUONG, bool GUICHANHVANPHONG, bool CHUYENCHOCHUYENVIEN
        public bool? TRINHTRUONGPHONG { get; set; }
        public bool? GUIVANTHU { get; set; }
        public bool? TRINHCUCTRUONG { get; set; }
        public bool? GUICHANHVANPHONG { get; set; }
        public bool? CHUYENCHOCHUYENVIEN { get; set; }
        public string CHUYENVIENXULY { get; set; }
        public long? CHUYENVIENXULY_ID { get; set; }
        public bool? CHUYENVIEN_DONGY { get; set; }
        public bool? CHUYENVIEN_KHONGDONGY { get; set; }
        public bool? CUCTRUONG_PHEDUYET { get; set; }
        public bool? CUCTRUONG_TUCHOI { get; set; }
        public int KEHOACH_ID { get; set; }
        public string KEHOACH { get; set; }
    }
}
