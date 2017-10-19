using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DBTool;
using Business.CommonBusiness;

namespace Web.Areas.NguoiDungArea.Models
{
    public class PhanQuyenViewModel
    {
        public NguoiDungBO NguoiDung { get; set; }
        public DM_VAITRO VaiTro { get; set; }
        public List<dmChucNangBO> ListChucNang { get; set; }
        public bool? IS_MAXLEVER { get; set; }//CAP  CO SO CAO NHAT
        public List<SelectListItem> ListCoSo { get; set; }
        public int? COSO_ID { get; set; }
        public string TENCAP_COSO { get; set; }
        public string TEN_COSO { get; set; }
    }
    public class PhanQuyenNguoiDungViewModel
    {
        public DM_NGUOIDUNG NguoiDung { get; set; }
        public List<dmChucNangBO> ListChucNang { get; set; }
    }
    public class NguoiDungIndexViewModel
    {
        public CCTCItemTreeBO TreeDonVi { get; set; }
        public List<SelectListItem> ListTinhThanh { get; set; }
        public List<SelectListItem> ListVaiTro { get; set; }
        public List<SelectListItem> ListPhongBan { get; set; }
        public List<SelectListItem> ListDonVi { get; set; }
        public List<SelectListItem> ListCoSo { get; set; }
        public List<SelectListItem> ListCapCoSo { get; set; }
        public string phancap { get; set; }
        public int? HINHTHUC { get; set; }
        public List<SelectListItem> ListChucVu { get; set; }
        public string autocomplete { get; set; }
        public PageListResultBO<NguoiDungBO> ListNguoiDung { get; set; }
    }
    public class NguoiDungCreateViewModel
    {
        public List<SelectListItem> ListTinhThanh { get; set; }
        public List<SelectListItem> ListVaiTro { get; set; }
        public List<SelectListItem> ListPhongBan { get; set; }
        public List<SelectListItem> ListCoSo { get; set; }
        public List<SelectListItem> ListDonVi { get; set; }
        public List<SelectListItem> ListCapCoSo { get; set; }
        public List<SelectListItem> ListChucVu { get; set; }
        public CCTC_THANHPHAN DonVi { get; set; }
        public string phancap { get; set; }
        public int? hinhthuc { get; set; }
        public int? VAITROID { get; set; }
    }
    public class NguoiDungEditViewModel
    {
        
        public List<SelectListItem> ListVaiTro { get; set; }
        public List<int> ListVaiTroChecked { get; set; }
        public List<SelectListItem> ListPhongBan { get; set; }
        public List<SelectListItem> ListStatus { get; set; }
        public NguoiDungBO NGUOIDUNG { get; set; }
        public string phancap { get; set; }
        public short? HINHTHUC { get; set; }
        public int? VAITROID { get; set; }
        public List<SelectListItem> ListChucVu { get; set; }
        public CCTC_THANHPHAN DONVI { get; set; }

    }
    public class NguoiDungConstant
    {
        public const int USE_ROLE_DEFAULT = 0;// vai trò mặc định của người dùng
        public const int USE_ROLE_SETTING = 1;// tùy chỉnh vai trò riêng cho người dùng
    }
}