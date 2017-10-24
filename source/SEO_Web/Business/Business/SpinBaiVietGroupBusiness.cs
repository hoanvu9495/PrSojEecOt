using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;
using Business.CommonBusiness;

namespace Business.Business
{
    public class SpinBaiVietGroupBusiness : GenericBussiness<SPIN_BAIVIET_GROUP>
    {
        public SpinBaiVietGroupBusiness(Entities context = null)
            : base()
        {
            repository = new SpinBaiVietGroupRepository(context);
        }

        public void Save(SPIN_BAIVIET_GROUP item)
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
        public SPIN_BAIVIET_GROUP GetNhomTuByBaiViet(long idBaiViet, int nhomtuID)
        {
            var query = this.context.SPIN_BAIVIET_GROUP.Where(x => x.ID_BAI == idBaiViet && x.ID_GROUP_TU == nhomtuID).FirstOrDefault();
            return query;
        }

        public List<GroupTuDienBO> GetTuDienSpin(long idBaiViet)
        {
            var query = (from grp in this.context.SPIN_BAIVIET_GROUP.Where(x => x.ID_BAI == idBaiViet)
                         join gr_word in this.context.SPIN_GROUP_WORD on grp.ID_GROUP_TU equals gr_word.ID
                         select new GroupTuDienBO()
                         {
                             ID = gr_word.ID,
                             FOR_USER = gr_word.FOR_USER,
                             IS_DELETE = gr_word.IS_DELETE,
                             IS_GLOBAL = gr_word.IS_GLOBAL,
                             NAME = gr_word.NAME,
                             NGAYSUA = gr_word.NGAYSUA,
                             NGAYTAO = gr_word.NGAYTAO,
                             NGUOISUA = gr_word.NGUOISUA,
                             NGUOITAO = gr_word.NGUOITAO,
                             LstWords = this.context.SPIN_WORDS.Where(x => x.NHOMTU_ID == gr_word.ID).ToList(),

                         }).OrderByDescending(x => x.ID).ToList();
            foreach (var item in query)
            {
                item.Words = string.Join(",", item.LstWords.Select(x=>x.TU_CUMTU).ToArray());
            }
            return query;
        }

       
    }
}
