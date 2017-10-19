using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common
{
    public class TENMODULECHUCNANG
    {
        public const string QUANLYTAISAN = "QUẢN LÝ TÀI SẢN";
        public const string QUANLYDAOTAO = "QUẢN LÝ ĐÀO TẠO";
        public const string QUANLYLICHCONGTAC = "QUẢN LÝ LỊCH CÔNG TÁC";
        public const string QUANLYVANGMAT = "QUẢN LÝ VẮNG MẶT";
        public const string QUANLYVIPHAM = "QUẢN LÝ VI PHẠM";
        public const string QUANLYDANGVIEN = "QUẢN LÝ ĐẢNG VIÊN";
        public const string QUANLYDOANVIEN = "QUẢN LÝ ĐOÀN VIÊN";
        public const string QUANLYCONGDOANVIEN = "QUẢN LÝ CÔNG ĐOÀN VIÊN";
        public const string KHENTHUONGCHIBO = "QUẢN LÝ KHEN THƯỞNG CHI BỘ";
        public const string KHENTHUONGCHIDOAN = "QUẢN LÝ KHEN THƯỞNG CHI ĐOÀN";
        public const string KHENTHUONGCONGDOAN = "QUẢN LÝ KHEN THƯỞNG CÔNG ĐOÀN";
        public const string MUCLUCHOSODANGVIEN = "MỤC LỤC HỒ SƠ ĐẢNG VIÊN";
        public const string QLCHIDOAN = "QUẢN LÝ CHI ĐOÀN";
        public const string QLCHIBO = "QUẢN LÝ CHI BỘ";
        public const string QLCONGDOAN = "QUẢN LÝ CÔNG ĐOÀN";
    }
    public class LOAITAILIEU
    {
        /// <summary>
        /// Loại tài liệu: Đơn xin nghỉ
        /// </summary>
        public const int DONXINNGHI = 1;
        /// <summary>
        /// Loại tài liệu: Tài sản
        /// </summary>
        public const int TAISAN = 2;
        /// <summary>
        /// Loại tài liệu: Đăng ký vắng mặt
        /// </summary>
        public const int DANGKYVANGMAT = 3;
        /// <summary>
        /// Loại tài liệu: Văn bản đi
        /// </summary>
        public const int VANBANDI = 4;
        public const int LOG_VANBANDI = 100;
        /// <summary>
        /// Loại tài liệu: Văn bản đến
        /// </summary>
        public const int VANBANDEN = 5;
        public const int YKIEN_VANBANDEN = 101;
        /// <summary>
        /// Loại tài liệu: Văn bản đến nội bộ
        /// </summary>
        public const int VANBANDEN_NOIBO = 500;
        /// <summary>
        /// Báo cáo vắng mặt
        /// </summary>
        public const int BAOCAOVANGMAT = 6;
        /// <summary>
        /// Thông báo kế hoạch nâng lương
        /// </summary>
        public const int TBKH_NANGLUONG = 7;

        /// <summary>
        /// Chung chi dao tao
        /// </summary>
        public const int CHUNGCHIDAOTAO = 8;

        /// Đánh giá đào tạo
        /// </summary>
        public const int DANHGIADAOTAO = 81;
        /// <summary>
        /// Lịch công tác
        /// </summary>
        public const int LICHCONGTAC = 82;

        /// <summary>
        /// Thanh lý tài sản
        /// </summary>
        public const int THANHLYTAISAN = 83;


        /// <summary>
        /// Thanh lý chi bộ
        /// </summary>
        public const int QLCHIBO = 112;
        /// <summary>
        /// Thanh lý chi đoàn
        /// </summary>
        public const int QLCHIDOAN = 113;
        /// <summary>
        /// Thanh lý công đoàn
        /// </summary>
        public const int QLCONGDOAN = 114;

        /// <summary>
        /// Quản lý đoàn viên đảng viên
        /// </summary>
        public const int QUANLYDANGVIEN = 84;
        /// <summary>
        /// Quản lý đoàn viên đảng viên
        /// </summary>
        public const int QUANLYDANGVIENVERSION = 89;

        public const int QUANLYCONGDOANVIEN = 90;
        /// <summary>
        /// Quản lý đoàn viên đoàn viên
        /// </summary>
        public const int QUANLYDOANVIEN = 85;
        /// <summary>
        /// Quản lý khen thưởng chi bộ
        /// </summary>
        public const int KHENTHUONGCHIBO = 86;
        /// <summary>
        /// Quản lý khen thưởng chi đoàn
        /// </summary>
        public const int KHENTHUONGCHIDOAN = 87;
        /// <summary>
        /// Quản lý khen thưởng công đoàn
        /// </summary>
        public const int KHENTHUONGCONGDOAN = 88;
        /// <summary>
        /// Quản lý vi phạm
        /// </summary>
        public const int QUANLYVIPHAM = 9;
        /// <summary>
        /// Đơn xin nâng lương của cán bộ
        /// </summary>
        public const int NLTX_DONNANGLUONG = 10;
        /// <summary>
        /// Kết quả thẩm định của chuyên viên
        /// </summary>
        public const int NLTX_THAMDINH = 11;

        /// <summary>
        /// Công việc
        /// </summary>
        public const int CONGVIEC = 12;
        /// <summary>
        /// Kế hoạch nâng lương thường xuyên
        /// </summary>
        public const int KH_NLTX = 13;
        /// <summary>
        /// Nội dung trao đổi công việc
        /// </summary>
        public const int NOIDUNGTRAODOICONGVIEC = 14;
        /// <summary>
        /// Lùi hạn công việc
        /// </summary>
        public const int LUIHANCONGVIEC = 15;
        /// <summary>
        /// Thư mục upload
        /// </summary>
        public const int TM_UPLOAD = 16;
        /// <summary>
        /// Nâng lương trước thời hạn
        /// </summary>
        public const int NANGLUONGTTH = 17;

        /// <summary>
        /// Loại tài liệu gửi khi chat
        /// </summary>
        public const int CHAT = 18;
        /// <summary>
        /// Đánh dấu đã đọc
        /// </summary>
        public const int CONS_READ = 1;
        /// <summary>
        /// Đánh dấu chưa đọc
        /// </summary>
        public const int CONS_UNREAD = 2;
        /// <summary>
        /// Đánh dấu yêu thích
        /// </summary>
        public const int CONS_FAVORITE = 3;
        /// <summary>
        /// Đánh dấu không thích
        /// </summary>
        public const int CONS_UNFAVORITE = 4;
        /// <summary>
        /// Đánh dấu quan trọng
        /// </summary>
        public const int CONS_IMPORTANT = 5;
        /// <summary>
        /// Đánh dấu không quan trọng
        /// </summary>
        public const int CONS_UNIMPORTANT = 6;

        public const string IMPORTANT = "imp";

        public const string FAVORITE = "fav";
        /// <summary>
        /// Quản lý xe
        /// </summary>
        public const int QLXE = 26;
        /// <summary>
        /// Quản lý phòng họp
        /// </summary>
        public const int QUANLYPHONGHOP = 30;
        /// <summary>
        /// Công văn của đoàn ra
        /// </summary>
        public const int CONGVAN_DOANRA = 19;
        /// <summary>
        /// báo cáo của đoàn ra
        /// </summary>
        public const int BAOCAO_DOANRA = 20;

        /// <summary>
        /// tài liệu đoàn vào
        /// </summary>
        public const int TAILIEU_DOANVAO = 21;

        public const int HOCHIEU = 22;

        public const int VANBAN_HTQT = 24;

        public const int HTQT_HOINHI_HOITHAO = 25;

        public const int HOPTACQUOCTE = 26;

        public const int QUOCGIA_TOCHUC = 50;
        /// <summary>
        /// Dự thảo pháp luật
        /// </summary>
        public const int DUTHAOPHAPLUAT = 27;
        /// <summary>
        /// Đề cương pháp luật
        /// </summary>
        public const int DECUONGPHAPLUAT = 28;
        /// <summary>
        /// Soạn thảo văn bane QPPL
        /// </summary>
        public const int SOANTHAOVANBAN = 29;
        /// <summary>
        /// Lấy ý kiến của các đơn vị liên quan cho dự thảo pháp luật
        /// </summary>
        public const int XINYKIEN = 30;
        /// <summary>
        /// Cho ý kiến dự thảo pháp luật
        /// </summary>
        public const int CHOYKIEN = 31;
        /// <summary>
        /// Tài liệu gửi bộ tư pháp thẩm định dự thảo văn bản
        /// </summary>
        public const int GUITHAMDINH = 32;
        /// <summary>
        /// Trình dự thảo lên bộ trưởng
        /// </summary>
        public const int TRINHBOTRUONG = 33;
        /// <summary>
        /// đơn vị pháp chế gửi trả lời phản hồi
        /// </summary>
        public const int TRALOI_THAMDINH = 34;
        public const int BO_TRALOI = 35;
        /// <summary>
        /// Ảnh đại diện cho tài liệu chưa số hóa
        /// </summary>
        public const int COVER_IMAGE = 36;
        /// <summary>
        /// Biểu mẫu
        /// </summary>
        public const int BieuMau = 37;

        public const int SANGKIEN = 38;

        /// <summary>
        /// Bổ nhiệm lại công chức
        /// </summary>
        public const int BONHIEM_LAI_CONGCHUC = 39;
        /// <summary>
        /// Bổ nhiệm công chức
        /// </summary>
        public const int BONHIEM_CONGCHUC = 40;
        /// <summary>
        /// Chuyển ngạch công chức
        /// </summary>
        public const int CHUYEN_NGACH_CONGCHUC = 41;
        /// <summary>
        /// Nâng ngạch công chức
        /// </summary>
        public const int NANGNGACH_CONGCHUC = 42;
        /// <summary>
        /// Đánh giá công tác hàng năm của công chức
        /// </summary>
        public const int DANHGIA_CONGCHUC = 43;
        /// <summary>
        /// Điều chuyển công chức
        /// </summary>
        public const int DIEUCHUYEN_CONGCHUC = 44;
        /// <summary>
        /// Công văn cử cán bộ
        /// </summary>
        public const int CONGVAN = 45;
        /// <summary>
        /// Công văn đến về việc cử cán bộ/thư mời
        /// </summary>
        public const int INVITATION = 46;
        /// <summary>
        /// Quyết định đoàn ra của Bộ Trưởng
        /// </summary>
        public const int DECISION = 47;
        /// <summary>
        /// Ý kiến chỉ đạo của Lãnh đạo về việc tiếp đoàn/Thư đề nghị
        /// </summary>
        public const int SUGGEST = 48;
        /// <summary>
        /// Báo cáo nội dung tiếp đoàn
        /// </summary>
        public const int REPORT = 49;
        /// <summary>
        /// Ý kiến chỉ đạo của Lãnh đạo/Công văn của Vụ HTQT để thực hiện nội dung tiếp theo
        /// </summary>
        public const int CONGVANVUHTQT = 50;
        public const int KETXUATLUONG = 51;
        //Chi tiết đơn nghỉ
        public const int CHITIET_DONNGHI = 52;
        /// <summary>
        /// Thi đua khen thưởng
        /// </summary>
        public const int ThiDuaKhenThuong = 999;
        public const int TDKT_BAOCAOTHANHTICH = 1000;
        public const int TDKT_CHUYENVIENDONVI_DANHGIA = 1001;
        public const int TDKT_CHUYENVIENCUC_DANHGIA = 1001;

    }
    public class TaiLieuConstant
    {
        public const int DUTHAO = 1;
        public const int TRINHDUYET = 2;
        public const int TRAVE = 3;
        public const int DADUYET = 4;
    }
    public class CapDoNguoiDung
    {
        //Cán bộ bt
        public const int NHANVIEN = 1;
        //Trưởng đơn vị
        public const int DONVI = 2;
        //Cục trưởng
        public const int CUCTRUONG = 3;
        //Chánh Văn phòng
        public const int VANPHONG = 4;
    }

    public class EbizCapDoNguoiDung
    {
        //ebiz Office
        public const int LETAN = 1;
        public const int NHANVIEN = 2;
        public const int THUKY_PHONGBAN = 3;
        public const int TRUONGPHONG = 4;
        public const int TROLY_GIAMDOC = 5;
        public const int GIAMDOC = 6;
    }
    public class MAVAITRO
    {
        public const string CUCTRUONG = "CUCTRUONG";
        public const string CUCPHO = "C_CP";
        public const string PHO_CUCTRUONG = "PCT";
        public const string LANHDAOCUC = "LANHDAOCUC";
        public const string TRUONGDONVI = "TRUONGDONVI";
        public const string PHO_TRUONGDONVI = "PHOTDV";

        //ebiz Office
        public const string NHANVIEN = "NHANVIEN";
        public const string LETAN = "VANTHU";
        public const string THUKY_PHONGBAN = "CONGCHUC";
        public const string TRUONGPHONG = "TRUONGDONVI";
        public const string TROLY_GIAMDOC = "VIENCHUC";
        public const string GIAMDOC = "CUCTRUONG";
        public const string CHUYENVIEN = "CHUYENVIEN";
        public const string VANTHUDONVI = "VTDV";
    }
    public class LOAITAPTHE
    {
        public const int CHIBO = 1;
        public const int CHIDOAN = 2;
        public const int CONGDOAN = 3;
        public const int PHANDOAN = 4;
    }
    public class DANGVIENCONST
    {
        public const int DANGVIEN = 1;
        public const int DOANVIEN = 2;
        public const int CONGDOANVIEN = 3;
        public const int KHEN_THUONG = 1;
        public const int KY_LUAT = 2;
        public const int XEP_LOAI = 3;
    }
    public class HINHTHUC_KHENTHUONG
    {
        public const int KHEN_THUONG = 1;
        public const int KY_LUAT = 2;
        public const int XEP_LOAI = 3;
        public const string KHEN_THUONG_LABEL = "Khen thưởng";
        public const string KY_LUAT_LABEL = "Kỷ luật";
        public const string XEP_LOAI_LABEL = "Xếp loại";
        public string HinhThucLabel(int HinhThuc)
        {
            string label = string.Empty;
            switch (HinhThuc)
            {
                case KHEN_THUONG: label = KHEN_THUONG_LABEL; break;
                case KY_LUAT: label = KY_LUAT_LABEL; break;
                case XEP_LOAI: label = XEP_LOAI_LABEL; break;
            }
            return label;
        }
    }
}