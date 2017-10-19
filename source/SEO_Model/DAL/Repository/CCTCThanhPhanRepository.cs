using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class CCTCThanhPhanRepository:GenericRepository<CCTC_THANHPHAN>
    {
        public CCTCThanhPhanRepository():base()
        {

        }
        public CCTCThanhPhanRepository(Entities context)
            : base(context)
        {

        }
    }
}
