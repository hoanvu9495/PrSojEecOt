using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;



namespace Web.Areas.BaiVietArea.Models
{
    public class TaoBaiVM
    {
        public SPIN_BAIVIET BaiViet  { get; set; }
        public string ContentEdit { get; set; }
    }
}