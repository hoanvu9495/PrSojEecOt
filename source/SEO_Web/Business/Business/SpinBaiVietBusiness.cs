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

        public PageListResultBO<SPIN_BAIVIET> GetListExtend(SearchBaiVietBO searchModel, int baivietID, int pageIndex = 1, int pageSize = 10)
        {
            var result = new PageListResultBO<SPIN_BAIVIET>();
            var pagelist = this.context.SPIN_BAIVIET.Where(x => x.IS_ORIGIN == false && x.EXTEND_OF == baivietID);
            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.TieuDe))
                {
                    pagelist = pagelist.Where(x => x.TIEUDE.Contains(searchModel.TieuDe));
                }

            }
            var resultpagelist = pagelist.OrderByDescending(x => x.ID).ToPagedList(pageIndex, pageSize);
            result.Count = resultpagelist.TotalItemCount;
            result.TotalPage = resultpagelist.PageCount;
            result.ListItem = resultpagelist.ToList();
            return result;
        }

        public List<BaiVietGroupTuBO> GetListBaiVietUser(int userID)
        {
            var query = (from baiviet in this.context.SPIN_BAIVIET.Where(x => x.USER_ID == userID && x.IS_ORIGIN == true)
                        join tbl_group in this.context.SPIN_BAIVIET_GROUP on baiviet.ID equals tbl_group.ID_BAI into gGrouptu
                        select new BaiVietGroupTuBO()
                        {
                            ID = baiviet.ID,
                            TIEUDE = baiviet.TIEUDE,
                            NOIDUNG = baiviet.NOIDUNG,
                            lstGroup = this.context.SPIN_GROUP_WORD.Where(x => gGrouptu.Select(o => o.ID_GROUP_TU).Contains(x.ID)).Select(x => new GroupTuDienBO()
                         {
                             ID = x.ID,
                             FOR_USER = x.FOR_USER,
                             IS_DELETE = x.IS_DELETE,
                             IS_GLOBAL = x.IS_GLOBAL,
                             NAME = x.NAME,
                             NGAYSUA = x.NGAYSUA,
                             NGAYTAO = x.NGAYTAO,
                             NGUOISUA = x.NGUOISUA,
                             NGUOITAO = x.NGUOITAO,
                             LstWords = this.context.SPIN_WORDS.Where(y => y.NHOMTU_ID == x.ID).ToList(),
                           
                         }).OrderByDescending(x => x.ID).ToList()
                        }).OrderByDescending(x => x.ID).ToList();
            foreach (var item in query)
            {
                foreach (var itemword in item.lstGroup)
                {
                    itemword.Words = string.Join(",", itemword.LstWords.Select(x => x.TU_CUMTU).ToArray());
                }
               
            }
            return query;
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
