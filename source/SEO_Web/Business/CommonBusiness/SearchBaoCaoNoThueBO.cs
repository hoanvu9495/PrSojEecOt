using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.CommonBusiness
{
    public class SearchBaoCaoNoThueBO
    {
        public int Year { get; set; }
        public List<string> Ky { get; set; }

        public string DoanhNghiep { get; set; }

        public List<string> ToKhai { get; set; }
    }
}
