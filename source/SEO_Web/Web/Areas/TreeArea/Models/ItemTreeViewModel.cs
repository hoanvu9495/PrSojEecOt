using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Web.Areas.TreeArea.Models
{
    public class ItemTreeViewModel
    {
        public string Label { get; set; }
        public List<SelectListItem> Child { get; set; }
    }
}