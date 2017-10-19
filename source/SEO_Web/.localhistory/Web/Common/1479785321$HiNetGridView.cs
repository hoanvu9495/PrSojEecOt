using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Web.Common
{
    public class HiNetGridView : WebGrid
    {
        public WebGrid(IEnumerable<dynamic> source = null, IEnumerable<string> columnNames = null, string defaultSort = null, int rowsPerPage = 10, bool canPage = true, bool canSort = true, string ajaxUpdateContainerId = null, string ajaxUpdateCallback = null, string fieldNamePrefix = null, string pageFieldName = null, string selectionFieldName = null, string sortFieldName = null, string sortDirectionFieldName = null);

        public string AjaxUpdateCallback { get; }
        public string AjaxUpdateContainerId { get; }
        public IEnumerable<string> ColumnNames { get; }
        public string FieldNamePrefix { get; }
        public bool HasSelection { get; }
        public int PageCount { get; }
        public string PageFieldName { get; }
        public int PageIndex { get; set; }
        public IList<WebGridRow> Rows { get; }
        public int RowsPerPage { get; }
        public int SelectedIndex { get; set; }
        public WebGridRow SelectedRow { get; }
        public string SelectionFieldName { get; }
        public string SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SortDirectionFieldName { get; }
        public string SortFieldName { get; }
        public int TotalRowCount { get; }

        public WebGrid Bind(IEnumerable<dynamic> source, IEnumerable<string> columnNames = null, bool autoSortAndPage = true, int rowCount = -1);
        public WebGridColumn Column(string columnName = null, string header = null, Func<dynamic, object> format = null, string style = null, bool canSort = true);
        public WebGridColumn[] Columns(params WebGridColumn[] columnSet);
        public IHtmlString GetContainerUpdateScript(string path);
        public IHtmlString GetHtml(string tableStyle = null, string headerStyle = null, string footerStyle = null, string rowStyle = null, string alternatingRowStyle = null, string selectedRowStyle = null, string caption = null, bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null, WebGridPagerModes mode = WebGridPagerModes.Numeric | WebGridPagerModes.NextPrevious, string firstText = null, string previousText = null, string nextText = null, string lastText = null, int numericLinksCount = 5, object htmlAttributes = null);
        public string GetPageUrl(int pageIndex);
        public string GetSortUrl(string column);
        public HelperResult Pager(WebGridPagerModes mode = WebGridPagerModes.Numeric | WebGridPagerModes.NextPrevious, string firstText = null, string previousText = null, string nextText = null, string lastText = null, int numericLinksCount = 5);
        public IHtmlString Table(string tableStyle = null, string headerStyle = null, string footerStyle = null, string rowStyle = null, string alternatingRowStyle = null, string selectedRowStyle = null, string caption = null, bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null, Func<dynamic, object> footer = null, object htmlAttributes = null);

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