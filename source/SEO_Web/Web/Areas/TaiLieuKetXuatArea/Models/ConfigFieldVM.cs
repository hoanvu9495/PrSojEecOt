using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;
using System.Web.Mvc;

namespace Web.Areas.TaiLieuKetXuatArea.Models
{
    public class ConfigFieldVM
    {
        public TBL_TAILIEU_KETXUAT Tailieu { get; set; }
        public TBL_CONFIG_TAILIEU ConfigField { get; set; }
        public List<SelectListItem> DsColumn { get; set; }
    }
}