using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.CommonBusiness;
using DAL.Repository;
using Model.eAita;

namespace Business.Business
{
    public class Htqt_HoChieuBusiness : GenericBussiness<HTQT_HOCHIEU>
    {
        public Htqt_HoChieuBusiness(Entities context = null)
            : base()
        {
            repository = new Htqt_HoChieuRepository(context);
        }

        /// <summary>
        /// hàm thực hiện lưu thông tin hộ chiếu
        /// </summary>
        /// <param name="MucDich"></param>
        public void Save(HTQT_HOCHIEU HoChieu)
        {
            try
            {
                if (HoChieu.ID == 0)
                {
                    this.repository.Insert(HoChieu);
                }
                else
                    this.repository.Update(HoChieu);
                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Htqt_HoChieuBO> GetListAllHoChieu()
        {
            var result = from hochieu in this.context.HTQT_HOCHIEU
                join nguoidung in this.context.DM_NGUOIDUNG on hochieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                orderby hochieu.NGAYTAO descending
                select new Htqt_HoChieuBO()
                {
                    ID = hochieu.ID,
                    SOHOCHIEU = hochieu.SOHOCHIEU,
                    USER_ID = hochieu.USER_ID,
                    HOTEN = nguoidung.HOTEN,
                    NGAYSINH = nguoidung.NGAYSINH,
                    DIACHI = nguoidung.DIACHI,
                    DIENTHOAI = nguoidung.DIENTHOAI,
                    EMAIL = nguoidung.EMAIL,
                    //LOAIHOCHIEU = hochieu.LOAIHOCHIEU,
                    NGAYCAP = hochieu.NGAYCAP,
                    NGAYHETHAN = hochieu.NGAYHETHAN,
                    NGAYTAO = hochieu.NGAYTAO,
                    NGAYSUA = hochieu.NGAYSUA,
                    NGUOITAO = hochieu.NGUOITAO,
                    NGUOISUA = hochieu.NGUOISUA,
                    HAS_FILE = hochieu.HAS_FILE
                };
            return result.ToList();
        }

        public string GetHoSoIDCanBo(int? HOCHIEU_ID)
        {
            var result = string.Empty;
            var model = from hochieu in this.context.HTQT_HOCHIEU
                join nguoidung in this.context.DM_NGUOIDUNG on hochieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                join hscb_nguoidung_canbo in this.context.HSCB_NGUOIDUNG_CANBO on nguoidung.DM_NGUOIDUNG_ID equals
                    hscb_nguoidung_canbo.NGUOIDUNG_ID
                join hscb_congchuc in this.context.HSCB_CONGCHUC_VIENCHUC on hscb_nguoidung_canbo.HOSO_ID equals
                    hscb_congchuc.ID
                where hochieu.ID == HOCHIEU_ID
                select new Htqt_HoChieuBO()
                {
                    USER_ID = hochieu.USER_ID,
                };
            foreach (var item in model)
            {
                result += item.USER_ID + ",";
            }
            return result;
        }

        public Htqt_HoChieuBO GetcChiTietHoChieu(int? ID)
        {
            var result = from hochieu in this.context.HTQT_HOCHIEU
                join nguoidung in this.context.DM_NGUOIDUNG on hochieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID


                where hochieu.ID == ID
                select new Htqt_HoChieuBO()
                {
                    ID = hochieu.ID,
                    SOHOCHIEU = hochieu.SOHOCHIEU,
                    USER_ID = hochieu.USER_ID,
                    HOTEN = nguoidung.HOTEN,
                    //ANH = hochieu.ANH,

                    //LOAIHOCHIEU = hochieu.LOAIHOCHIEU,
                    NGAYCAP = hochieu.NGAYCAP,
                    NGAYHETHAN = hochieu.NGAYHETHAN,
                    NGAYTAO = hochieu.NGAYTAO,
                    NGAYSUA = hochieu.NGAYSUA,
                    NGUOITAO = hochieu.NGUOITAO,
                    NGUOISUA = hochieu.NGUOISUA,
                    HAS_FILE = hochieu.HAS_FILE
                };
            return result.FirstOrDefault();
        }

        public Htqt_HoChieuBO GetDetailHoChieu(int? ID)
        {
            var result = from hochieu in this.context.HTQT_HOCHIEU
                join nguoidung in this.context.DM_NGUOIDUNG on hochieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID

                join hscb_nguoidung_canbo in this.context.HSCB_NGUOIDUNG_CANBO on nguoidung.DM_NGUOIDUNG_ID equals
                    hscb_nguoidung_canbo.NGUOIDUNG_ID

                join hscb_congchuc in this.context.HSCB_CONGCHUC_VIENCHUC on hscb_nguoidung_canbo.HOSO_ID equals
                    hscb_congchuc.ID




                where hochieu.ID == ID
                select new Htqt_HoChieuBO()
                {
                    ID = hochieu.ID,
                    SOHOCHIEU = hochieu.SOHOCHIEU,
                    USER_ID = hochieu.USER_ID,
                    HOTEN = nguoidung.HOTEN,
                    //ANH = hochieu.ANH,
                    SOCHUNGMINHTHU = hscb_congchuc.SO_CMND,
                    NGAYSINH = hscb_congchuc.NGAY_SINH,
                    GIOITINH = hscb_congchuc.GIOI_TINH,
                    NOIDANGKY_HOKHAU = hscb_congchuc.NOI_DANG_KY_HO_KHAU,
                    NOIOHIENNAY = hscb_congchuc.NOI_O_HIEN_NAY,
                    DIENTHOAI = hscb_congchuc.DIENTHOAI,
                    EMAIL = hscb_congchuc.EMAIL,

                    LOAIHOCHIEU = hochieu.LOAIHOCHIEU,
                    NGAYCAP = hochieu.NGAYCAP,
                    NGAYHETHAN = hochieu.NGAYHETHAN,
                    NGAYTAO = hochieu.NGAYTAO,
                    NGAYSUA = hochieu.NGAYSUA,
                    NGUOITAO = hochieu.NGUOITAO,
                    NGUOISUA = hochieu.NGUOISUA,
                    HAS_FILE = hochieu.HAS_FILE
                };
            return result.FirstOrDefault();
        }

        //Chuyển tiếng việt có dấu thành không dấu trong 
        public string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public List<int> GetListJobAvailable(string acc)
        {
            List<int> listJobAvailable = new List<int>();
            List<int> list_congviec = (from hochieu in this.context.HTQT_HOCHIEU
                                       where hochieu.NGUOITAO == acc || hochieu.NGUOISUA == acc
                                       select hochieu.ID).ToList();

            if (list_congviec != null)
            {
                listJobAvailable.AddRange(list_congviec);
            }

            return listJobAvailable;
        }
    }
}
