using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class FileElastic
    {
        public string file { get; set; }
        public string title_file { get; set; }
        public long ID { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string DUONGDAN { get; set; }
        public string DINHDANG { get; set; }
        public string MOTA { get; set; }
    }
}