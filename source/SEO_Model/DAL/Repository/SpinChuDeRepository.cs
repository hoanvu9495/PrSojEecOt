using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class SpinChuDeRepository : GenericRepository<SPIN_CHUDE>
    {
        public SpinChuDeRepository()
            : base()
        {

        }

        public SpinChuDeRepository(Entities context)
            : base(context)
        {

        }
    }
}
