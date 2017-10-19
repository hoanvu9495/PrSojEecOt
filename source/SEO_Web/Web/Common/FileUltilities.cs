using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Web.Common
{
    public class FileUltilities
    {
        private string path = WebConfigurationManager.AppSettings["FileUpload"];
        /// <summary>
        /// Tạo thư mục
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CreateFolder(string name)
        {
            bool isCreate = false;

            //string UrlFile = path + name;
            try
            {
                if (!Directory.Exists(name))
                {
                    Directory.CreateDirectory(name);
                    isCreate = true;
                }
            }
            catch
            {
                isCreate = false;
            }
            return isCreate;
        }
        //Xóa thư mục
        public bool RemoveFolder(string name)
        {
            bool isRemove = false;
            //string UrlFile = path + name;
            if (Directory.Exists(name))
            {
                Directory.Delete(name,true);
                isRemove = true;
            }
            return isRemove;
        }
        public void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        //Đổi tên thư mục
        public bool RenameFolder(string oldFolder, string newFolder)
        {
            bool isRename = false;
            string str = path + "\\" + oldFolder;
            if (oldFolder != newFolder)
            {
                if (Directory.Exists(str))
                {
                    Directory.Move(path + "\\" + oldFolder, path + "\\" + newFolder);
                    isRename = true;
                }
            }
            else
            {
                isRename = true;
            }
            return isRename;
        }
        /// <summary>
        /// Di chuyển file
        /// </summary>
        /// <param name="oldFolder">thư mục hiện tại</param>
        /// <param name="newFolder">Thư mục mới</param>
        /// <returns></returns>
        public bool MoveFile(string oldFolder, string newFolder)
        {
            System.IO.File.Move(oldFolder, newFolder);
            return true;
        }
    }
}