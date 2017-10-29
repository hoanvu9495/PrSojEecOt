using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;

namespace Web.Areas.FacebookArea.Models
{
    public class KeThuaBaiVietVM
    {
        public List<FB_POST> LstPost { get; set; }
        public int ViTri { get; set; }
    }
}