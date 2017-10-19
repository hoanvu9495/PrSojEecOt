using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class DMNguoiDungBO
    {
        public long DM_NGUOIDUNG_ID { get; set; }
        public string TENDANGNHAP { get; set; }
        public int TRANGTHAI { get; set; }
        public string DIENTHOAI { get; set; }
        
        public string EMAIL { get; set; }
        public string TEN_PHONGBAN { get; set; }

        public int? TINH_ID { get; set; }
        public int? HUYEN_ID { get; set; }
        public long? XA_ID { get; set; }
        public int? DM_PHONGBAN_ID { get; set; }
        public string HOTEN { get; set; }
        public int HOSO_ID { get; set; }

        public int? coSoId { set; get; }
        public string TEN_COSO { get; set; }
        public int? dmDonViId { set; get; }
        public string tenDonVi { set; get; }
        public int? vaiTroId { set; get; }
        public string tenVaiTro { set; get; }

        public int dmPhongBanId { set; get; }
        public string tenPhongBan { get; set; }
        public string  maVaiTro { get; set; }
    }
}
