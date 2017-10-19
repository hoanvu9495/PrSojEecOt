using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.CommonBusiness
{
    public static class StringVariable
    {
        public static class StatusPDK
        {
            public static readonly string ChuaDuyet = "Chờ duyệt";
            public static readonly string DuyetTTDT = "Đã duyệt thông tin đăng kí dự thi";
            public static readonly string XacMinhDKDT = "Sai thông tin đăng ký dự thi";
            public static readonly string DuyetTTXTN = "Đã duyệt thông tin xét tốt nghiệp";
            public static readonly string Huy = "Hủy";
            public static readonly string XacMinhDKTN = "Sai thông tin xét TN";
        }
        public static class ThaoTacString
        {
            public static readonly string CAP_NHAT = "CẬP NHẬT";
            public static readonly string XOA = "XÓA";
            public static readonly string THEM_MOI = "THÊM MỚI";
            public static readonly string BAO_SAI_SOT = "BÁO SAI SÓT";
            public static readonly string CAPNHAT_DACCACH_TN = "CAPNHAT_DACCACH_TN";
            public static readonly string CAPNHAT_MIENTHI_NN = "CAPNHAT_MIENTHI_NN";
            public static readonly string CAPNHAT_MIENTHI_TN = "CAPNHAT_MIENTHI_TN ";
            public static readonly string CAP_NHAT_CAM_THI = "CAP_NHAT_CAM_THI";
        }

        public static class LoaiThaoTacString
        {
            public static readonly string DIEM_TIEP_NHAN = "ĐIỂM TIẾP NHẬN";
            public static readonly string THI_SINH = "THÍ SINH";
            public static readonly string SO_TIEP_NHAN = "SỞ GD&DT";
            public static readonly string CUM_TIEP_NHAN = "CỤM THI";
            public static readonly string BO = "BỘ";
        }
        public static class TruongTacDong
        {
            public static readonly string DM_CUMTHI_ID = "DM_CUMTHI_ID";
        }
        public static class SortListKey
        {
            public static readonly string sophieu_as = "sophieu";
            public static readonly string sophieu_des = "sophieu_des";
            public static readonly string name_as = "name";
            public static readonly string name_des = "name_des";
            public static readonly string yearG_as = "yearG";
            public static readonly string yearG_des = "yearG_des";
            public static readonly string none = "none";
            public static readonly string diem_as = "diem";
            public static readonly string diem_des = "diem_des";
            public static readonly string sobaodanh_as = "sobaodanh";
            public static readonly string sobaodanh_des = "sobaodanh_des";
        }
        public static string tranformTrangThai(int trangthai)
        {
            switch (trangthai)
            {
                case (int)StatusPDKVal.ChuaDuyet:
                    return StatusPDK.ChuaDuyet;
                case (int)StatusPDKVal.DuyetTTDT:
                    return StatusPDK.DuyetTTDT;
                case (int)StatusPDKVal.DuyetTTXTN:
                    return StatusPDK.DuyetTTXTN;
                case (int)StatusPDKVal.XacMinhDKDT:
                    return StatusPDK.XacMinhDKDT;
                case (int)StatusPDKVal.Huy:
                    return StatusPDK.Huy;
                case (int)StatusPDKVal.XacMinhDKTN:
                    return StatusPDK.XacMinhDKTN;
                default:
                    return "N/A";
            }
        }
        public static string tranformTrangThai(string trangthai)
        {
            int code = Convert.ToInt32(trangthai);
            switch (code)
            {
                case (int)StatusPDKVal.ChuaDuyet:
                    return StatusPDK.ChuaDuyet;
                case (int)StatusPDKVal.DuyetTTDT:
                    return StatusPDK.DuyetTTDT;
                case (int)StatusPDKVal.DuyetTTXTN:
                    return StatusPDK.DuyetTTXTN;
                case (int)StatusPDKVal.XacMinhDKDT:
                    return StatusPDK.XacMinhDKDT;
                case (int)StatusPDKVal.Huy:
                    return StatusPDK.Huy;
                case (int)StatusPDKVal.XacMinhDKTN:
                    return StatusPDK.XacMinhDKTN;
                default:
                    return "N/A";
            }
        }
    }
    public enum StatusPDKVal
    {
        TatCa,
        ChuaDuyet,
        DuyetTTDT,
        Huy,
        XacMinhDKDT,
        DuyetTTXTN,
        XacMinhDKTN
    }

    public static class RoleId
    {
        public const int ADMIN_CUC = 1;
        public const int ADMIN_SO = 2;
        public const int ADMIN_CUM = 3;
        public const int ADMIN_TRUONG = 4;
        public const int THI_SINH = 5;
        public const int ADMIN_DIEM = 6;
    }

    public static class SortListKey
    {
        public static readonly string sophieu_as = "sophieu";
        public static readonly string sophieu_des = "sophieu_des";
        public static readonly string name_as = "name";
        public static readonly string name_des = "name_des";
        public static readonly string yearG_as = "yearG";
        public static readonly string yearG_des = "yearG_des";
        public static readonly string none = "none";
        public static readonly string sobaodanh_as = "sobaodanh";
        public static readonly string sobaodanh_des = "sobaodanh_des";
        public static readonly string diem_as = "diem";
        public static readonly string diem_des = "diem_des";
    }

    public static class ThoiGian
    {
        public static readonly DateTime ThoiGianTitle = new DateTime(DateTime.Now.Year, 05, 28);
        public static readonly DateTime ThoiGianTapTrung = new DateTime(DateTime.Now.Year, 05, 30, 7, 15, 00);
    }
    public static class DanhMucMonHocId
    {
        public static int Toan = 1;
        public static int Van = 2;
        public static int Ly = 3;
        public static int Hoa = 4;
        public static int Sinh = 5;
        public static int Su = 6;
        public static int Dia = 7;
        public static int TiengAnh = 8;
        public static int TiengNga = 9;
        public static int TiengPhap = 10;
        public static int TiengTrung = 11;
        public static int TiengDuc = 12;
        public static int TiengNhat = 13;
    }

    
}