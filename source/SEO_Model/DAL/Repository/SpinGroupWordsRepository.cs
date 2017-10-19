using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;


namespace DAL.Repository
{
    public class SpinGroupWordsRepository : GenericRepository<SPIN_GROUP_WORD>
    {
        public SpinGroupWordsRepository()
            : base()
        {

        }

        public SpinGroupWordsRepository(Entities context)
            : base(context)
        {

        }
    }
}
