using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class SpinBaiVietGroupRepository:GenericRepository<SPIN_BAIVIET_GROUP>
    {
        public SpinBaiVietGroupRepository()
            : base()
        {

        }

        public SpinBaiVietGroupRepository(Entities context)
            : base(context)
        {

        }
    }
}
