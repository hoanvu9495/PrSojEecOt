using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DBTool;
using Business.Business;
using Web.FwCore;
using System.Web.Mvc;
using System.Text;
using Business.CommonBusiness;
using Html;

namespace Web.Common
{
    public static class GlobalCommon
    {
        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        /// <summary>
        /// Get Cell address of Excel file
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetCellAddress(int row, int col)
        {
            int dividend = col;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName + row;
        }

        public static string GetStatusName(int StatusID)
        {
            if (StatusID.ToString() == UIConstant.COMBOBOX_STATUS_ITEM2_NAME)
            {
                return UIConstant.COMBOBOX_STATUS_ITEM2_VALUE;
            }
            if (StatusID.ToString() == UIConstant.COMBOBOX_STATUS_ITEM3_NAME)
            {
                return UIConstant.COMBOBOX_STATUS_ITEM3_VALUE;
            }
            return "";
        }

        public static string GetLoaiDVQL(int DVQLId)
        {
            if (DVQLId.ToString() == UIConstant.COMBOBOX_DVQL_ITEM0_VALUE)
            {
                return UIConstant.COMBOBOX_DVQL_ITEM0_NAME;
            }
            if (DVQLId.ToString() == UIConstant.COMBOBOX_DVQL_ITEM1_VALUE)
            {
                return UIConstant.COMBOBOX_DVQL_ITEM1_NAME;
            }
            if (DVQLId.ToString() == UIConstant.COMBOBOX_DVQL_ITEM2_VALUE)
            {
                return UIConstant.COMBOBOX_DVQL_ITEM2_NAME;
            }
            return "";
        }

        public static string GetLoaiCumName(int loaiCum)
        {
            if (loaiCum.ToString() == UIConstant.COMBOBOX_LOAICUM_ITEM1_NAME)
            {
                return UIConstant.COMBOBOX_LOAICUM_ITEM1_VALUE;
            }
            if (loaiCum.ToString() == UIConstant.COMBOBOX_LOAICUM_ITEM2_NAME)
            {
                return UIConstant.COMBOBOX_LOAICUM_ITEM2_VALUE;
            }
            return "";
        }
        /// <summary>
        /// Convert guid to short
        /// </summary>
        /// <param name="gooid"></param>
        /// <returns></returns>
        public static string GetGlobalString()
        {
            Guid gooid = Guid.NewGuid();
            string encoded = gooid.ToString();
            encoded = encoded.Replace("/", "").Replace("+", "").Replace("-","");
            return encoded.Substring(0, 10);
        }

        public static MvcHtmlString GetDownloadLink(int? AttachFileID, string Filename)
        {
            if (AttachFileID == null || AttachFileID == 0)
            {
                return new MvcHtmlString("");
            }
            if (AttachFileID > 0)
            {
                return new MvcHtmlString(string.Format("<a href='/AttachFile/Download?FileID={0}' target='_blank'>{1}</a>", AttachFileID, Filename));
            }
            return new MvcHtmlString("");
        }

        public static string GetSafeHtml(string html)
        {
            var sanitizer = new HtmlSanitizer();
            string sanitized = sanitizer.Sanitize(html);
            return sanitized;
        }

        public static MvcHtmlString BuildPageLink(string baseLink, int numOfPage, int currentPage = 1)
        {
            string str = @"
<div class='pagination'>
	<ul style='display: block;float: right;margin-right: 7px;' class=''>
		<li {5}><a style='tfoot' href='{0}?page=1'>&lt;&lt;</a></li>
		<li {5}><a style='tfoot' href='{0}?page={1}'>&lt;</a></li>
		{2}
		<li {6}><a style='tfoot' href='{0}?page={3}'>&gt;</a></li>
		<li {6}><a style='tfoot' href='{0}?page={4}'>&gt;&gt;</a></li>
	</ul>
	<span style='float: right;margin: 5px;'>Trang {7}/{4}</span>
</div>
";
            int prePage = Math.Max(1, currentPage - 1);
            int nextPage = Math.Min(numOfPage, currentPage + 1);
            string links = "";
            int start = Math.Max(1, currentPage - 2);
            int end = start + 4;
            if (end > numOfPage)
            {
                start = Math.Max(1, numOfPage - 4);
                end = numOfPage;
            }
            
            for (int i = start; i <= end; i++)
            {
                if (i == currentPage)
                {
                    links += string.Format("<li style='opacity: 1 !important;' class='disabled'><a style='tfoot' href='{1}?page={0}' class='currentPage'>{0}</a></li>", i, baseLink);
                }
                else
                {
                    links += string.Format("<li><a style='tfoot' href='{1}?page={0}'>{0}</a></li>", i, baseLink);
                }
            }

            string firstStyle = currentPage == 1 ? "class='disabled'" : "";
            string endStyle = currentPage == numOfPage ? "class='disabled'" : "";

            string res = string.Format(str, baseLink, prePage, links, nextPage, numOfPage, firstStyle, endStyle, currentPage);
            return new MvcHtmlString(res);
        }

        //public static string NewJoin(this string str, string separeate, ICollection<string> arr)
        //{
        //    return "";
        //}
    }

    public class SelectListCombo
    {
        public SelectListCombo(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public SelectListCombo()
        {
        }
        /// <summary>
        /// Key of Item (for Display)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// value of Item (for get value)
        /// </summary>
        public string Value { get; set; }
    }
}