using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class SpinBaiVietRepository:GenericRepository<SPIN_BAIVIET>
    {
        public SpinBaiVietRepository()
            : base()
        {

        }

        public SpinBaiVietRepository(Entities context)
            : base(context)
        {

        }
    }
}
