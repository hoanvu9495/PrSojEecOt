using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CommentViewModel
    {
        public string TITLE { get; set; }// TIÊU ĐỀ DIALOG
        public string VALUE { get; set; }// GIÁ TRỊ NHẬN VỀ
        public string FORM_ID { get; set; }// FORM SUBMIT
        public string MESSAGE { get; set; } // THÔNG BÁO KHI SUBMIT THÀNH CÔNG
        public int USER_SUBMIT { get; set; }// 0: KHÔNG SỬ DỤNG SUBMIT FORM - 1: CÓ SỬ DỤNG SUBMIT FORM
        public string CALL_FUNCTION { get; set; } // GỌI FUNTION XỬ LÝ
        public string CLOSE_FUNCTION { get; set; }// HÀM CLOSE
        public int INDEX { get; set; }//CHỈ SỐ
        public string BUTTON_CLICK { get; set; }//BUTTON CLICK
    }
    public class ListLogNhanSuViewModel
    {
        public int object_id { get; set; }
        public string title { get; set; }
        public int type_log { get; set; }
        public string exclude { get; set; }
    }
    public class ObjectData
    {
        public string data { get; set; }
        public string value { get; set; }
    }
    public class BoNhiem
    {
        public string ChucVuCurrent { get; set; }
        public string ListChucVu { get; set; }
    }
}