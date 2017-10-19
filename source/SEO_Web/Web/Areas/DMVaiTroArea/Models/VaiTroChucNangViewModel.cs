using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.CommonBusiness;
using Model.DBTool;

namespace Web.Areas.DMVaiTroArea.Models
{
    public class PhanQuyenViewModel
    {
        public DM_VAITRO VaiTro { get; set; }
        public List<dmChucNangBO> ListChucNang { get; set; }
        public bool? IS_MAXLEVER { get; set; }//CAP  CO SO CAO NHAT
        public List<SelectListItem> ListCoSo { get; set; }
        public int? COSO_ID { get; set; }
        public string TENCAP_COSO { get; set; }
        public string TEN_COSO { get; set; }
        public List<SelectListItem> DSChucNang { get; set; }
        public TreeNodeBO treeData { get; set; }
    }
    public class DefaultChucNangViewMoel
    {
        public DM_VAITRO VaiTro { get; set; }
        public List<dmChucNangBO> ListChucNang { get; set; }
    }
    public class ChucNangTrangChuViewModel
    {
        public List<int> ListChucNangInRole { get; set; }
        //public List<SYS_CHUCNANGTRANGCHU> ListAllChucNang { get; set; }
        public int DM_VAITRO_ID { get; set; }
        public List<NguoiDungBO> ListUserIDByRoleID { get; set; }

    }
    public class VaiTroViewModel
    {
        public bool RoleIsValid { get; set; }
        public DmVaiTroBO VaiTro { get; set; }
        public List<SelectListItem> ListCapCoSo { get; set; }
        public List<SelectListItem> ListCoSo { get; set; }
        public List<DmVaiTroBO> listVaitro { get; set; }
    }
}