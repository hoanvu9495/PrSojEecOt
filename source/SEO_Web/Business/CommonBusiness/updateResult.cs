using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
   public class updateResult
    {
       public int successCount { set; get; }
       public Dictionary<int,string> ListError { set; get; }

       public updateResult()
       {
           successCount = 0;
           ListError = new Dictionary<int, string>();
       
       }
    }
}
