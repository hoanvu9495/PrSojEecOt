using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Web.Common
{
    public class HiNetGridView : WebGrid
    {
        public HiNetGridView(IEnumerable<dynamic> source = null, IEnumerable<string> columnNames = null, string defaultSort = null, int rowsPerPage = 10, bool canPage = true, bool canSort = true, string ajaxUpdateContainerId = null, string ajaxUpdateCallback = null, string fieldNamePrefix = null, string pageFieldName = null, string selectionFieldName = null, string sortFieldName = null, string sortDirectionFieldName = null)
            : base(source, columnNames, defaultSort, rowsPerPage, canPage, canSort, ajaxUpdateContainerId, ajaxUpdateCallback, fieldNamePrefix, pageFieldName, selectionFieldName, sortFieldName, sortDirectionFieldName)
        {
            //return WebGrid(source, columnNames, defaultSort, rowsPerPage, canPage, canSort, ajaxUpdateContainerId, ajaxUpdateCallback, fieldNamePrefix, pageFieldName, selectionFieldName, sortFieldName, sortDirectionFieldName);
        }

        
        public WebGridColumn[] CusColumns(params WebGridColumn[] columnSet)
        {            
            int i = 0;
            int count = 0;
            foreach(var col in columnSet){
                if(col.ColumnName != null || col.Format != null || col.Header != null){
                    count = count + 1;
                }
            }
            WebGridColumn[] correctColumnSet = new WebGridColumn[count];
            foreach (var col in columnSet)
            {
                if(col.ColumnName != null || col.Format != null || col.Header != null)
                {
                    correctColumnSet[i] = col;
                    i = i + 1;
                }
            }
            return Columns(correctColumnSet);
        }
    }
}