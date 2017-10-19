using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;

namespace Business.Business
{
    public class SpinGroupWordBusiness:GenericBussiness<SPIN_GROUP_WORD>
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
    }
}
