using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;
using Business.CommonConstant;
using Business.CommonBusiness;
using System.Web.Mvc;
namespace Business.Business
{
    public class TblTaiLieuKetXuatBusiness : GenericBussiness<TBL_TAILIEU_KETXUAT>
    {
        public TblTaiLieuKetXuatBusiness(Entities context = null)
            : base(context)
        {
            repository = new TblTaiLieuKetXuatRepository(context);
        }
        public void Save(TBL_TAILIEU_KETXUAT item)
        {
            try
            {
                if (item.ID == 0)
                {
                    this.repository.Insert(item);
                }
                else
                    this.repository.Update(item);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public JsonResultBO CheckMaTaiLieu(string maTaiLieu, int id = 0)
        {
            var query = repository.All.Where(x => x.MA_TAILIEU.Equals(maTaiLieu));
            if (id > 0)
            {
                query = query.Where(x => x.ID != id);
            }
            var tailieu = query.FirstOrDefault();
            var model = new JsonResultBO();
            if (tailieu == null)
            {
                model.Status = true;
            }
            else
            {
                model.Status = false;
                model.Message = "Mã tài liệu đã tồn tại.";
            }
            return model;
        }


        public TBL_TAILIEU_KETXUAT GetByMa(string ma)
        {
            var query = this.context.TBL_TAILIEU_KETXUAT.Where(x => x.MA_TAILIEU.Equals(ma));
            return query.FirstOrDefault();

        }
        public int GetSoTaiLieu(int id)
        {
            int sotailieu = 0;
            var query = this.context.TBL_TAILIEU_KETXUAT.Where(x => x.ID == id).FirstOrDefault();
            if (query != null)
            {
                sotailieu = query.SOTAILIEU.GetValueOrDefault(0) + 1;
            }

            return sotailieu;

        }
        public void TangSoTaiLieu(int id)
        {
            var query = this.context.TBL_TAILIEU_KETXUAT.Where(x => x.ID == id).FirstOrDefault();
            if (query != null)
            {
                query.SOTAILIEU += 1;
                Save(query);
                Save();
            }

        }
       
        public List<TaiLieuKetXuatBO> GetList()
        {
            var query = repository.All.Where(x => x.IS_DELETE != true).Select(x => new TaiLieuKetXuatBO()
            {
                ID = x.ID,
                IS_DELETE = x.IS_DELETE,
                NGAYSUA = x.NGAYSUA,
                NGAYTAO = x.NGAYTAO,
                NGUOITAO = x.NGUOITAO,
                NGUOISUA = x.NGUOISUA,
                TENTAILIEU = x.TENTAILIEU,
                MA_TAILIEU = x.MA_TAILIEU,
                URL = x.URL,
            });

            var listData = query.OrderByDescending(x => x.ID).ToList();
            foreach (var item in listData)
            {
                //item.LstStatus = GetListStatus(item.ID);
            }
            return listData;
        }

       


    }
}
