using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class JsonResultBO
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public JsonResultBO(){

        }
        public JsonResultBO(bool st)
        {
            this.Status = st;
        }

    }
}
