using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class TaiLieuDinhKemBO
    {
        public long? TAILIEU_ID { get; set; }
        public string TENTAILIEU { get; set; }
        public DateTime? NGAYTAO { get; set; }
        public string DINHDANG_FILE { get; set; }
        public string HOTEN { get; set; }
        public int? IS_PHEDUYET { get; set; }
        public long? USER_ID { get; set; }
        public long? FOLDER_ID { get; set; }
        public bool? IS_PRIVE { get; set; }
        public string MATAILIEU { get; set; }
        public bool? IS_SHARING { get; set; }
        public string TENTACGIA { get; set; }
        public string TENTHUMUC { get; set; }
        public string VERSION { get; set; }
        public int? TRANGTHAI { get; set; }
        public DateTime? NGAYPHATHANH { get; set; }

    }
}
