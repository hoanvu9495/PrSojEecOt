using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
   public class NguoiDungVaiTroBO
    {
       public long? DM_NGUOIDUNG_ID { get; set; }
       public string DIENTHOAI { set; get; }
       public string HOTEN { get; set; }       
       public string HOTEN_FTS { set; get; }
       public string VAITRO { get; set; }
       public string MA_VAITRO { set; get; }
       public int? DM_VAITRO_ID { get; set; }
       public string TENDANGNHAP { get; set; }
       public int? DONVI_ID { get; set; }
       public string TEN_DONVI { set; get; }
       public int? COSO_ID { get; set; }
       public bool SELECTED { get; set; }
       public string EMAIL { get; set; }
    }
}
