using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;
using Business.CommonBusiness;
using PagedList;

namespace Business.Business
{
    public class SpinBaiVietBusiness : GenericBussiness<SPIN_BAIVIET>
    {
        public SpinBaiVietBusiness(Entities context = null)
            : base(context)
        {
            repository = new SpinBaiVietRepository(context);
        }

        public void Save(SPIN_BAIVIET item)
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

        public List<SPIN_BAIVIET> GetListExtend(int baivietID)
        {
            return this.context.SPIN_BAIVIET.Where(x => x.IS_ORIGIN == false && x.EXTEND_OF == baivietID).ToList();
        }

        public BaiVietBO GetByID(int idBaiViet)
        {
            var query = (from bv in this.context.SPIN_BAIVIET.Where(x => x.ID == idBaiViet)
                         select new BaiVietBO()
                            {
                                ID = bv.ID,
                                IS_ORIGIN = bv.IS_ORIGIN,
                                EXTEND_OF = bv.EXTEND_OF,
                                NGAYSUA = bv.NGAYSUA,
                                NGAYTAO = bv.NGAYTAO,
                                NGUOISUA = bv.NGUOISUA,
                                NGUOITAO = bv.NGUOITAO,
                                NOIDUNG = bv.NOIDUNG,
                                TIEUDE = bv.TIEUDE,
                                USER_ID = bv.USER_ID,
                                LstClone = this.context.SPIN_BAIVIET.Where(x => x.IS_ORIGIN == false && x.EXTEND_OF == bv.ID).ToList()
                            }).FirstOrDefault();
            return query;
        }

        public PageListResultBO<BaiVietBO> GetByUser(SearchBaiVietBO searchModel, int userID, int pageIndex = 1, int pageSize = 20)
        {
            var resultModel = new PageListResultBO<BaiVietBO>();

            var queryNvative = this.context.SPIN_BAIVIET.Where(x => x.USER_ID == userID && x.IS_ORIGIN == true);
            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.TieuDe))
                {
                    queryNvative = queryNvative.Where(x => x.TIEUDE.Contains(searchModel.TieuDe));
                }
                if (searchModel.StartNgayTao.HasValue)
                {
                    queryNvative = queryNvative.Where(x => x.NGAYTAO >= searchModel.StartNgayTao);

                }

                if (searchModel.EndNgayTao.HasValue)
                {
                    queryNvative = queryNvative.Where(x => x.NGAYTAO <= searchModel.EndNgayTao);

                }
            }
            var query = (from bv in queryNvative
                         select new BaiVietBO()
                         {
                             ID = bv.ID,
                             IS_ORIGIN = bv.IS_ORIGIN,
                             EXTEND_OF = bv.EXTEND_OF,
                             NGAYSUA = bv.NGAYSUA,
                             NGAYTAO = bv.NGAYTAO,
                             NGUOISUA = bv.NGUOISUA,
                             NGUOITAO = bv.NGUOITAO,
                             NOIDUNG = bv.NOIDUNG,
                             TIEUDE = bv.TIEUDE,
                             USER_ID = bv.USER_ID,
                             LstClone = this.context.SPIN_BAIVIET.Where(x => x.IS_ORIGIN == false && x.EXTEND_OF == bv.ID).ToList()
                         }).OrderByDescending(x => x.ID).ToPagedList(pageIndex, pageSize); ;

            resultModel.Count = query.TotalItemCount;
            resultModel.TotalPage = query.PageCount;
            resultModel.ListItem = query.ToList();
            return resultModel;
        }


    }
}
