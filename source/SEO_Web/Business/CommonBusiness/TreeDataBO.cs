using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Business.CommonBusiness
{
    public class TreeDataBO
    {
        public string Type { get; set; }
        public List<SelectListItem> Item { get; set; }
    }
}
