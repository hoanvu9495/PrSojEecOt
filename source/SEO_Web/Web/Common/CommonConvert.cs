using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common
{
    public class CommonConvert
    {
        public static string ConvertStatusReport(short? Status)
        {

            string result = "";
            if (Status.HasValue)
            {
                if (Status == 1)
                {
                    result = "Dự thảo";
                }
                if (Status == 2)
                {
                    result = "Đã gửi lên cấp trên";
                }
                if (Status == 3)
                {
                    result = "Đã chốt số liệu";
                }
                if (Status == 4)
                {
                    result = "Yêu cầu tổng hợp lại";
                }
            }
            return result;

        }
        public static string ConvertTypePeriod(short? periodtype)
        {

            string result = "";
            if (periodtype.HasValue)
            {
                if (periodtype == 1)
                {
                    result = "Thực hiện 1 lần";
                }
                if (periodtype == 2)
                {
                    result = "Hàng ngày";
                }
                if (periodtype == 3)
                {
                    result = "Hàng tuần";
                }
                if (periodtype == 4)
                {
                    result = "Hàng tháng";
                }
                if (periodtype == 5)
                {
                    result = "Hàng năm";
                }
            }
            return result;

        }
        public static int ConvertToDay(int Day, int Month, int Year)
        {
            int songay = 0;
            int Thu = 0;
            int[] a = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            songay = ((Year - 1) % 7) * 365 + (Year - 1) / 4;
            /* Do so qua lon nen minh lay phan du luon o day
            khong lam sai thuat toan nhe*/
            if (Year % 4 == 0) a[1] = 29;
            for (int i = 0; i < (Month - 1); i++) songay += a[i];
            songay += Day;
            Thu = songay % 7;
            return Thu;
        }
    }
}