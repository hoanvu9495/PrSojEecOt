using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class SpinMarkBO
    {
        public string Content { get; set; }
        public Dictionary<string, List<SPIN_WORDS>> Marks { get; set; }
    }
}
