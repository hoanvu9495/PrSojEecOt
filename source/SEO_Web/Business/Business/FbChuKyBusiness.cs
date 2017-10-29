using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;
namespace Business.Business
{
    public class FbChuKyBusiness : GenericBussiness<FB_CHUKY>
    {
        public FbChuKyBusiness(Entities context)
            : base(context)
        {
            repository = new FbChuKyRepository(context);
        }

        public void Save(FB_CHUKY item)
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

        public void HuyChuKyChinh(long id, int userid)
        {
            var query = this.context.FB_CHUKY.Where(x => x.IS_MAIN == true && x.USER_ID == userid&&x.ID!=id).ToList();
            foreach (var item in query)
            {
                item.IS_MAIN = false;
                Save(item);
            }

        }

        public List<FB_CHUKY> GetListByUser(int userID)
        {
            var query = this.context.FB_CHUKY.Where(x => x.USER_ID == userID).OrderByDescending(x=>x.ID).ToList();
            return query;

        }
        public FB_CHUKY GetChuKy(int userID)
        {
            var query = this.context.FB_CHUKY.Where(x => x.USER_ID == userID && x.IS_MAIN==true).FirstOrDefault();
            return query;

        }

    }
}
