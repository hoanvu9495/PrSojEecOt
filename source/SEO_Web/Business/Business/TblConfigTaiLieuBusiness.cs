using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;

namespace Business.Business
{
    public class TblConfigTaiLieuBusiness : GenericBussiness<TBL_CONFIG_TAILIEU>
    {
        public TblConfigTaiLieuBusiness(Entities context = null)
            : base(context)
        {
            repository = new TblConfigTaiLieuRepository(context);
        }

        public void Save(TBL_CONFIG_TAILIEU item)
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

        public TBL_CONFIG_TAILIEU GetByKey(string key, int tailieuid)
        {
            var query = this.context.TBL_CONFIG_TAILIEU.Where(x => x.ID_TAILIEU == tailieuid && x.FIELD_KEY.Equals(key));
            return query.OrderByDescending(x => x.ID).FirstOrDefault();

        }

        public List<TBL_CONFIG_TAILIEU> GetByTaiLieuID(int taiLieuID)
        {
            var query = this.context.TBL_CONFIG_TAILIEU.Where(x => x.ID_TAILIEU == taiLieuID);
            return query.OrderByDescending(x => x.ID).ToList();

        }
    }
}
