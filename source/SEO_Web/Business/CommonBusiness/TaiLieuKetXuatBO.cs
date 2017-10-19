using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
namespace Business.CommonBusiness
{
    public class TaiLieuKetXuatBO : TBL_TAILIEU_KETXUAT
    {
        public string PathDownload { get; set; }
        public string strNgayTao { get; set; }

        public string LstStatus { get; set; }
    }
}
