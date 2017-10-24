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
    public class SpinGroupWordBusiness : GenericBussiness<SPIN_GROUP_WORD>
    {
        public SpinGroupWordBusiness(Entities context = null)
            : base()
        {
            repository = new SpinGroupWordsRepository(context);
        }

        public void Save(SPIN_GROUP_WORD item)
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

        public JsonResultBO SaveListGroupForUser(int userID, int baivietID, List<int> lstGroup)
        {
            var modelresult = new JsonResultBO(true);
            var query = (
                        from gr_word in this.context.SPIN_GROUP_WORD.Where(x => lstGroup.Contains(x.ID) && x.IS_DELETE != true)
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


            var db = new Entities();
            using (var dbTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in query)
                    {
                        var group = new SPIN_GROUP_WORD();
                        group.IS_GLOBAL = false;
                        group.NAME = item.NAME;
                        group.NGAYTAO = DateTime.Now;
                        group.NGUOITAO = userID;
                        group.FOR_USER = userID;
                        db.SPIN_GROUP_WORD.Add(group);
                        db.SaveChanges();
                        foreach (var itemword in item.LstWords)
                        {
                            var word = new SPIN_WORDS();
                            word.CHUDE = 2;
                            word.ID_USER = userID;
                            word.NGAYTAO = DateTime.Now;
                            word.NGUOITAO = userID;
                            word.NHOMTU_ID = group.ID;
                            word.TU_CUMTU = itemword.TU_CUMTU;
                            db.SPIN_WORDS.Add(word);
                            db.SaveChanges();
                        }

                        var groupbai = new SPIN_BAIVIET_GROUP();
                        groupbai.ID_BAI = baivietID;
                        groupbai.ID_GROUP_TU = group.ID;
                        db.SPIN_BAIVIET_GROUP.Add(groupbai);
                        db.SaveChanges();
                    }
                    dbTransaction.Commit();
                }
                catch 
                {

                    dbTransaction.Rollback();
                    modelresult.Status = false;
                    modelresult.Message = "Không lưu được từ thay thế";
                }
            }
            return modelresult;


        }
        public List<GroupTuDienBO> GetTuDienSys()
        {
            var query = (
                         from gr_word in this.context.SPIN_GROUP_WORD.Where(x => x.IS_DELETE != true && x.IS_GLOBAL == true)
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
                item.Words = string.Join(",", item.LstWords.Select(x => x.TU_CUMTU).ToArray());
            }
            return query;
        }

        public List<GroupTuDienBO> GetByListID(List<int> lstid)
        {
            var query = (
                         from gr_word in this.context.SPIN_GROUP_WORD.Where(x => lstid.Contains(x.ID) && x.IS_DELETE != true && x.IS_GLOBAL == true)
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

            return query;
        }
    }
}
