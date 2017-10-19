using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class TreeNodeBO
    {
        public int ID { get; set; }
        public int ID_DM { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public List<TreeNodeBO> Child{ get; set; }
    }
}
