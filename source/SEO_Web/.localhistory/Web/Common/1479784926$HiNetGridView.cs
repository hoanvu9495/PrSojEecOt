using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Web.Common
{
    public class HiNetGridView : WebGrid
    {
        public WebGridColumn CusColumn(string columnName = null, string header = null, Func<dynamic, object> format = null, string style = null, bool canSort = true, bool isAction = false)
        {
            if (isAction == true)
            {
                var format = format;
                if (format == null)
                {
                    return "";
                }
                else
                {
                    return 
                }
            }
        }
    }
}