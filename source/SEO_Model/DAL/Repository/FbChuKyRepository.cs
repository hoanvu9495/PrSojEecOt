using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;


namespace DAL.Repository
{
    public class FbChuKyRepository : GenericRepository<FB_CHUKY>
    {
        public FbChuKyRepository()
            : base()
        {

        }

        public FbChuKyRepository(Entities context)
            : base(context)
        {

        }
    }
}
