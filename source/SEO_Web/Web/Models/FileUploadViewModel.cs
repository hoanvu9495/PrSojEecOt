using Model.DBTool;
using System.Collections.Generic;
using Business.CommonBusiness;
namespace Web.Models
{
    public class FileUploadViewModel
    {
        public List<TAILIEUDINHKEM> ListFile { get; set; }
    }
    public class FilesViewModel
    {
        //public List<HSCB_FILES> ListFiles { get; set; }
        public string DUONG_DAN { get; set; }
        public bool DOWLOAD_ONLY { get; set; }
        public string ValidFileExtensions { get; set; }
        public int? CountAllow { get; set; }
        public string MaxSize { get; set; }
        public string NameDisplay { get; set; }
    }
    public class MenuListChild
    {
        public List<ChucNangBO> ListChucNang { get; set; }
        public string TenChucNangCha { get; set; }
    }
}