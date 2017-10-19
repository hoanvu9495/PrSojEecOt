using Model.DBTool;
using System.Collections.Generic;
using System.Web.Mvc;
using Business.CommonBusiness;

namespace Web.Areas.DMChucNangArea.Models
{
    public class DMChucNangIndexViewModel
    {
        public List<SelectListItem> ListChucNangCha { get; set; }
        public int CurrentChucNangCha { get; set; }

        public List<DMChucNangVM> ListChucNang { get; set; }
    }
    public class DMChucNangCreateViewModel
    {
        public List<SelectListItem> ListChucNangCha { get; set; }
    }

    public class DMChucNangEditViewModel
    {
        public List<SelectListItem> ListChucNangCha { get; set; }
        public DM_CHUCNANG ChucNang { get; set; }
    }

}