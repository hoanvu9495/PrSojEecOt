using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using FileIo = System.IO.File;
using System.Xml.Linq;
using System.Web.Configuration;
using Novacode;
using Business.CommonBusiness;
using Model.eAita;
using Business.Business;
using Business.CommonConstant;

namespace Web.Common
{
    public class DocxProvider
    {

        #region KhaiBao
        private static TblTaiLieuKetXuatBusiness TblTaiLieuKetXuatBusiness;
        private static TblConfigTaiLieuBusiness TblConfigTaiLieuBusiness;

        private static Entities dbEntities = new Entities();
        #endregion
        private static string URLPath = WebConfigurationManager.AppSettings["FileUpload"];

        private static string ConvertToUnsign(string str)
        {
            string[] signs = new string[] { 
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ",
                "!@#$%^&*(),.[]{}"
                };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
                }
            }
            for (int i = 0; i < signs[signs.Length - 1].Length; i++)
            {
                str = str.Replace(signs[signs.Length - 1][i], ' ');
            }
            return str;
        }
        public static XElement GetHTMLString(string pathFile, string TaiLieuName)
        {
            FileInfo fileSource = new FileInfo(pathFile);
            string templateName = Guid.NewGuid().ToString();
            var pathfolder = Path.Combine(URLPath, "TempDocx");

            if (!Directory.Exists(pathfolder))
            {
                Directory.CreateDirectory(pathfolder);
            }

            string templateCopyPath = Path.Combine(URLPath, "TempDocx", templateName + ".docx");
            fileSource.CopyTo(templateCopyPath);

            DocX docx = DocX.Load(templateCopyPath);

            for (int i = 0; i < docx.Tables.Count; i++)
            {
                Novacode.Table t = docx.Tables[i];
                if (t.TableCaption != null)
                {
                    var rowcount = t.RowCount;
                    for (int k = rowcount - 1; k >= 1; k--)
                    {
                        t.RemoveRow(k);
                    }
                    t.InsertRow(1);
                    Novacode.Row myRow = t.Rows[1];
                    for (int j = 0; j < t.ColumnCount; j++)
                    {
                        var key_name = "[[ISTABLE_" + ConvertToUnsign(t.TableCaption + "_" + t.Paragraphs[j].Text).ToUpper().Replace(" ", "_") + "]]";
                        myRow.Cells[j].Paragraphs.First().InsertText(" " + key_name);
                    }

                }
            }
            docx.SaveAs(templateCopyPath);

            byte[] byteArray = FileIo.ReadAllBytes(templateCopyPath);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                {
                    string documentText;
                    using (StreamReader reader = new StreamReader(doc.MainDocumentPart.GetStream()))
                    {
                        documentText = reader.ReadToEnd();
                    }

                    documentText = documentText.Replace("##date##", DateTime.Today.ToShortDateString());
                    using (StreamWriter writer = new StreamWriter(doc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        writer.Write(documentText);
                    }

                    int imageCounter = 0;
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        PageTitle = TaiLieuName,
                        ImageHandler = imageInfo =>
                        {
                            DirectoryInfo localDirInfo = new DirectoryInfo("img");
                            if (!localDirInfo.Exists)
                                localDirInfo.Create();
                            ++imageCounter;
                            string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png")
                            {
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "gif")
                                imageFormat = ImageFormat.Gif;
                            else if (extension == "bmp")
                                imageFormat = ImageFormat.Bmp;
                            else if (extension == "jpeg")
                                imageFormat = ImageFormat.Jpeg;
                            else if (extension == "tiff")
                            {
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "x-wmf")
                            {
                                extension = "wmf";
                                imageFormat = ImageFormat.Wmf;
                            }
                            if (imageFormat == null)
                                return null;

                            string imageFileName = "img/image" +
                                imageCounter.ToString() + "." + extension;
                            try
                            {
                                imageInfo.Bitmap.Save(imageFileName, imageFormat);
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageFileName),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    XElement html = HtmlConverter.ConvertToHtml(doc, settings);
                    docx.Dispose();
                    FileIo.Delete(templateCopyPath);
                    return html;
                }
            }
        }

        public static string GetValueField(DataExportTHUEBO dataExport, string propertyName)
        {
            var infoType = dataExport.GetType();
            var property = infoType.GetProperty(propertyName);
            return property.GetValue(dataExport).ToString();

        }

        public static JsonResultBO ExportToWord(int taiLieuID, DataExportTHUEBO dataExport, string FileName,string userName="UnKnow")
        {

            var folderName = Path.Combine(URLPath, FolderFileConst.DOCX_OUTPUT, userName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var path = Path.Combine(folderName, FileName);


            var ModelResult = new JsonResultBO();
            try
            {
                TblTaiLieuKetXuatBusiness = new TblTaiLieuKetXuatBusiness(dbEntities);
                TblConfigTaiLieuBusiness = new TblConfigTaiLieuBusiness(dbEntities);

                var tailieuDetail = TblTaiLieuKetXuatBusiness.Find(taiLieuID);
                if (tailieuDetail == null)
                {
                    ModelResult.Status = false;
                    ModelResult.Message = "Không tìm thấy tài liệu cần kết xuất";
                    return ModelResult;
                }
                else
                {
                    if (string.IsNullOrEmpty(tailieuDetail.URL) || !FileIo.Exists(Path.Combine(URLPath, tailieuDetail.URL)))
                    {
                        ModelResult.Status = false;
                        ModelResult.Message = "Không tìm thấy tài liệu cần kết xuất";
                        return ModelResult;
                    }
                }


                //lấy list config
                var configTaiLieu = TblConfigTaiLieuBusiness.GetByTaiLieuID(tailieuDetail.ID);


                if (tailieuDetail != null)
                {

                    string template_path = Path.Combine(URLPath, tailieuDetail.URL);
                    FileInfo fileSource = new FileInfo(template_path);
                    string templateName = Guid.NewGuid().ToString();
                    string templateCopyPath = Path.Combine(folderName,FileName);
                    if (FileIo.Exists(templateCopyPath))
                    {
                        var dt= DateTime.Now;
                        FileName = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.TimeOfDay.TotalMilliseconds.ToString()+FileName;
                        templateCopyPath = Path.Combine(folderName, FileName);
                    }
                    if (System.IO.File.Exists(templateCopyPath))
                    {
                        System.IO.File.Delete(templateCopyPath);
                    }
                    fileSource.CopyTo(templateCopyPath);
                    DocX docx = DocX.Load(templateCopyPath);
                    for (int i = 0; i < docx.Tables.Count; i++)
                    {
                        Novacode.Table t = docx.Tables[i];
                        if (t.TableCaption != null)
                        {
                            var rowcount = t.RowCount;
                            var datakekhaiitem = configTaiLieu.Where(o => o.FIELD_KEY.Contains(ConvertToUnsign(t.TableCaption).ToUpper().Replace(" ", "_"))).ToList();

                            configTaiLieu = configTaiLieu.Where(o => !o.FIELD_KEY.Contains(ConvertToUnsign(t.TableCaption).ToUpper().Replace(" ", "_"))).ToList();
                            for (int k = rowcount - 1; k >= 1; k--)
                            {
                                t.RemoveRow(k);
                            }

                            for (int m = 1; m <= Math.Ceiling((double)datakekhaiitem.Count / (t.ColumnCount)); m++)
                            {
                                t.InsertRow(m);
                                Novacode.Row myRow = t.Rows[m];
                                for (int j = 0; j < t.ColumnCount; j++)
                                {
                                    var indexdata = (m - 1) * t.ColumnCount + j;

                                    myRow.Cells[j].Paragraphs.First().InsertText(" " + GetValueField(dataExport, datakekhaiitem[indexdata].COLUM_MIX));
                                }
                            }

                        }
                    }
                    var listCheckBox = configTaiLieu.Where(x => x.FIELD_KEY.StartsWith("ck.")).ToList();
                    configTaiLieu.RemoveAll(X => X.FIELD_KEY.StartsWith("ck."));

                    foreach (var item in configTaiLieu)
                    {
                        docx.ReplaceText("[[" + item.FIELD_KEY + "]]", GetValueField(dataExport, item.COLUM_MIX));

                    }

                    docx.RemoveProtection();
                    docx.AddProtection(EditRestrictions.none);
                    docx.SaveAs(templateCopyPath);
                    docx.Dispose();
                }
            }
            catch
            {
                ModelResult.Status = false;
                ModelResult.Message = "Không kết xuất được file";
                return ModelResult;
            }
            ModelResult.Status = true;
            ModelResult.Message = Path.Combine(FolderFileConst.DOCX_OUTPUT, userName, FileName);
            return ModelResult;
        }


    }
}