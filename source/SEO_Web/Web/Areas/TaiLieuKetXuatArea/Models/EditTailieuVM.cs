using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;
using System.Web.Mvc;
namespace Web.Areas.TaiLieuKetXuatArea.Models
{
    public class EditTailieuVM
    {
        public List<SelectListItem> DsTrangThai { get; set; }
        public TBL_TAILIEU_KETXUAT TaiLieu { get; set; }
    }
}