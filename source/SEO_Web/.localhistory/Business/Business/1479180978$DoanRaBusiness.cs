using Business.CommonBusiness;
using DAL.Repository;
using Model.eAita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Util;

namespace Business.Business
{

    public class DoanRaBusiness : GenericBussiness<HTQT_DOANRA>
    {
        public DoanRaBusiness(Entities context = null)
            : base()
        {
            repository = new DoanVaoDoanRaRepository(context);
        }
        public void Save(HTQT_DOANRA DoanRa)
        {
            try
            {
                if (DoanRa.ID == 0)
                {
                    this.repository.Insert(DoanRa);
                }
                else
                {
                    this.repository.Update(DoanRa);
                }
                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// lấy toàn bộ danh sách ĐOÀN RA
        /// </summary>
        /// <returns></returns>
        public List<DoanRaBO> GetListAllDoanRa()
        {
            var result = (from doanra in this.context.HTQT_DOANRA
                          //join mucdich in this.context.HTQT_DOANRA_MUCDICH on doanra.MUCDICH_ID equals mucdich.ID
                          join thanhvien in this.context.HTQT_DOANRA_THANHVIEN on doanra.ID equals thanhvien.DOANRA_ID
                          select new DoanRaBO
                          {
                              ID = doanra.ID,
                              TENDOANRA = doanra.TENDOANRA,
                              THOIGIAN_BATDAU = doanra.THOIGIAN_BATDAU,
                              THOIGIAN_KETTHUC = doanra.THOIGIAN_KETTHUC,
                              DIADIEM = doanra.DIADIEM,
                              //MUCDICH = mucdich.TENMUCDICH,
                              BAOCAO = thanhvien.IS_REPORT,
                              NGUOICHOYKIEN = doanra.NGUOICHOYKIEN,
                              HAS_FILE = doanra.HAS_FILE,
                              NGAYTAO = doanra.NGAYTAO
                          }).Distinct().OrderByDescending(bo => bo.NGAYTAO);
            return result.ToList();
        }
        /// <summary>
        /// lấy toàn bộ danh sách đoàn theo mã người dùng đăng nhập
        /// </summary>
        /// <returns></returns>
        public List<DoanRaBO> GetListAllDoanRaForUser(decimal? USER_ID)
        {
            var result = (from doanra in this.context.HTQT_DOANRA
                          //join mucdich in this.context.HTQT_DOANRA_MUCDICH on doanra.MUCDICH_ID equals mucdich.ID
                          join thanhvien in this.context.HTQT_DOANRA_THANHVIEN on doanra.ID equals thanhvien.DOANRA_ID
                          //join nguoidung in this.context.DM_NGUOIDUNG on doanra.NGUOICHOYKIEN_ID equals nguoidung.DM_NGUOIDUNG_ID
                          where thanhvien.USER_ID == USER_ID
                          // orderby doanra.NGAYTAO
                          select new DoanRaBO()
                          {
                              ID = doanra.ID,
                              //THANHVIEN_ID = thanhvien.ID,
                              TENDOANRA = doanra.TENDOANRA,
                              THOIGIAN_BATDAU = doanra.THOIGIAN_BATDAU,
                              THOIGIAN_KETTHUC = doanra.THOIGIAN_KETTHUC,
                              DIADIEM = doanra.DIADIEM,
                              // MUCDICH = mucdich.TENMUCDICH,
                              BAOCAO = thanhvien.IS_REPORT,
                              // NOIDUNGBAOCAO = thanhvien.NOIDUNGBAOCAO,
                              NGAYBAOCAO = thanhvien.NGAYBAOCAO,
                              NGUOICHOYKIEN = doanra.NGUOICHOYKIEN,
                              //IS_DELETE = doanra.IS_DELETE,
                              // HAS_FILE = doanra.HAS_FILE,
                              NGAYTAO = doanra.NGAYTAO
                          }).Distinct().OrderByDescending(x => x.NGAYTAO);
            //.Distinct()
            return result.ToList();
        }
        /// <summary>
        /// lấy thông tin chi tiết đoàn ra được chọn
        /// </summary>
        /// <param name="ID">mã đoàn ra</param>
        /// <returns></returns>
        public DoanRaBO GetDetailDoanRa(int? ID)
        {
            var result = from doanra in this.context.HTQT_DOANRA
                         //join mucdich in this.context.HTQT_DOANRA_MUCDICH on doanra.MUCDICH_ID equals mucdich.ID
                         join nguoidung in this.context.DM_NGUOIDUNG
                         on doanra.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         join thanhvien in this.context.HTQT_DOANRA_THANHVIEN on doanra.ID equals thanhvien.DOANRA_ID
                         where doanra.ID == ID
                         orderby doanra.NGAYTAO
                         select new DoanRaBO
                         {
                             ID = doanra.ID,
                             TENDOANRA = doanra.TENDOANRA,
                             THOIGIAN_BATDAU = doanra.THOIGIAN_BATDAU,
                             THOIGIAN_KETTHUC = doanra.THOIGIAN_KETTHUC,
                             DIADIEM = doanra.DIADIEM,
                             //MUCDICH_ID = mucdich.ID,
                             MUCDICH = doanra.MUCDICH,
                             USER_ID = doanra.USER_ID,
                             // HOVATEN = nguoidung.HOTEN,
                             BAOCAO = thanhvien.IS_REPORT,
                             CHIPHIDAITHO = doanra.CHIPHIDAITHO,
                             DONVIDAITHO = doanra.DONVIDAITHO,
                             CHIPHIPHAITRA = doanra.CHIPHIPHAITRA,
                             DONVICHITRA = doanra.DONVICHITRA,
                             NGUONCHIPHI = doanra.NGUONCHIPHI,
                             YKIENCHIDAO = doanra.YKIENCHIDAO,
                             //NGUOICHOYKIEN_ID = doanra.NGUOICHOYKIEN_ID,
                             NGUOICHOYKIEN = doanra.NGUOICHOYKIEN,
                             NGAYCHOYKIEN = doanra.NGAYCHOYKIEN,
                             NGAYTAO = doanra.NGAYTAO,
                             NGUOITAO = g1.HOTEN,
                             NGAYSUA = doanra.NGAYSUA,
                             NGUOISUA = doanra.NGUOISUA
                         };
            if (ID > 0)
            {
                result = result.Where(d => d.ID == ID);
            }
            return result.FirstOrDefault();
        }
        /// <summary>
        /// lấy toàn bộ danh sách cán bộ theo mã đoàn ra
        /// </summary>
        /// <param name="DOANRA_ID"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetCanBoByDoanRa(int? DOANRA_ID)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var model = from doanra in this.context.HTQT_DOANRA
                        join thanhvien in context.HTQT_DOANRA_THANHVIEN on doanra.ID equals thanhvien.DOANRA_ID
                        join nguoidung in this.context.DM_NGUOIDUNG on thanhvien.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                        where thanhvien.DOANRA_ID == DOANRA_ID
                        select new DoanRaBO()
                        {
                            HOVATEN = nguoidung.HOTEN,
                            USER_ID = nguoidung.DM_NGUOIDUNG_ID
                        };
            var user = string.Empty;
            var userID = string.Empty;
            foreach (var item in model)
            {
                user += item.HOVATEN + ",";
                userID += item.USER_ID + ",";
            }
            result.Add("user", user);
            result.Add("userID", userID);
            return result;
        }
        public List<DoanRaBO> ListCanBo(int? DOANRA_ID)
        {
            var model = from doanra in this.context.HTQT_DOANRA
                        join thanhvien in this.context.HTQT_DOANRA_THANHVIEN on doanra.ID equals thanhvien.DOANRA_ID
                        join nguoidung in this.context.DM_NGUOIDUNG on thanhvien.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                        join donvi in this.context.DM_DONVI on nguoidung.DM_DONVI_ID equals donvi.ID
                        where doanra.ID == DOANRA_ID
                        select new DoanRaBO()
                        {
                            ID = thanhvien.DOANRA_ID,
                            USER_ID = thanhvien.USER_ID,
                            HOVATEN = nguoidung.HOTEN,
                            TEN_DONVI = donvi.TEN_DONVI,
                            BAOCAO = thanhvien.IS_REPORT,
                            NOIDUNGBAOCAO = thanhvien.NOIDUNGBAOCAO
                        };
            return model.ToList();
        }
    }
}

