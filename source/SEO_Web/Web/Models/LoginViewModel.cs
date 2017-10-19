using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class LoginViewModel
    {
        public SelectList ListCapDonVi { get; set; }
        public List<SelectListItem> ListCity { get; set; }
        public short? CAPDONVI { get; set; }
        public decimal? TINH { get; set; }
        public decimal? HUYEN { get; set; }
        public decimal? XA { get; set; }
        public decimal? DONVI { get; set; }
    }
}