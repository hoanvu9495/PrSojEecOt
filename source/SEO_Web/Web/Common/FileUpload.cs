using Business.Business;
using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Web.Custom;
using Web.FwCore;
using Business.CommonBusiness;
namespace Web.Common
{

    public class FileUpload
    {
        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="file">FileBase cần lưu</param>
        /// <param name="name">TeenFile Muốn lưu</param>
        /// <param name="folder">Tên folder chứa file trong thư mục upload</param>
        /// <param name="PathSave">Đường dẫn thư mục upload</param>
        /// <returns></returns>
        public static JsonResultBO SaveFile(HttpPostedFileBase file, string name, string extentionList, int? maxSize, string folder, string PathSave)
        {
            var result = new JsonResultBO();
            var fileName = "";

            

         

            #region 1.Kiểm tra có ghi đè tên file không
            if (string.IsNullOrEmpty(name))
            {
                fileName = file.FileName;
            }
            else
            {
                fileName = name;
            }
            #endregion



            var dt = DateTime.Now;
            var arrName = fileName.Split('.');
            var extention = '.' + arrName[arrName.Length - 1];
            var Name_File = string.Join(".", arrName, 0, arrName.Length - 1);

            #region Check extention có hợp lệ không

            if (!string.IsNullOrEmpty(extentionList))
            {
                var listExtention = extentionList.Split(',');
                if (!listExtention.Contains(extention))
                {
                    result.Status = false;
                    result.Message = "Định dạng file không được chấp nhận";
                    return result;
                }
            }
            #endregion

            #region CheckSize
            if (maxSize.HasValue && file.ContentLength > maxSize)
            {
                result.Status = false;
                result.Message = "File vượt quá kích cỡ cho phép";
                return result;
            }
            #endregion

            #region 2. Kiểm tra có lưu thư mục riêng không. Nếu chưa có thì tạo

            var pathFolder = "";
            if (string.IsNullOrEmpty(folder))
            {
                folder = "Unknow";

            }

            pathFolder = Path.Combine(PathSave, folder);



            //Kiểm tra folder đã tồn tại chưa. Nếu chưa tồn tại rồi thì tạo mới
            if (!Directory.Exists(pathFolder))
            {
                try
                {
                    Directory.CreateDirectory(pathFolder);
                }
                catch
                {

                    result.Status = false;
                    result.Message = "Không tạo được folder";
                    return result;
                }

            }

            #endregion

            #region 3.Kiểm tra File đã tồn tại chưa? Nếu tồn tại sửa tên

            var pathFile = Path.Combine(pathFolder, fileName); //Đường đẫn vật lý của file;

            if (File.Exists(pathFile))
            {

                Name_File += string.Format("{0:ddMMyyyy-hhmmss}", dt);
                fileName = Name_File + extention;

                pathFile = Path.Combine(pathFolder, fileName);
            }

            #endregion

            #region 4. Lưu file
            try
            {
                file.SaveAs(pathFile);
                var URLFILE = Path.Combine(folder, fileName);
                result.Status = true;
                result.Message = URLFILE;
            }
            catch
            {
                result.Status = false;
                result.Message = "Không lưu được tài liệu";
                return result;
            }

            #endregion

            return result;
        }
    }
}