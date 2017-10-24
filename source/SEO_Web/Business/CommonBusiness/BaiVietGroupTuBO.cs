using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace Business.CommonBusiness
{
    public class BaiVietGroupTuBO:SPIN_BAIVIET
    {
        public List<GroupTuDienBO> lstGroup { get; set; }
    }
}
