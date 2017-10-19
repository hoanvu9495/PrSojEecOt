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
        /// <summary>
        /// Loại tài liệu: Văn bản đến
        /// </summary>
        public const int VANBANDEN = 5;
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
        /// Quản lý đoàn viên đảng viên
        /// </summary>
        public const int QUANLYDANGVIEN = 84;
        /// <summary>
        /// Quản lý đoàn viên đoàn viên
        /// </summary>
        public const int QUANLYDOANVIEN = 85;
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
        /// <summary>
        /// Thi đua khen thưởng
        /// </summary>
        public const int ThiDuaKhenThuong = 999;
    }

}