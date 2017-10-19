using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class SpinWordsRepository : GenericRepository<SPIN_WORDS>
    {
        public SpinWordsRepository()
            : base()
        {

        }
        public SpinWordsRepository(Entities context)
            : base(context)
        {

        }
    }
}
