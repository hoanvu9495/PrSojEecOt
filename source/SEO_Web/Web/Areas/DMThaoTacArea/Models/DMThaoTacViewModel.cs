using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.CommonBusiness;

namespace Web.Areas.DMThaoTacArea.Models
{
    public class DMThaoTacIndexViewModel
    {
        public List<SelectListItem> ListChucNangCap1 { get; set; }
        public List<SelectListItem> ListChucNangCap2 { get; set; }
        public List<DmThaoTacBO> ListThaoTac { get; set; }
        public int CurrentThaoTacCha { get; set; }
    }
    public class DMThaoTacCreateViewModel
    {
        public List<SelectListItem> ListChucNangCap1 { get; set; }
        public List<SelectListItem> ListChucNangCap2 { get; set; }
    }

    public class DMThaoTacEditViewModel
    {
        public List<SelectListItem> ListChucNangCap1 { get; set; }
        public List<SelectListItem> ListChucNangCap2 { get; set; }
        public DmThaoTacBO ThaoTac { get; set; }
    }
}