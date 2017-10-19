using System;
using System.Collections.Generic;
using Model.DBTool;
using AE.Net.Mail;

namespace Business.CommonBusiness
{
    [Serializable]
    public class UserInfoBO
    {
        public decimal UserID;
        public string Username;
        public string Password;
        public string Fullname;
        public int UnitID;
        public string UnitName;
        public int? DmCumThiID;
        public string TenCum;
        public int? DmDiemTNID;
        public string TenDiem;
        //tiennb3 them truong MaDiem + Ten & Ma So GD&DT
        public string MaDiem;
        public string TenSoGDDT;
        public string MaSoGDDT;
        public int ProvinceID;
        public string ProvinceName;
        public int? PhongBanID;
        public string TenDVQL;
        public int? LoaiDVQL;
        public int? DmTruongDHID;
        public string TenTruongDH;
        public int RoleID;
        public string PasswordSalt;
        public DateTime? LastLoginDate;
        public List<ChucNangBO> ListCN;
        public List<ChucNangBO> ListCNFull;
        public List<ThaoTacBO> ListThaoTac;
        public List<int?> ListVaiTro { get; set; }
        public List<UserBO> ListUserBO;
        public string ListUserName { get; set; }
        public decimal PhieuDangKyId;
        public int? CoSoID;
        public int? TinhID;
        public int? HuyenID;
        public long? XaID;
        public int PhanLoaiCoSo;
        public int? HoSo_ID { get; set; }
        public int DonViID;
        public string Email { get; set; }
        public string EmailPass { get; set; }
        public string TenCoSo { get; set; }
        //duynt thêm bậc lương và hệ số lương, chức danh hiện tại
        public string ChucDanh { get; set; }
        public string BacLuong { get; set; }
        public string HeSoLuong { get; set; }
        //end
        //thuynt 
        public int? NguoidungID { get; set; }
        public string ImagesUrl { get; set; }
        public int PhongBan_ID { get; set; }
        public int? CAPCOSO_ID { get; set; }
        public List<int> ListCapCoSo { get; set; }
        public int? OptionRole { get; set; }
        //end
        public List<DM_VAITRO> ListRole { get; set; }
        public List<NGUOIDUNG_VAITRO> ListNguoiDung_Vaitro { get; set; }
        public List<int> ListCoSoRemove { get; set; }
        public ImapClient UserImapClient { get; set; }
        public int? LOAI_HOSO { get; set; }
        public int? VersionHoSo { get; set; }

        public List<int> ListRoleId { get; set; }
    }

    [Serializable]
    public class ChucNangBO
    {
        public int DM_CHUCNANG_ID { get; set; }
        public string TEN_CHUCNANG { get; set; }
        public int? TT_HIENTHI { get; set; }
        public string URL { get; set; }
        public int? CHUCNANG_CHA { get; set; }
        public int? MAC_DINH { get; set; }
        public int? IS_HIDDEN { get; set; }
        public string CSSCLASS { get; set; }
        public string ICONPATH { get; set; }
        public int? VAITROID { get; set; }
        public string MA_CHUCNANG { get; set; }
    }

    [Serializable]
    public class ThaoTacBO
    {
        public string MENU_LINK { get; set; }
        public int DM_CHUCNANG_ID { get; set; }
        public long DM_THAOTAC_ID { get; set; }
        public string TEN_THAOTAC { get; set; }
        public string MA_BUOC { get; set; }
        public int? TT_HIENTHI { get; set; }
        public string THAOTAC { get; set; }
        public List<DateTime?> listThoiGian = new List<DateTime?>();
        public bool? IS_HIENTHI { get; set; }
    }
}
