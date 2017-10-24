using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;

namespace Business.Business
{
    public class SpinWordsBusiness : GenericBussiness<SPIN_WORDS>
    {
        public SpinWordsBusiness(Entities context = null)
            : base()
        {
            repository = new SpinWordsRepository(context);
        }

        public void Save(SPIN_WORDS item)
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

        public List<SPIN_WORDS> getListByGroup(int groupid)
        {
            return this.context.SPIN_WORDS.Where(x => x.NHOMTU_ID == groupid).ToList();
        }
    }
}
