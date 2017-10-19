using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Business.CommonBusiness
{
    public class KyConstant
    {
        public static List<SelectListItem> getDS(List<string> lstselected)
        {
            var listdata = new List<SelectListItem>();
            listdata.Add(new SelectListItem() { Text = "01", Value = "01", Selected = lstselected != null && lstselected.Contains("01") });
            listdata.Add(new SelectListItem() { Text = "02", Value = "02", Selected = lstselected != null && lstselected.Contains("02") });
            listdata.Add(new SelectListItem() { Text = "03", Value = "03", Selected = lstselected != null && lstselected.Contains("03") });
            listdata.Add(new SelectListItem() { Text = "04", Value = "04", Selected = lstselected != null && lstselected.Contains("04") });
            listdata.Add(new SelectListItem() { Text = "05", Value = "05", Selected = lstselected != null && lstselected.Contains("05") });
            listdata.Add(new SelectListItem() { Text = "06", Value = "06", Selected = lstselected != null && lstselected.Contains("06") });
            listdata.Add(new SelectListItem() { Text = "07", Value = "07", Selected = lstselected != null && lstselected.Contains("07") });
            listdata.Add(new SelectListItem() { Text = "08", Value = "08", Selected = lstselected != null && lstselected.Contains("08") });
            listdata.Add(new SelectListItem() { Text = "09", Value = "09", Selected = lstselected != null && lstselected.Contains("09") });
            listdata.Add(new SelectListItem() { Text = "10", Value = "10", Selected = lstselected != null && lstselected.Contains("10") });
            listdata.Add(new SelectListItem() { Text = "11", Value = "11", Selected = lstselected != null && lstselected.Contains("11") });
            listdata.Add(new SelectListItem() { Text = "12", Value = "12", Selected = lstselected != null && lstselected.Contains("12") });
            listdata.Add(new SelectListItem() { Text = "Q1", Value = "Q1", Selected = lstselected != null && lstselected.Contains("Q1") });
            listdata.Add(new SelectListItem() { Text = "Q2", Value = "Q2", Selected = lstselected != null && lstselected.Contains("Q2") });
            listdata.Add(new SelectListItem() { Text = "Q3", Value = "Q3", Selected = lstselected != null && lstselected.Contains("Q3") });
            listdata.Add(new SelectListItem() { Text = "Q4", Value = "Q4", Selected = lstselected != null && lstselected.Contains("Q4") });
            listdata.Add(new SelectListItem() { Text = "CN", Value = "CN", Selected = lstselected != null && lstselected.Contains("CN") });
            return listdata;




        }
        public static List<SelectListItem> GetNam(int nam = 0)
        {
            var dt = DateTime.Now;
            var yearstart = dt.Year - 10;
            var listdata = new List<SelectListItem>();
            for (int i = 10; i >= 0; i--)
            {
                var value = i + yearstart;
                listdata.Add(new SelectListItem() { Text = value.ToString(), Value = value.ToString(), Selected = nam > 0 && nam == value });
            }
            return listdata;
        }
    }
}
