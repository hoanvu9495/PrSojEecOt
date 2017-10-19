/**
 * The HiNet License
 *
 * Copyright 2015 Hinet JSC. All rights reserved.
 * HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using Business.CommonBusiness;
using DAL.Repository;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class TaiLieuDinhKemBusiness : GenericBussiness<TAILIEUDINHKEM>
    {
        public TaiLieuDinhKemBusiness(Entities context = null)
            : base()
        {
            repository = new TailieudinhkemRepository(context);
        }

        public bool Save(TAILIEUDINHKEM TAILIEU)
        {
            try
            {
                if (TAILIEU.TAILIEU_ID == 0)
                {
                    this.repository.Insert(TAILIEU);
                }
                else
                {
                    this.repository.Update(TAILIEU);
                }
                this.repository.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // return false;
            }
        }
        /// <summary>
        /// Lay du lieu cho item_ID
        /// </summary>
        /// <param name="ITEM_ID"></param>
        /// <param name="LOAITAI_LIEU"></param>
        /// <returns></returns>
        public List<TAILIEUDINHKEM> GetDataByItemID(long ITEM_ID, int LOAITAI_LIEU)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         where tailieu.ITEM_ID == ITEM_ID && tailieu.LOAI_TAILIEU == LOAITAI_LIEU
                         orderby tailieu.TENTAILIEU
                         select tailieu;
            return result.ToList();
        }
        public List<TaiLieuDinhKemBO> GetDataByItemIDCustom(long ITEM_ID, int LOAITAI_LIEU)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         join nguoidung in this.context.DM_NGUOIDUNG
                         on tailieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         where tailieu.ITEM_ID == ITEM_ID && tailieu.LOAI_TAILIEU == LOAITAI_LIEU
                         orderby tailieu.TENTAILIEU
                         select new TaiLieuDinhKemBO
                         {
                             TAILIEU_ID = tailieu.TAILIEU_ID,
                             DINHDANG_FILE = tailieu.DINHDANG_FILE,
                             NGAYTAO = tailieu.NGAYTAO,
                             TENTAILIEU = tailieu.TENTAILIEU,
                             HOTEN = g1.HOTEN,
                             IS_PHEDUYET = tailieu.IS_PHEDUYET,
                             USER_ID = tailieu.USER_ID,
                             IS_SHARING = tailieu.IS_SHARING
                         };
            return result.ToList();
        }
        public TAILIEUDINHKEM GetDataDESC(int LOAITAILIEU, long ITEM_ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         where tailieu.LOAI_TAILIEU == LOAITAILIEU && tailieu.ITEM_ID == ITEM_ID
                         orderby tailieu.TAILIEU_ID descending
                         select tailieu;
            return result.FirstOrDefault();
        }
        public TAILIEUDINHKEM GetDataDESC(int LOAITAILIEU)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         where tailieu.LOAI_TAILIEU == LOAITAILIEU
                         orderby tailieu.TAILIEU_ID descending
                         select tailieu;
            return result.FirstOrDefault();
        }
        public TaiLieuDinhKemBO GetDataByItemIDCustom(long ITEM_ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         join nguoidung in this.context.DM_NGUOIDUNG
                         on tailieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         where tailieu.TAILIEU_ID == ITEM_ID
                         orderby tailieu.TENTAILIEU
                         select new TaiLieuDinhKemBO
                         {
                             TAILIEU_ID = tailieu.TAILIEU_ID,
                             DINHDANG_FILE = tailieu.DINHDANG_FILE,
                             NGAYTAO = tailieu.NGAYTAO,
                             TENTAILIEU = tailieu.TENTAILIEU,
                             HOTEN = g1.HOTEN,
                             IS_PHEDUYET = tailieu.IS_PHEDUYET,
                             USER_ID = tailieu.USER_ID,
                             IS_SHARING = tailieu.IS_SHARING
                         };
            return result.FirstOrDefault();
        }
        public List<TaiLieuDinhKemBO> GetDataByLoaiTaiLieu(int LOAITAILIEU)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         join nguoidung in this.context.DM_NGUOIDUNG
                         on tailieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         where tailieu.LOAI_TAILIEU == LOAITAILIEU
                         orderby tailieu.TENTAILIEU
                         select new TaiLieuDinhKemBO
                         {
                             TAILIEU_ID = tailieu.TAILIEU_ID,
                             DINHDANG_FILE = tailieu.DINHDANG_FILE,
                             NGAYTAO = tailieu.NGAYTAO,
                             TENTAILIEU = tailieu.TENTAILIEU,
                             HOTEN = g1.HOTEN,
                             IS_PHEDUYET = tailieu.IS_PHEDUYET,
                             USER_ID = tailieu.USER_ID,
                             FOLDER_ID = tailieu.FOLDER_ID,
                             MATAILIEU = tailieu.MATAILIEU,
                             IS_SHARING = tailieu.IS_SHARING,
                             IS_PRIVE = tailieu.IS_PRIVATE
                         };
            return result.ToList();
        }
        public List<TaiLieuDinhKemBO> GetDataByFolder(int LOAITAILIEU, long FOLDER_ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         join nguoidung in this.context.DM_NGUOIDUNG
                         on tailieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         where tailieu.LOAI_TAILIEU == LOAITAILIEU && tailieu.FOLDER_ID == FOLDER_ID
                         orderby tailieu.TENTAILIEU
                         select new TaiLieuDinhKemBO
                         {
                             TAILIEU_ID = tailieu.TAILIEU_ID,
                             DINHDANG_FILE = tailieu.DINHDANG_FILE,
                             NGAYTAO = tailieu.NGAYTAO,
                             TENTAILIEU = tailieu.TENTAILIEU,
                             HOTEN = g1.HOTEN,
                             IS_PHEDUYET = tailieu.IS_PHEDUYET,
                             USER_ID = tailieu.USER_ID,
                             FOLDER_ID = tailieu.FOLDER_ID,
                             MATAILIEU = tailieu.MATAILIEU,
                             IS_SHARING = tailieu.IS_SHARING
                         };
            return result.ToList();
        }
        public List<TaiLieuDinhKemBO> GetDataByFolder(long FOLDER_ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         join nguoidung in this.context.DM_NGUOIDUNG
                         on tailieu.USER_ID equals nguoidung.DM_NGUOIDUNG_ID
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         where tailieu.FOLDER_ID == FOLDER_ID
                         orderby tailieu.TENTAILIEU
                         select new TaiLieuDinhKemBO
                         {
                             TAILIEU_ID = tailieu.TAILIEU_ID,
                             DINHDANG_FILE = tailieu.DINHDANG_FILE,
                             NGAYTAO = tailieu.NGAYTAO,
                             TENTAILIEU = tailieu.TENTAILIEU,
                             HOTEN = g1.HOTEN,
                             IS_PHEDUYET = tailieu.IS_PHEDUYET,
                             USER_ID = tailieu.USER_ID,
                             IS_SHARING = tailieu.IS_SHARING
                         };
            return result.ToList();
        }
        public List<TAILIEUDINHKEM> GetTaiLieuByFolder(long ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         where tailieu.FOLDER_ID == ID
                         select tailieu;
            return result.ToList();
        }
      
        public List<TAILIEUDINHKEM> GetDataByFolder_ID(long? FOLDER_ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         where tailieu.FOLDER_ID == FOLDER_ID
                         select tailieu;
            return result.ToList();
        }
        public List<TAILIEUDINHKEM> GetListByUser(long USER_ID)
        {
            var result = from tailieu in this.context.TAILIEUDINHKEM
                         where tailieu.USER_ID == USER_ID && tailieu.KICHCO.HasValue && tailieu.FOLDER_ID > 0
                         select tailieu;
            return result.ToList();
        }
    }
}
