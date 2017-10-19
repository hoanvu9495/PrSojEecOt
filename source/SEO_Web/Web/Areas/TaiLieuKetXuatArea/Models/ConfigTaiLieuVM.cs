using System;
using System.Collections.Generic;
using System.Linq;
using Model.DBTool;
using System.Web;
using System.Xml.Linq;

namespace Web.Areas.TaiLieuKetXuatArea.Models
{
    public class ConfigTaiLieuVM
    {
        public XElement HtmlString { get; set; }
        public TBL_TAILIEU_KETXUAT TaiLieu { get; set; }
    }
}