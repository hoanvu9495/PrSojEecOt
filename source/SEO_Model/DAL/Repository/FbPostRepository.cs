using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class FbPostRepository : GenericRepository<FB_POST>
    {
        public FbPostRepository()
            : base()
        {

        }

        public FbPostRepository(Entities context)
            : base(context)
        {

        }
    }
}
