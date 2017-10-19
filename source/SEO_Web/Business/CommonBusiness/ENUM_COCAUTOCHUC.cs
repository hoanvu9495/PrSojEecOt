using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Business.CommonBusiness
{
    public class ENUM_COCAUTOCHUC
    {
        
        private string  Name{ get; set; }
        private int Value { get; set; }
      
        public static List<SelectListItem> DSLoaiCoCauToChuc(int id=0)
        {
            List<ENUM_COCAUTOCHUC> ListDS = new List<ENUM_COCAUTOCHUC>();
            ENUM_COCAUTOCHUC TongCongTY = new ENUM_COCAUTOCHUC() { Name = "Tổng công ty", Value = 1 };
            ENUM_COCAUTOCHUC CongTY = new ENUM_COCAUTOCHUC() { Name = "Công ty", Value = 2 };
            ENUM_COCAUTOCHUC PhongBan = new ENUM_COCAUTOCHUC() { Name = "Phòng ban", Value = 3 };
            ListDS.Add(TongCongTY);
            ListDS.Add(CongTY);
            ListDS.Add(PhongBan);

            if (id>0)
            {
                var listResult = ListDS.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Value.ToString(),
                    Selected = x.Value == id
                }).OrderBy(x => x.Value).ToList();
                return listResult;
            }
            else
            {
                return ListDS.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Value.ToString(),
                }).OrderBy(x => x.Value).ToList();
            }
           
           
        }
        
    }
}
