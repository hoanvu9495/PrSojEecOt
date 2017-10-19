using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CommonBusiness
{
    public class FileProcessModel
    {
        //Đường dẫn vật lý đến folder chưa file
        public string PathFolder_Real { get; set; }
        //Đường dẫn logic đến folder chứa file
        public string PathFolder_File_Logic { get; set; }
        //Tên file đầy đủ abb.png
        public string FileName { get; set; }
        //Phần mở rộng .png, .jpg
        public string File_Extention { get; set; }
        //Tên của file abb, anhdep
        public string Name_FILE { get; set; }
        //Đường dẫn vật lý của file
        public string PathSaveFile { get; set; }
        //Đường dẫn lưu vào CSDL
        public string PathFileDB { get; set; }

    }
}
