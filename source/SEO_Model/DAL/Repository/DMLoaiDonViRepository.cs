using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
namespace DAL.Repository
{
    public class DMLoaiDonViRepository:GenericRepository<DM_LOAI_DONVI>
    {
        public DMLoaiDonViRepository()
            : base()
        {

        }

        public DMLoaiDonViRepository(Entities context)
            : base(context)
        {

        }
    }
}
