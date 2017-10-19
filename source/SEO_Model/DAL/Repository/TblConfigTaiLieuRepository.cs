using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class TblConfigTaiLieuRepository : GenericRepository<TBL_CONFIG_TAILIEU>
    {
        public TblConfigTaiLieuRepository()
            : base()
        {

        }

        public TblConfigTaiLieuRepository(Entities context)
            : base(context)
        {

        }
    }
}
