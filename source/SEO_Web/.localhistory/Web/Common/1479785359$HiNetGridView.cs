using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Web.Common
{
    public class HiNetGridView : WebGrid
    {
        public HiNetGridView(IEnumerable<dynamic> source = null, IEnumerable<string> columnNames = null, string defaultSort = null, int rowsPerPage = 10, bool canPage = true, bool canSort = true, string ajaxUpdateContainerId = null, string ajaxUpdateCallback = null, string fieldNamePrefix = null, string pageFieldName = null, string selectionFieldName = null, string sortFieldName = null, string sortDirectionFieldName = null);
        
        public WebGridColumn CusColumn(string columnName = null, string header = null, Func<dynamic, object> format = null, string style = null, bool canSort = true, bool isAction = false)
        {
            if (isAction == true)
            {
                var cformat = format;
                if (format == null)
                {
                    return new WebGridColumn();
                }
                else
                {
                    return Column(columnName, header, format, style,canSort);
                }
            }
            return Column(columnName, header, format, style, canSort);
        }
    }
}