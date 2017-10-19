using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace DAL.Repository
{
    public class NguoiDungVaiTroRepository : GenericRepository<NGUOIDUNG_VAITRO>
    {
        public NguoiDungVaiTroRepository() : base()
        {
        }
        public NguoiDungVaiTroRepository(Entities context)
            : base(context)
        {
        }
    }
}