using Business.Business;
using Business.CommonBusiness;
using Model.eAita;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Web.FwCore;
using Web.Models;

namespace Web.Common
{
    public class UploadFileTool
    {
        Entities context = new Entities();
 

        private TaiLieuDinhKemBusiness TaiLieuDinhKemBusiness;
        private string UrlSearch = WebConfigurationManager.AppSettings["TMLTSearch"];
        private string FTPSERVER = WebConfigurationManager.AppSettings["FTPSERVER"];
        private List<string> arrFolder = new List<string>();
        /// <summary>
        /// Hàm upload file
        /// </summary>
        /// <param name="file">file đầu vào</param>+6398+-+
        /// <param name="Is_MultiFile">cho phép multiple file hay không</param>
        /// <param name="extension">chuỗi định dạng file(.jpg,.png,.doc,.pdf)</param>
        /// <param name="path_Folder">Đường dẫn đến folder</param>
        /// <param name="MaxSize">Kích cỡ tối đa của file (0 = unlimited) tính theo byte</param>
        /// <param name="THUMUC_ID">Thư mục tồn tại trên hệ thống</param>
        /// <param name="ITEM_ID">ID của ....(VD đơn xin nghỉ,đơn xin vắng mặt,kế hoạch nâng lương....)</param>
        /// <param name="ITEM_TYPE">kiểu của item chính là LOAI_TAILIEU trong TAILIEUDINHKEM</param>
        /// <returns></returns>
        public bool UploadCustomFile(IEnumerable<HttpPostedFileBase> file, bool Is_MultiFile, string extension, string path_Folder, int MaxSize, string[] THUMUC_ID, string[] filename, long ITEM_ID, int ITEM_TYPE = 1, string moduleName = "Đơn xin nghỉ", UserInfoBO user = null)
        {
            UserInfoBO UserInfo;
            if (user == null)
            {
                UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            }
            else
            {
                UserInfo = user;
            }
            bool IsSave = false;
            var extend = extension.ToListStringLower(',');

            FileUltilities fileulti = new FileUltilities();
            int count = 0;
            int Folder_ID = 0;
            //Load tất cả fileupload            
            if (file != null)
            {
                foreach (HttpPostedFileBase f in file)
                {
                    if (f != null)
                    {
                        if (string.IsNullOrEmpty(filename[count]))
                        {
                            filename[count] = f.FileName.Split('.')[0];
                        }
                        FileInfo info = new FileInfo(f.FileName);
                        bool IsMaxSize = false;
                        if (MaxSize == 0)
                        {
                            IsMaxSize = true;
                        }
                        else if (f.ContentLength < MaxSize)
                        {
                            IsMaxSize = true;
                        }
                        else
                        {
                            IsMaxSize = false;
                        }
                        if (IsMaxSize)
                        {
                            if (!string.IsNullOrEmpty(info.Extension) && extend.Contains(info.Extension.ToLower()) && !string.IsNullOrEmpty(path_Folder))
                            {
                                //Kiểm tra định dạng file và maxsize                                    
                                string url = "";
                                if (THUMUC_ID != null)
                                {
                                    int.TryParse(THUMUC_ID[count], out Folder_ID);
                                }
                                if (Folder_ID > 0)
                                {
                                    arrFolder.Clear();
                                    //Lấy tất cả thư mục cha
                                    THUMUC_LUUTRU THUMUC = ThuMucLuuTruBusiness.Find(Folder_ID);
                                    arrFolder = this.GetAllParent(THUMUC.PARENT_ID);
                                    url = this.GetPath(THUMUC.DONVI_ID, THUMUC.PHONG_ID, THUMUC.COSO_ID);
                                    url = url + "\\eFile";
                                    arrFolder.Reverse();
                                    for (int i = 0; i < arrFolder.Count; i++)
                                    {
                                        url += "\\" + arrFolder[i];
                                    }
                                    //Kiểm tra thư mục tồn tại hay chưa
                                    if (!Directory.Exists(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO))
                                    {
                                        ////Chưa có: tạo mới folder với đường dẫn như trên
                                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO);
                                    }
                                    //Kiểm tra file trùng lặp
                                    string path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName);
                                    string file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName);
                                    if (System.IO.File.Exists(path))
                                    {
                                        filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                                        path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                        file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                    }
                                    f.SaveAs(path);
                                    IsSave = true;
                                    //Chinh sua loai tai lieu dung constant
                                    this.CreateTaiLieuDinhKem(filename[count], ITEM_TYPE, filename[count].Trim() == "" ? f.FileName : filename[count].Trim(), file_path, f.ContentType, (long)UserInfo.UserID, THUMUC.ID, 1, ITEM_ID, f.ContentLength);
                                }
                                else
                                {
                                    //url = "Tài Liệu Lưu Trữ";
                                    url = this.GetPath(UserInfo.DonViID, UserInfo.PhongBanID, UserInfo.CoSoID);
                                    url = url + "\\" + moduleName;
                                    if (Directory.Exists(path_Folder + "\\" + url + "\\" + ITEM_ID))
                                    {
                                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, f.FileName);
                                        string file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, f.FileName);
                                        if (System.IO.File.Exists(path))
                                        {
                                            // filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy hh");
                                            path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                            file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                        }
                                        f.SaveAs(path);
                                        IsSave = true;
                                        this.CreateTaiLieuDinhKem(filename[count], ITEM_TYPE, filename[count].Trim() == "" ? f.FileName : filename[count].Trim(), file_path, info.Extension, (long)UserInfo.UserID, 0, 1, ITEM_ID, f.ContentLength);
                                    }
                                    else
                                    {
                                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + ITEM_ID);
                                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, f.FileName);
                                        string file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, f.FileName);
                                        if (System.IO.File.Exists(path))
                                        {
                                            // filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy hh");
                                            path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                            file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                        }
                                        f.SaveAs(path);
                                        IsSave = true;
                                        this.CreateTaiLieuDinhKem(filename[count], ITEM_TYPE, filename[count].Trim() == "" ? f.FileName : filename[count].Trim(), file_path, f.ContentType, (long)UserInfo.UserID, 0, 1, ITEM_ID, f.ContentLength);
                                    }
                                }
                                //Kiểm tra cho phép multifile (false => thoát)
                                if (!Is_MultiFile)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    count++;
                }
            }
            return IsSave;
        }

        //public bool UploadCustomFileScan(string[] file, bool Is_MultiFile, string extension, string path_Folder, int MaxSize, string[] THUMUC_ID, string[] filename, long ITEM_ID, int ITEM_TYPE = 1, string moduleName = "Đơn xin nghỉ", UserInfoBO user = null)
        //{
        //    var ftp = new FTPProvider(FTPSERVER, "dthang", "ntd", "hinet123");
        //    UserInfoBO UserInfo;
        //    if (user == null)
        //    {
        //        UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
        //    }
        //    else
        //    {
        //        UserInfo = user;
        //    }
        //    ThuMucLuuTruBusiness = new ThuMucLuuTruBusiness(context);
        //    bool IsSave = false;
        //    var extend = extension.ToListStringLower(',');

        //    FileUltilities fileulti = new FileUltilities();
        //    int count = 0;
        //    int Folder_ID = 0;
        //    //Load tất cả fileupload            
        //    if (file != null)
        //    {
        //        foreach (var f in file)
        //        {
        //            if (f != null)
        //            {
        //                var fileSize = ftp.GetSizeFileURL(f);
        //                //if (string.IsNullOrEmpty(filename[count]))
        //                //{
        //                //    filename[count] = f.Split('.')[0];
        //                //}
                        
        //                bool IsMaxSize = false;
        //                if (MaxSize == 0)
        //                {
        //                    IsMaxSize = true;
        //                }
        //                else if (fileSize < MaxSize)
        //                {
        //                    IsMaxSize = true;
        //                }
        //                else
        //                {
        //                    IsMaxSize = false;
        //                }
        //                if (IsMaxSize)
        //                {
        //                    var arrFileName=f.Split('/');

        //                    var extensionFile = '.'+arrFileName[arrFileName.Length - 1].Split('.').LastOrDefault();
        //                    var FileName = arrFileName[arrFileName.Length - 1];
        //                    if (!string.IsNullOrEmpty(extensionFile) && extend.Contains(extensionFile.ToLower()) && !string.IsNullOrEmpty(path_Folder))
        //                    {
        //                        //Kiểm tra định dạng file và maxsize                                    
        //                        string url = "";
        //                        if (THUMUC_ID != null)
        //                        {
        //                            int.TryParse(THUMUC_ID[count], out Folder_ID);
        //                        }
        //                        if (Folder_ID > 0)
        //                        {
        //                            arrFolder.Clear();
        //                            //Lấy tất cả thư mục cha
        //                            THUMUC_LUUTRU THUMUC = ThuMucLuuTruBusiness.Find(Folder_ID);
        //                            arrFolder = this.GetAllParent(THUMUC.PARENT_ID);
        //                            url = this.GetPath(THUMUC.DONVI_ID, THUMUC.PHONG_ID, THUMUC.COSO_ID);
        //                            url = url + "\\eFile";
        //                            arrFolder.Reverse();
        //                            for (int i = 0; i < arrFolder.Count; i++)
        //                            {
        //                                url += "\\" + arrFolder[i];
        //                            }
        //                            //Kiểm tra thư mục tồn tại hay chưa
        //                            if (!Directory.Exists(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO))
        //                            {
        //                                ////Chưa có: tạo mới folder với đường dẫn như trên
        //                                fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO);
        //                            }
        //                            //Kiểm tra file trùng lặp
        //                            string path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, FileName);
        //                            string file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, FileName);
        //                            if (System.IO.File.Exists(path))
        //                            {
        //                                filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        //                                path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + FileName.Split('.')[1]);
        //                                file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + FileName.Split('.')[1]);
        //                            }
        //                            var fileInfo = ftp.GetFileURL(f, path);
        //                            IsSave = true;
        //                            //Chinh sua loai tai lieu dung constant
        //                            this.CreateTaiLieuDinhKem(filename[count].Trim() == "" ? FileName : filename[count].Trim(), ITEM_TYPE, filename[count].Trim() == "" ? FileName : filename[count].Trim(), file_path, extensionFile, (long)UserInfo.UserID, THUMUC.ID, 1, ITEM_ID, fileSize);
        //                        }
        //                        else
        //                        {
        //                            //url = "Tài Liệu Lưu Trữ";
        //                            url = this.GetPath(UserInfo.DonViID, UserInfo.PhongBanID, UserInfo.CoSoID);
        //                            url = url + "\\" + moduleName;
        //                            if (Directory.Exists(path_Folder + "\\" + url + "\\" + ITEM_ID))
        //                            {
        //                                string path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID,FileName);
        //                                string file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, FileName);
        //                                if (System.IO.File.Exists(path))
        //                                {
        //                                    // filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy hh");
        //                                    path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + FileName.Split('.')[1]);
        //                                    file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + FileName.Split('.')[1]);
        //                                }
        //                                var fileInfo = ftp.GetFileURL(f, path);
        //                                IsSave = true;
        //                                this.CreateTaiLieuDinhKem(filename[count].Trim() == "" ? FileName : filename[count].Trim(), ITEM_TYPE, filename[count].Trim() == "" ? FileName : filename[count].Trim(), file_path, fileInfo.Extension, (long)UserInfo.UserID, 0, 1, ITEM_ID, fileSize);
        //                            }
        //                            else
        //                            {
        //                                fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + ITEM_ID);
        //                                string path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, FileName);
        //                                string file_path = Path.Combine("\\" + url + "\\" + ITEM_ID,FileName);
        //                                if (System.IO.File.Exists(path))
        //                                {
        //                                    // filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy hh");
        //                                    path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + FileName.Split('.')[1]);
        //                                    file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + FileName.Split('.')[1]);
        //                                }
        //                                var fileInfo = ftp.GetFileURL(f, path);

        //                                IsSave = true;
        //                                this.CreateTaiLieuDinhKem(filename[count].Trim() == "" ? FileName : filename[count].Trim(), ITEM_TYPE, filename[count].Trim() == "" ? FileName : filename[count].Trim(), file_path, fileInfo.Extension, (long)UserInfo.UserID, 0, 1, ITEM_ID, fileSize);
        //                            }
        //                        }
        //                        //Kiểm tra cho phép multifile (false => thoát)
        //                        if (!Is_MultiFile)
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //            count++;
        //        }
        //    }
        //    return IsSave;
        //}
        public bool UploadCustomFileAndOutPath(IEnumerable<HttpPostedFileBase> file, bool Is_MultiFile, string extension, string path_Folder, int MaxSize, string[] THUMUC_ID, string[] filename, long ITEM_ID, out List<string> OutFilePath, out List<string> OutFileName, out List<string> OutFileExt, out List<long> OutFileID, int ITEM_TYPE = 1, string moduleName = "Đơn xin nghỉ")
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            ThuMucLuuTruBusiness = new ThuMucLuuTruBusiness(context);
            bool IsSave = false;
            var extend = extension.ToListStringLower(',');
            OutFilePath = new List<string>();
            OutFileName = new List<string>();
            OutFileExt = new List<string>();
            OutFileID = new List<long>();
            FileUltilities fileulti = new FileUltilities();
            int count = 0;
            int Folder_ID = 0;
            //Load tất cả fileupload            
            if (file != null)
            {
                foreach (HttpPostedFileBase f in file)
                {

                    if (f != null)
                    {
                        if (string.IsNullOrEmpty(filename[count]))
                        {
                            filename[count] = f.FileName;
                        }
                        FileInfo info = new FileInfo(f.FileName);

                        //Load lan luot định dạng file cho phép
                        bool IsMaxSize = false;
                        if (MaxSize == 0)
                        {
                            IsMaxSize = true;
                        }
                        else if (f.ContentLength < MaxSize)
                        {
                            IsMaxSize = true;
                        }
                        else
                        {
                            IsMaxSize = false;
                        }
                        if (IsMaxSize)
                        {
                            long taiLieuID = 0;
                            var IsValidFile = !string.IsNullOrEmpty(info.Extension) && extend.Contains(info.Extension.ToLower()) && !string.IsNullOrEmpty(path_Folder);
                            if (IsValidFile || string.IsNullOrEmpty(extension))
                            {
                                //Kiểm tra định dạng file và maxsize                                    
                                string url = "";
                                if (THUMUC_ID != null)
                                {
                                    int.TryParse(THUMUC_ID[count], out Folder_ID);
                                }
                                if (Folder_ID > 0)
                                {
                                    arrFolder.Clear();
                                    //Lấy tất cả thư mục cha
                                    THUMUC_LUUTRU THUMUC = ThuMucLuuTruBusiness.Find(Folder_ID);
                                    arrFolder = this.GetAllParent(THUMUC.PARENT_ID);
                                    url = this.GetPath(THUMUC.DONVI_ID, THUMUC.PHONG_ID, THUMUC.COSO_ID);
                                    arrFolder.Reverse();
                                    for (int i = 0; i < arrFolder.Count; i++)
                                    {
                                        url += "\\" + arrFolder[i];
                                    }
                                    //Kiểm tra thư mục tồn tại hay chưa
                                    if (!Directory.Exists(path_Folder + "\\" + url + "\\" + THUMUC.TENTHUMUC))
                                    {
                                        ////Chưa có: tạo mới folder với đường dẫn như trên
                                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + THUMUC.TENTHUMUC);
                                    }
                                    string path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.TENTHUMUC, f.FileName);
                                    string file_path = Path.Combine("\\" + url + "\\" + THUMUC.TENTHUMUC, f.FileName);

                                    f.SaveAs(path);
                                    IsSave = true;
                                    //Chinh sua loai tai lieu dung constant
                                    this.CreateTaiLieuDinhKemOutID(filename[count], ITEM_TYPE, filename[count].Trim() == "" ? f.FileName : filename[count].Trim(), file_path, f.ContentType, (int)UserInfo.UserID, THUMUC.ID, 1, ITEM_ID, out taiLieuID);

                                    OutFilePath.Add("/Uploads/" + url.Replace("\\", "/") + "/" + THUMUC.TENTHUMUC + "/" + f.FileName);
                                    OutFileName.Add(f.FileName);
                                    OutFileExt.Add(info.Extension.ToLower());
                                    OutFileID.Add(taiLieuID);
                                }
                                else
                                {
                                    //url = "Tài Liệu Lưu Trữ";
                                    url = this.GetPath(UserInfo.DonViID, UserInfo.PhongBanID, UserInfo.CoSoID);
                                    if (Directory.Exists(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID))
                                    {
                                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID, f.FileName);
                                        string file_path = Path.Combine("\\" + url + "\\" + moduleName + "\\" + ITEM_ID, f.FileName);
                                        f.SaveAs(path);
                                        IsSave = true;
                                        this.CreateTaiLieuDinhKemOutID(filename[count], ITEM_TYPE, filename[count].Trim() == "" ? f.FileName : filename[count].Trim(), file_path, info.Extension, (int)UserInfo.UserID, 0, 1, ITEM_ID, out taiLieuID);
                                        OutFilePath.Add("/Uploads/" + url.Replace("\\", "/") + "/" + moduleName + "/" + ITEM_ID + "/" + f.FileName);
                                        OutFileName.Add(f.FileName);
                                        OutFileExt.Add(info.Extension.ToLower());
                                        OutFileID.Add(taiLieuID);
                                    }
                                    else
                                    {
                                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID);
                                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID, f.FileName);
                                        string file_path = Path.Combine("\\" + url + "\\" + moduleName + "\\" + ITEM_ID, f.FileName);
                                        f.SaveAs(path);
                                        IsSave = true;
                                        this.CreateTaiLieuDinhKemOutID(filename[count], ITEM_TYPE, filename[count].Trim() == "" ? f.FileName : filename[count].Trim(), file_path, f.ContentType, (int)UserInfo.UserID, 0, 1, ITEM_ID, out taiLieuID);
                                        OutFilePath.Add("/Uploads/" + url.Replace("\\", "/") + "/" + moduleName + "/" + ITEM_ID + "/" + f.FileName);
                                        OutFileName.Add(f.FileName);
                                        OutFileExt.Add(info.Extension.ToLower());
                                        OutFileID.Add(taiLieuID);
                                    }
                                }
                                //Kiểm tra cho phép multifile (false => thoát)
                                if (!Is_MultiFile)
                                {
                                    return true;
                                }
                                //    }
                                //}
                            }
                        }

                    }
                    count++;

                }
            }
            return IsSave;

        }

       
       
        /// <summary>
        /// Thêm mới tài liệu đính kèm
        /// </summary>
        /// <param name="TENTAILIEU">Tên tài liệu do người dùng nhập</param>
        /// <param name="LOAI_TAILIEU">Loại tài liệu</param>
        /// <param name="MOTA">Mô tả cho tài liệu</param>
        /// <param name="DUONGDAN_FILE">Đường dẫn đến file</param>
        /// <param name="DINHDANG_FILE">Định dạng file (.jpg,.pdf,.docx,.doc)</param>
        /// <param name="USER_ID">ID của người dùng</param>
        /// <param name="FOLDER_ID">ID của thư mục được chọn lưu</param>
        /// <param name="IS_ACTIVE">???</param>
        /// <returns></returns>
        private bool CreateTaiLieuDinhKem(string TENTAILIEU, int LOAI_TAILIEU, string MOTA, string DUONGDAN_FILE, string DINHDANG_FILE, long USER_ID, long FOLDER_ID, int IS_ACTIVE, long ITEM_ID, long KICHCO)
        {
            bool IS_Create = false;
            TAILIEUDINHKEM TAILIEU = new TAILIEUDINHKEM();
            TaiLieuDinhKemBusiness = new TaiLieuDinhKemBusiness(context);
            TAILIEU.TENTAILIEU = TENTAILIEU;
            TAILIEU.LOAI_TAILIEU = LOAI_TAILIEU;
            TAILIEU.MOTA = MOTA;
            TAILIEU.DUONGDAN_FILE = DUONGDAN_FILE;
            TAILIEU.DINHDANG_FILE = DINHDANG_FILE;
            TAILIEU.USER_ID = USER_ID;
            TAILIEU.FOLDER_ID = FOLDER_ID;
            TAILIEU.IS_ACTIVE = IS_ACTIVE;
            TAILIEU.SOLUONG_DOWNLOAD = 0;
            TAILIEU.ITEM_ID = ITEM_ID;
            TAILIEU.NGAYTAO = DateTime.Now;
            TAILIEU.DM_LOAITAILIEU_ID = 0;
            //TAILIEU.IS_MODIFY = 0;
            TAILIEU.IS_LOCK = false;
            TAILIEU.NGUOI_LOCK = 0;
            TAILIEU.IS_PHEDUYET = TaiLieuConstant.DADUYET;
            TAILIEU.IS_QLPHIENBAN = false;
            if (TaiLieuDinhKemBusiness.Save(TAILIEU))
            {
                IS_Create = true;
            }

            return IS_Create;
        }
        private bool CreateTaiLieuDinhKemOutID(string TENTAILIEU, int LOAI_TAILIEU, string MOTA, string DUONGDAN_FILE, string DINHDANG_FILE, int USER_ID, long FOLDER_ID, int IS_ACTIVE, long ITEM_ID, out long OutID)
        {
            OutID = 0;
            bool IS_Create = false;
            TAILIEUDINHKEM TAILIEU = new TAILIEUDINHKEM();
            TaiLieuDinhKemBusiness = new TaiLieuDinhKemBusiness(context);
            TAILIEU.TENTAILIEU = TENTAILIEU;
            TAILIEU.LOAI_TAILIEU = LOAI_TAILIEU;
            TAILIEU.MOTA = MOTA;
            TAILIEU.DUONGDAN_FILE = DUONGDAN_FILE;
            TAILIEU.DINHDANG_FILE = DINHDANG_FILE;
            TAILIEU.USER_ID = USER_ID;
            TAILIEU.FOLDER_ID = FOLDER_ID;
            TAILIEU.IS_ACTIVE = IS_ACTIVE;
            TAILIEU.SOLUONG_DOWNLOAD = 0;
            TAILIEU.ITEM_ID = ITEM_ID;
            TAILIEU.NGAYTAO = DateTime.Now;
            TAILIEU.DM_LOAITAILIEU_ID = 0;
            //TAILIEU.IS_MODIFY = 0;
            TAILIEU.IS_LOCK = false;
            TAILIEU.NGUOI_LOCK = 0;
            if (TaiLieuDinhKemBusiness.Save(TAILIEU))
            {
                IS_Create = true;
            }
            OutID = TAILIEU.TAILIEU_ID;
            return IS_Create;
        }
        private long CreateTaiLieuDinhKemver2(string TENTAILIEU, int LOAI_TAILIEU, string MOTA, string DUONGDAN_FILE, string DINHDANG_FILE, long USER_ID, long FOLDER_ID, int IS_ACTIVE, long ITEM_ID, int LOAITAILIE_ID, int IS_PHEDUYET, string CODE, string TACGIA, DateTime? NGAYPHATHANH, bool? IS_PRIVATE, long KICHCO, string VERSION, int TRANGTHAI)
        {

            TAILIEUDINHKEM TAILIEU = new TAILIEUDINHKEM();
            TaiLieuDinhKemBusiness = new TaiLieuDinhKemBusiness(context);
            TAILIEU.TENTAILIEU = TENTAILIEU;
            TAILIEU.LOAI_TAILIEU = LOAI_TAILIEU;
            TAILIEU.MOTA = MOTA;
            TAILIEU.DUONGDAN_FILE = DUONGDAN_FILE;
            TAILIEU.DINHDANG_FILE = DINHDANG_FILE;
            TAILIEU.USER_ID = USER_ID;
            TAILIEU.FOLDER_ID = FOLDER_ID;
            TAILIEU.IS_ACTIVE = IS_ACTIVE;
            TAILIEU.SOLUONG_DOWNLOAD = 0;
            TAILIEU.ITEM_ID = ITEM_ID;
            TAILIEU.NGAYTAO = DateTime.Now;
            TAILIEU.DM_LOAITAILIEU_ID = string.IsNullOrEmpty(LOAITAILIE_ID.ToString()) == true ? 0 : LOAITAILIE_ID;
            TAILIEU.IS_LOCK = false;
            TAILIEU.NGUOI_LOCK = 0;
            TAILIEU.IS_QLPHIENBAN = false;
            TAILIEU.IS_PHEDUYET = IS_PHEDUYET;
            TAILIEU.MATAILIEU = CODE;
            TAILIEU.TENTACGIA = TACGIA;
            TAILIEU.NGAYPHATHANH = NGAYPHATHANH;
            TAILIEU.IS_PRIVATE = IS_PRIVATE;
            TAILIEU.KICHCO = KICHCO;
            TAILIEU.VERSION = VERSION;
            TAILIEU.TRANGTHAI = TRANGTHAI;
            if (TaiLieuDinhKemBusiness.Save(TAILIEU))
            {
                return TAILIEU.TAILIEU_ID;
            }
            return 0;
        }
        //public bool UploadCustomFileVer2(int IS_PHEDUYET, IEnumerable<HttpPostedFileBase> file, bool Is_MultiFile, string extension, string path_Folder, int MaxSize, long THUMUC_ID, string[] filename, long ITEM_ID, string[] LOAITAILIEU, string mota, string[] filecode, string TACGIA, DateTime? NGAYPHATHANH, bool? IS_PRIVATE, string VERSION, int TRANGTHAI, int ITEM_TYPE = 1, string moduleName = "Đơn xin nghỉ")
        //{
        //    UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
        //    ThuMucLuuTruBusiness = new ThuMucLuuTruBusiness(context);
        //    bool IsSave = false;
        //    var extend = extension.ToListStringLower(',');
        //    FileUltilities fileulti = new FileUltilities();
        //    //Load tất cả fileupload            
        //    if (file != null)
        //    {
        //        for (int count = 0; count < file.Count(); count++)
        //        {
        //            if (file.ElementAt(count) != null)
        //            {
        //                if (string.IsNullOrEmpty(filename[count]))
        //                {
        //                    filename[count] = file.ElementAt(count).FileName.Split('.')[0];
        //                }
        //                FileInfo info = new FileInfo(file.ElementAt(count).FileName);
        //                //Load lan luot định dạng file cho phép
        //                if (!string.IsNullOrEmpty(info.Extension) && extend.Contains(info.Extension.ToLower()))
        //                {
        //                    bool IsMaxSize = false;
        //                    if (MaxSize == 0)
        //                    {
        //                        IsMaxSize = true;
        //                    }
        //                    else if (file.ElementAt(count).ContentLength < MaxSize)
        //                    {
        //                        IsMaxSize = true;
        //                    }
        //                    else
        //                    {
        //                        IsMaxSize = false;
        //                    }
        //                    if (IsMaxSize)
        //                    {
        //                        if (!string.IsNullOrEmpty(path_Folder))
        //                        {
        //                            //Kiểm tra định dạng file và maxsize 
        //                            string url = "";
        //                            if (THUMUC_ID > 0)
        //                            {
        //                                arrFolder.Clear();
        //                                //Lấy tất cả thư mục cha
        //                                THUMUC_LUUTRU THUMUC = ThuMucLuuTruBusiness.Find(THUMUC_ID);
        //                                arrFolder = this.GetAllParent(THUMUC.PARENT_ID);
        //                                url = this.GetPath(THUMUC.DONVI_ID, THUMUC.PHONG_ID, THUMUC.COSO_ID);
        //                                url += "\\" + moduleName;
        //                                arrFolder.Reverse();
        //                                for (int i = 0; i < arrFolder.Count; i++)
        //                                {
        //                                    url += "\\" + arrFolder[i];
        //                                }
        //                                //Kiểm tra thư mục tồn tại hay chưa
        //                                if (!Directory.Exists(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO))
        //                                {
        //                                    ////Chưa có: tạo mới folder với đường dẫn như trên
        //                                    fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO);
        //                                }
        //                                //Kiểm tra file trùng lặp
        //                                string path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName);
        //                                string file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName);
        //                                if (System.IO.File.Exists(path))
        //                                {
        //                                    if (!string.IsNullOrEmpty(filename[count]))
        //                                    {
        //                                        filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        //                                    }
        //                                    path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
        //                                    file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
        //                                }
        //                                file.ElementAt(count).SaveAs(path);
        //                                IsSave = true;
        //                                long ID = this.CreateTaiLieuDinhKemver2(filename[count], ITEM_TYPE, mota == "" ? filename[count] : mota, file_path, file.ElementAt(count).ContentType, (int)UserInfo.UserID, THUMUC.ID, 1, ITEM_ID, LOAITAILIEU[count].ToIntOrZero(), IS_PHEDUYET, filecode[count], TACGIA, NGAYPHATHANH, IS_PRIVATE, file.ElementAt(count).ContentLength, VERSION, TRANGTHAI);
        //                                // this.ElasticInsert(file_path, ID, file.ElementAt(count).ContentType, mota == "" ? filename[count] : mota, filename[count]);
        //                            }
        //                            if (!Is_MultiFile)
        //                            {
        //                                return true;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return IsSave;

        //}
        //public bool UploadSingleFile(int PHEDUYET, UserInfoBO user, IEnumerable<HttpPostedFileBase> file, string path_Folder, long ITEM_ID, string mota, int ITEM_TYPE = 1, string moduleName = "Hình đại diện tài liệu")
        //{
        //    UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
        //    ThuMucLuuTruBusiness = new ThuMucLuuTruBusiness(context);
        //    bool IsSave = false;
        //    FileUltilities fileulti = new FileUltilities();
        //    if (file != null)
        //    {
        //        for (int count = 0; count < file.Count(); count++)
        //        {
        //            if (file.ElementAt(count) != null)
        //            {
        //                FileInfo info = new FileInfo(file.ElementAt(count).FileName);
        //                string url = "";
        //                if (!string.IsNullOrEmpty(path_Folder))
        //                {

        //                    url = this.GetPath(UserInfo.DonViID, UserInfo.PhongBanID, UserInfo.CoSoID);
        //                    //Kiểm tra folder đã tồn tại hay chưa
        //                    if (Directory.Exists(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID))
        //                    {
        //                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName);
        //                        string file_path = Path.Combine("\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName);
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            path = Path.Combine(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
        //                            file_path = Path.Combine("\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
        //                        }

        //                        file.ElementAt(count).SaveAs(path);
        //                        IsSave = true;
        //                        this.CreateTaiLieuDinhKem(file.ElementAt(count).FileName, ITEM_TYPE, file.ElementAt(count).FileName, file_path, file.ElementAt(count).ContentType, (long)user.UserID, ITEM_ID, 1, ITEM_ID, file.ElementAt(count).ContentLength);
        //                    }
        //                    else
        //                    {
        //                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID);
        //                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName);
        //                        string file_path = Path.Combine("\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName);
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            path = Path.Combine(path_Folder + "\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
        //                            file_path = Path.Combine("\\" + url + "\\" + moduleName + "\\" + ITEM_ID, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
        //                        }
        //                        file.ElementAt(count).SaveAs(path);
        //                        IsSave = true;
        //                        this.CreateTaiLieuDinhKem(file.ElementAt(count).FileName, ITEM_TYPE, file.ElementAt(count).FileName, file_path, file.ElementAt(count).ContentType, (long)user.UserID, ITEM_ID, 1, ITEM_ID, file.ElementAt(count).ContentLength);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    return IsSave;
        //}

        ////private void ElasticInsert(string DUONGDAN, long ID, string DINHDANG, string MOTA, string TENFILE)
        ////{
        ////    bool IsElastic = false;
        ////    switch (DINHDANG)
        ////    {
        ////        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
        ////            IsElastic = true;
        ////            break;
        ////        case "application/vnd.ms-word.document.12":
        ////            IsElastic = true;
        ////            break;
        ////        case "application/msword":
        ////            IsElastic = true;
        ////            break;
        ////        case ".doc":
        ////            IsElastic = true;
        ////            break;
        ////        case ".docx":
        ////            IsElastic = true;
        ////            break;
        ////        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
        ////            IsElastic = true;
        ////            break;
        ////        case "application/vnd.ms-excel":
        ////            IsElastic = true;
        ////            break;
        ////        case ".xls":
        ////            IsElastic = true;
        ////            break;
        ////        case ".xlsx":
        ////            IsElastic = true;
        ////            break;
        ////        case "application/pdf":
        ////            IsElastic = true;
        ////            break;
        ////        case ".pdf":
        ////            IsElastic = true;
        ////            break;

        ////        case "text/plain":
        ////            IsElastic = true;
        ////            break;
        ////        case "txt":
        ////            IsElastic = true;
        ////            break;
        ////        default:
        ////            IsElastic = false;
        ////            break;
        ////    }
        ////    if (IsElastic)
        ////    {
        ////        var local = new Uri(UrlSearch);
        ////        if (Uri.TryCreate(UrlSearch, UriKind.Absolute, out local))
        ////        {
        ////            var settings = new ConnectionConfiguration(local);
        ////            var elastic = new ElasticLowLevelClient(settings);
        ////            string template_path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + DUONGDAN);
        ////            byte[] bytes = System.IO.File.ReadAllBytes(template_path);
        ////            string content = System.IO.File.ReadAllText(template_path);
        ////            FileElastic fileelas = new FileElastic
        ////            {
        ////                title_file = TENFILE,
        ////                file = System.Convert.ToBase64String(bytes),
        ////                //ID = ID,
        ////                //MOTA = MOTA,
        ////                //DUONGDAN = DUONGDAN,
        ////                //DINHDANG = DINHDANG,
        ////                //NGAYTAO = DateTime.Now
        ////            };
        ////            var index = elastic.Index<FileElastic>("test", "document", fileelas);
        ////        }

        ////    }
        //}
        //public bool UploadCustomSingleFile(HttpPostedFileBase file, string extension, string path_Folder, int MaxSize, string THUMUC_ID, string filename, long ITEM_ID, int ITEM_TYPE = 1, string moduleName = "Đơn xin nghỉ", UserInfoBO user = null)
        //{
        //    UserInfoBO UserInfo;
        //    if (user == null)
        //    {
        //        UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
        //    }
        //    else
        //    {
        //        UserInfo = user;
        //    }
        //    ThuMucLuuTruBusiness = new ThuMucLuuTruBusiness(context);
        //    bool IsSave = false;
        //    var extend = extension.ToListStringLower(',');

        //    FileUltilities fileulti = new FileUltilities();
        //    int Folder_ID = 0;
        //    //Load tất cả fileupload            
        //    if (file != null)
        //    {


        //        FileInfo info = new FileInfo(file.FileName);

        //        bool IsMaxSize = false;
        //        if (MaxSize == 0)
        //        {
        //            IsMaxSize = true;
        //        }
        //        else if (file.ContentLength < MaxSize)
        //        {
        //            IsMaxSize = true;
        //        }
        //        else
        //        {
        //            IsMaxSize = false;
        //        }
        //        if (IsMaxSize)
        //        {
        //            if (!string.IsNullOrEmpty(info.Extension) && extend.Contains(info.Extension.ToLower()) && !string.IsNullOrEmpty(path_Folder))
        //            {
        //                //Kiểm tra định dạng file và maxsize                                    
        //                string url = "";
        //                if (THUMUC_ID != null)
        //                {
        //                    int.TryParse(THUMUC_ID, out Folder_ID);
        //                }
        //                if (Folder_ID > 0)
        //                {
        //                    arrFolder.Clear();
        //                    //Lấy tất cả thư mục cha
        //                    THUMUC_LUUTRU THUMUC = ThuMucLuuTruBusiness.Find(Folder_ID);
        //                    arrFolder = this.GetAllParent(THUMUC.PARENT_ID);
        //                    url = this.GetPath(THUMUC.DONVI_ID, THUMUC.PHONG_ID, THUMUC.COSO_ID);
        //                    url = url + "\\eFile";
        //                    arrFolder.Reverse();
        //                    for (int i = 0; i < arrFolder.Count; i++)
        //                    {
        //                        url += "\\" + arrFolder[i];
        //                    }
        //                    //Kiểm tra thư mục tồn tại hay chưa
        //                    if (!Directory.Exists(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO))
        //                    {
        //                        ////Chưa có: tạo mới folder với đường dẫn như trên
        //                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO);
        //                    }
        //                    //Kiểm tra file trùng lặp
        //                    string path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.FileName);
        //                    string file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.FileName);
        //                    if (System.IO.File.Exists(path))
        //                    {
        //                        filename = filename + DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        //                        path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.FileName.Split('.')[1]);
        //                        file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.FileName.Split('.')[1]);
        //                    }
        //                    file.SaveAs(path);
        //                    IsSave = true;
        //                    //Chinh sua loai tai lieu dung constant
        //                    this.CreateTaiLieuDinhKem(filename, ITEM_TYPE, filename == null ? file.FileName : filename.Trim(), file_path, file.ContentType, (long)UserInfo.UserID, THUMUC.ID, 1, ITEM_ID, file.ContentLength);
        //                }
        //                else
        //                {
        //                    //url = "Tài Liệu Lưu Trữ";
        //                    url = this.GetPath(UserInfo.DonViID, UserInfo.PhongBanID, UserInfo.CoSoID);
        //                    url = url + "\\" + moduleName;
        //                    if (Directory.Exists(path_Folder + "\\" + url + "\\" + ITEM_ID))
        //                    {
        //                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, file.FileName);
        //                        string file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, file.FileName);
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            // filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy hh");
        //                            path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, file.FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.FileName.Split('.')[1]);
        //                            file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, file.FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.FileName.Split('.')[1]);
        //                        }
        //                        file.SaveAs(path);
        //                        IsSave = true;
        //                        this.CreateTaiLieuDinhKem(filename, ITEM_TYPE, filename == null ? file.FileName : filename.Trim(), file_path, info.Extension, (long)UserInfo.UserID, 0, 1, ITEM_ID, file.ContentLength);
        //                    }
        //                    else
        //                    {
        //                        fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + ITEM_ID);
        //                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, file.FileName);
        //                        string file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, file.FileName);
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            // filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy hh");
        //                            path = Path.Combine(path_Folder + "\\" + url + "\\" + ITEM_ID, file.FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.FileName.Split('.')[1]);
        //                            file_path = Path.Combine("\\" + url + "\\" + ITEM_ID, file.FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.FileName.Split('.')[1]);
        //                        }
        //                        file.SaveAs(path);
        //                        IsSave = true;
        //                        this.CreateTaiLieuDinhKem(filename, ITEM_TYPE, filename == null ? file.FileName : filename.Trim(), file_path, file.ContentType, (long)UserInfo.UserID, 0, 1, ITEM_ID, file.ContentLength);
        //                    }
        //                }
        //                //Kiểm tra cho phép multifile (false => thoát)
                       
        //            }
        //        }

        //    }
        //    return IsSave;
        //}

        public List<long> UploadCustomFileVer2(int IS_PHEDUYET, IEnumerable<HttpPostedFileBase> file, bool Is_MultiFile, string extension, string path_Folder, int MaxSize, long THUMUC_ID, string[] filename, long ITEM_ID, string[] LOAITAILIEU, string mota, string[] filecode, string TACGIA, DateTime? NGAYPHATHANH, bool? IS_PRIVATE, string VERSION, int TRANGTHAI, int ITEM_TYPE = 1, string moduleName = "Đơn xin nghỉ")
        {
            List<long> ListTaiLieu = new List<long>();
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            ThuMucLuuTruBusiness = new ThuMucLuuTruBusiness(context);
            var extend = extension.ToListStringLower(',');
            FileUltilities fileulti = new FileUltilities();
            //Load tất cả fileupload            
            if (file != null)
            {
                if(LOAITAILIEU == null){
                    LOAITAILIEU = new string[file.Count()];
                }
                for (int count = 0; count < file.Count(); count++)
                {
                    if (file.ElementAt(count) != null)
                    {
                        if (string.IsNullOrEmpty(filename[count]))
                        {
                            filename[count] = file.ElementAt(count).FileName.Split('.')[0];
                        }
                        FileInfo info = new FileInfo(file.ElementAt(count).FileName);
                        //Load lan luot định dạng file cho phép
                        if (!string.IsNullOrEmpty(info.Extension) && extend.Contains(info.Extension.ToLower()))
                        {
                            bool IsMaxSize = false;
                            if (MaxSize == 0)
                            {
                                IsMaxSize = true;
                            }
                            else if (file.ElementAt(count).ContentLength < MaxSize)
                            {
                                IsMaxSize = true;
                            }
                            else
                            {
                                IsMaxSize = false;
                            }
                            if (IsMaxSize)
                            {
                                if (!string.IsNullOrEmpty(path_Folder))
                                {
                                    //Kiểm tra định dạng file và maxsize 
                                    string url = "";
                                    if (THUMUC_ID > 0)
                                    {
                                        arrFolder.Clear();
                                        //Lấy tất cả thư mục cha
                                        THUMUC_LUUTRU THUMUC = ThuMucLuuTruBusiness.Find(THUMUC_ID);
                                        arrFolder = this.GetAllParent(THUMUC.PARENT_ID);
                                        url = this.GetPath(THUMUC.DONVI_ID, THUMUC.PHONG_ID, THUMUC.COSO_ID);
                                        url += "\\" + moduleName;
                                        arrFolder.Reverse();
                                        for (int i = 0; i < arrFolder.Count; i++)
                                        {
                                            url += "\\" + arrFolder[i];
                                        }
                                        //Kiểm tra thư mục tồn tại hay chưa
                                        if (!Directory.Exists(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO))
                                        {
                                            ////Chưa có: tạo mới folder với đường dẫn như trên
                                            fileulti.CreateFolder(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO);
                                        }
                                        //Kiểm tra file trùng lặp
                                        string path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName);
                                        string file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName);
                                        if (System.IO.File.Exists(path))
                                        {
                                            if (!string.IsNullOrEmpty(filename[count]))
                                            {
                                                filename[count] = filename[count] + DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                                            }
                                            path = Path.Combine(path_Folder + "\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                            file_path = Path.Combine("\\" + url + "\\" + THUMUC.THUMUC_AO, file.ElementAt(count).FileName.Split('.')[0] + "-" + DateTime.Now.ToString("dd-MM-yyyy hh") + "." + file.ElementAt(count).FileName.Split('.')[1]);
                                        }
                                        file.ElementAt(count).SaveAs(path);

                                        long ID = this.CreateTaiLieuDinhKemver2(filename[count], ITEM_TYPE, mota == "" ? filename[count] : mota, file_path, file.ElementAt(count).ContentType, (long)UserInfo.UserID, THUMUC.ID, 1, ITEM_ID, LOAITAILIEU[count].ToIntOrZero(), IS_PHEDUYET, filecode[count], TACGIA, NGAYPHATHANH, IS_PRIVATE, file.ElementAt(count).ContentLength, VERSION, TRANGTHAI);
                                        // this.ElasticInsert(file_path, ID, file.ElementAt(count).ContentType, mota == "" ? filename[count] : mota, filename[count]);\
                                        ListTaiLieu.Add(ID);
                                    }
                                    if (!Is_MultiFile)
                                    {
                                        return ListTaiLieu;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ListTaiLieu;
        }
    }
}
