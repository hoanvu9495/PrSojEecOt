using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.NguoiDungArea.Models
{
    public class UserImportModel
    {
        public int excelProductIndex { get; set; }
        public bool excIsValidProduct { get; set; }
        public bool excIsNullUsername { get; set; }
        public bool excIsUsernameExistedInDb { get; set; }
        public string TENDANGNHAP {get;set;}
        public bool excHoTenEmpty{get;set;}
        public string HOTEN{get;set;}
        public int DM_PHONGBAN_ID{get;set;}
        public string PHONGBAN{get;set;}
        public int CHUCVU_ID{get;set;}
        public string CHUCVU {get;set;}
        public string DIENTHOAI{get;set;}
        public string EMAIL{get;set;}
        public int VAITRO_ID { get; set; }
        public string VAITRO { get; set; }
    }
}