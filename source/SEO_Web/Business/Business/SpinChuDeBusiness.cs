using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;

namespace Business.Business
{
    public class SpinChuDeBusiness : GenericBussiness<SPIN_CHUDE>
    {
        public SpinChuDeBusiness(Entities context = null)
            : base()
        {
            repository = new SpinChuDeRepository(context);
        }

        public void Save(SPIN_CHUDE item)
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
