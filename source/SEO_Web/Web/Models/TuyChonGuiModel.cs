using Business.CommonBusiness;
using System.Collections.Generic;

namespace Web.Models
{
    public class TuyChonGuiModel
    {
        public int COSO_ID { get; set; }
        public int DM_VAITRO_ID { set; get; }
        public int DONVI_ID { get; set; }
        public List<string> LISTROLE { get; set; }
        public bool IS_MULTI { get; set; }
        public long USER_ID { get; set; }
        public string CALL_BACK_FUNCTION { get; set; }
        public string TIEUDE { get; set; }
        public List<NguoiDungVaiTroBO> lstNguoiDung { get; set; }
        public List<DmDonViBO> ListDonVi { set; get; }
        public string SELECTED { get; set; }
        public string EXISTED { get; set; }
    }
}