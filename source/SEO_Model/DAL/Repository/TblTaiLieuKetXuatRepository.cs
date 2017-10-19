using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
namespace DAL.Repository
{
    public class TblTaiLieuKetXuatRepository:GenericRepository<TBL_TAILIEU_KETXUAT>
    {
        public TblTaiLieuKetXuatRepository()
            : base()
        {

        }

        public TblTaiLieuKetXuatRepository(Entities context)
            : base(context)
        {

        }
    }
}
