using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;

namespace Business.CommonBusiness
{
    public class GroupTuDienBO:SPIN_GROUP_WORD
    {
        public List<SPIN_WORDS> LstWords { get; set; }
        public string Words { get; set; }
    }
}
