using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class NguoiDungBO
    {
        public decimal DM_NGUOIDUNG_ID { get; set; }
        public string TENDANGNHAP { get; set; }
        public string MATKHAU { get; set; }
        public string MATKHAUMOI { get; set; }
        public string MAHOA_MK { get; set; }
        public int TRANGTHAI { get; set; }
        public string DIENTHOAI { get; set; }
        public int? DM_DIEM_TIEPNHAN_ID { get; set; }
        public int? DM_DONVI_QL_ID { get; set; }
        public int? DM_CUMTHI_ID { get; set; }
        public int? DM_TRUONGDH_ID { get; set; }
        public int? DM_VAITRO_ID { get; set; }
        public string TENVAITRO { get; set; }
        public int? TYPE { get; set; }
        public int? TYPEID { get; set; }
        public string TYPENAME { get; set; }
        public string MADONVI { get; set; }
        public string TENDONVI { get; set; }
        public string NGUOITAO { get; set; }
        public string NGUOISUA { get; set; }
        public int RoleUser { get; set; }
        public string EMAIL { get; set; }
        public int? COSO_ID { get; set; }

        public int? HINHTHUC { get; set; }
        public int? TINH_ID { get; set; }
        public int? HUYEN_ID { get; set; }
        public long? XA_ID { get; set; }
        public int? DM_PHONGBAN_ID { get; set; }
        public int? DM_DONVI_ID { get; set; }
        public string TEN_COSO { get; set; }
        public int? CAPCOSO_ID { get; set; }
        public string CAPCOSO { get; set; }
        public string HOTEN { get; set; }
        public int? CAPCOSO_PARENT { get; set; }
        public string PASS_OLD { get; set; }
        public int? CHUCVU_ID { get; set; }
        public List<int> ListVaiTro { get; set; }
        public int? OptionRole { get; set; }
        public string FTS { get; set; }
        public string CANBO { get; set; }
        public int? HSCB_ID { get; set; }
        public int? LOAI_HD { get; set; }
        public string ANH_DAIDIEN { get; set; }
    }
}
