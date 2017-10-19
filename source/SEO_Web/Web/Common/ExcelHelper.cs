using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OfficeOpenXml;

namespace Web.Common
{
    public class ExcelHelper<T> where T : new()
    {
        private int maxImportPerTime = 500;
        private string defaultStatus = "NNT đang hoạt động (đã được cấp GCN đăng ký thuế)";
        public int startRow;
        public int startColumn;
        public string[] arrProperties;

        private List<T> correctObjects;
        private List<T> inCorrectObjects;


        public List<T> GetListCorrectObjects()
        {
            return this.correctObjects;
        }

        public List<T> GetListInCorrectObjects()
        {
            return this.inCorrectObjects;
        }

        public T GetObject()
        {
            return new T();
        }

        public ExcelHelper()
        {
            this.correctObjects = new List<T>();
            this.inCorrectObjects = new List<T>();
        }

        public ExcelHelper(int startRow, int startColumn, string[] arrProperties)
        {
            this.startRow = startRow;
            this.startColumn = startColumn;
            this.arrProperties = arrProperties;

            this.correctObjects = new List<T>();
            this.inCorrectObjects = new List<T>();
        }

        //review excel - old way
        public void GetListObjectsByInterop(HttpServerUtilityBase server, HttpPostedFileBase file)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string storeFolder = server.MapPath("~/TemporaryExcel/");
            string filePath = Path.Combine(storeFolder, file.FileName);
            if (Directory.Exists(storeFolder))
            {
                FileUltilities fileUlti = new FileUltilities();
                fileUlti.CreateFolder("TemporaryExcel");
            }
            else
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            file.SaveAs(filePath);
            Application app = null;
            Workbook workbook = null;
            Worksheet worksheet = null;
            Range range = null;
            app = new Microsoft.Office.Interop.Excel.Application();
            workbook = app.Workbooks.Open(filePath);
            worksheet = workbook.ActiveSheet;
            range = worksheet.UsedRange;

            List<T> result = new List<T>();
            int rangeCount = range.Rows.Count;
            int arrPropertiesLength = arrProperties.Length;

            T currentObject = this.GetObject();

            List<ExcelObjectProperty> listProperties = new List<ExcelObjectProperty>();
            for (int col = 0; col < arrPropertiesLength; col++)
            {
                ExcelObjectProperty prop = new ExcelObjectProperty();
                prop.propertyInfo = currentObject.GetType().GetProperty(arrProperties[col]);
                listProperties.Add(prop);
            }

            for (int row = this.startRow; row < rangeCount; row++)
            {
                int startColumn = this.startColumn;
                currentObject = this.GetObject();
                bool error = false;
                for (int col = 0; col < arrPropertiesLength; col++)
                {
                    string value = ((Range)range.Cells[row, startColumn]).Text;
                    var property = listProperties[col];
                    try
                    {
                        object objectValue = new object();
                        objectValue = value;
                        if (typeof(DateTime) == property.propertyInfo.PropertyType || typeof(DateTime) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                        {
                            objectValue = value.ToDateTime();
                        }
                        else if (typeof(Int32) == property.propertyInfo.PropertyType || typeof(Int32) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                        {
                            if (property.propertyInfo.Name.Equals("TRANGTHAI"))
                            {
                                objectValue = value.Trim().Equals(this.defaultStatus) ? 0 : 1;
                            }
                            else
                            {
                                objectValue = value.ToIntOrZero();
                            }
                        }
                        else if (typeof(Decimal) == property.propertyInfo.PropertyType || typeof(Decimal) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                        {
                            objectValue = value.ToDecimalOrZero();
                        }
                        else if (typeof(bool) == property.propertyInfo.PropertyType || typeof(bool) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                        {
                            if (property.propertyInfo.Name.Equals("TRANGTHAI") && value.Trim().Equals(this.defaultStatus))
                            {
                                objectValue = true;
                            }
                            else if (property.propertyInfo.Name.Equals("CO_GIAMNHE") && !value.Trim().Equals(string.Empty))
                            {
                                objectValue = true;
                            }
                            objectValue = false;
                        }
                        property.propertyInfo.SetValue(currentObject, objectValue);
                    }
                    catch
                    {
                        error = true;
                        continue;
                    }
                    finally
                    {
                        startColumn++;
                    }
                }
                if (error)
                {
                    this.inCorrectObjects.Add(currentObject);
                }
                else
                {
                    this.correctObjects.Add(currentObject);
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            workbook.Close(0);
            app.Quit();
            //kill process
            ExcelKiller.TerminateExcelProcess(app);
        }

        //review excel - new way using ole db
        public List<T> GetListObjectByOleDb(HttpServerUtilityBase server, HttpPostedFileBase file)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string storeFolder = server.MapPath("~/TemporaryExcel/");
            string filePath = Path.Combine(storeFolder, file.FileName);
            if (Directory.Exists(storeFolder))
            {
                FileUltilities fileUlti = new FileUltilities();
                fileUlti.CreateFolder("TemporaryExcel");
            }
            else
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            file.SaveAs(filePath);
            #region
            Dictionary<string, string> props = new Dictionary<string, string>();
            // XLSX - Excel 2007, 2010, 2012, 2013
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = filePath;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }
            string connectionString = sb.ToString();
            int arrPropertiesLength = this.arrProperties.Length;
            List<T> result = new List<T>();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", conn))
                {
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T currentObject = this.GetObject();
                            Type currentType = currentObject.GetType();
                            for (int col = 0; col < arrPropertiesLength; col++)
                            {
                                 
                            }
                            result.Add(currentObject);
                        }
                    }
                }
            }
            #endregion
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return result;
        }

        public void GetListObjectByEpplus(HttpServerUtilityBase server, HttpPostedFileBase file)
        {
            string storeFolder = server.MapPath("~/TemporaryExcel/");
            string filePath = Path.Combine(storeFolder, file.FileName);
            if (Directory.Exists(storeFolder))
            {
                FileUltilities fileUlti = new FileUltilities();
                fileUlti.CreateFolder("TemporaryExcel");
            }
            else
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            file.SaveAs(filePath);

            System.Data.DataTable dt = new System.Data.DataTable();
            using(ExcelPackage package =  new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count >= 1)
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();

                    int arrPropertiesLength = this.arrProperties.Length;
                    T currentObject = this.GetObject();
                    List<ExcelObjectProperty> listProperties = new List<ExcelObjectProperty>();
                    for (int col = 0; col < arrPropertiesLength; col++)
                    {
                        ExcelObjectProperty prop = new ExcelObjectProperty();
                        prop.propertyInfo = currentObject.GetType().GetProperty(arrProperties[col]);
                        listProperties.Add(prop);
                    }
                    int rowCount = workSheet.Dimension.End.Row;

                    for (int row = this.startRow; row < rowCount; row++)
                    {
                        int startColumn = this.startColumn;
                        currentObject = this.GetObject();
                        bool error = false;
                        for (int col = 0; col < arrPropertiesLength; col++)
                        {
                            string value = workSheet.Cells[row, startColumn, row, startColumn].Text;
                            var property = listProperties[col];
                            try
                            {
                                object objectValue = new object();
                                objectValue = value;
                                if (typeof(DateTime) == property.propertyInfo.PropertyType || typeof(DateTime) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                                {
                                    objectValue = value.ToDateTime();
                                }
                                else if (typeof(Int32) == property.propertyInfo.PropertyType || typeof(Int32) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                                {
                                    if (property.propertyInfo.Name.Equals("TRANGTHAI"))
                                    {
                                        objectValue = value.Trim().Equals(this.defaultStatus) ? 0 : 1;
                                    }
                                    else
                                    {
                                        objectValue = value.ToIntOrZero();
                                    }
                                }
                                else if (typeof(Decimal) == property.propertyInfo.PropertyType || typeof(Decimal) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                                {
                                    objectValue = value.ToDecimalOrZero();
                                }
                                else if (typeof(bool) == property.propertyInfo.PropertyType || typeof(bool) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                                {
                                    if (property.propertyInfo.Name.Equals("TRANGTHAI") && value.Trim().Equals(this.defaultStatus))
                                    {
                                        objectValue = true;
                                    }
                                    else if (property.propertyInfo.Name.Equals("CO_GIAMNHE") && !value.Trim().Equals(string.Empty))
                                    {
                                        objectValue = true;
                                    }
                                    objectValue = false;
                                }
                                property.propertyInfo.SetValue(currentObject, objectValue);
                            }
                            catch
                            {
                                error = true;
                                continue;
                            }
                            finally
                            {
                                startColumn++;
                            }
                        }
                        if (error)
                        {
                            this.inCorrectObjects.Add(currentObject);
                        }
                        else
                        {
                            this.correctObjects.Add(currentObject);
                        }
                    }
                }
            }
        }

        public void GetListObjectTblDoanhNghiepByEpplus(HttpServerUtilityBase server, HttpPostedFileBase file)
        {
            string storeFolder = server.MapPath("~/TemporaryExcel/");
            string filePath = Path.Combine(storeFolder, file.FileName);
            if (Directory.Exists(storeFolder))
            {
                FileUltilities fileUlti = new FileUltilities();
                fileUlti.CreateFolder("TemporaryExcel");
            }
            else
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            file.SaveAs(filePath);

            System.Data.DataTable dt = new System.Data.DataTable();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count >= 1)
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();
                    int arrPropertiesLength = this.arrProperties.Length;
                    T currentObject = this.GetObject();
                    List<ExcelObjectProperty> listProperties = new List<ExcelObjectProperty>();
                    for (int col = 0; col < arrPropertiesLength; col++)
                    {
                        ExcelObjectProperty prop = new ExcelObjectProperty();
                        prop.propertyInfo = currentObject.GetType().GetProperty(arrProperties[col]);
                        listProperties.Add(prop);
                    }
                    int rowCount = workSheet.Dimension.End.Row;
                    for (int row = this.startRow; row < rowCount; row++)
                    {
                        int startColumn = this.startColumn;
                        currentObject = this.GetObject();
                        bool error = false;
                        for (int col = 0; col < arrPropertiesLength; col++)
                        {
                            string value = workSheet.Cells[row, startColumn, row, startColumn].Text;
                            var property = listProperties[col];
                            try
                            {
                                object objectValue = new object();
                                objectValue = value;
                                if (typeof(DateTime) == property.propertyInfo.PropertyType || typeof(DateTime) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                                {
                                    objectValue = value.ToDateTime();
                                }
                                else if (typeof(Int32) == property.propertyInfo.PropertyType || typeof(Int32) == property.propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault())
                                {
                                    objectValue = property.propertyInfo.Name.Equals("TINH") ? 0 : value.ToIntOrZero();
                                }
                                property.propertyInfo.SetValue(currentObject, objectValue);
                            }
                            catch
                            {
                                error = true;
                                continue;
                            }
                            finally
                            {
                                startColumn++;
                            }
                        }
                        if (error)
                        {
                            this.inCorrectObjects.Add(currentObject);
                        }
                        else
                        {
                            this.correctObjects.Add(currentObject);
                        }
                    }
                }
            }
        }

        //multiple insert sql using SQLBulk technique
        public bool Import(IList<T> listObjects, int userId)
        {
            try
            {
                if (listObjects != null && listObjects.Count > 0)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    string efConnectionString = WebConfigurationManager.ConnectionStrings["Entities"].ConnectionString;
                    var builder = new EntityConnectionStringBuilder(efConnectionString);
                    string connectionString = builder.ProviderConnectionString;
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                    System.Data.DataTable table = new System.Data.DataTable();
                    for (int i = 0; i < props.Count; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                    using (SqlConnection destinationConnection = new SqlConnection(connectionString))
                    {
                        destinationConnection.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                        {
                            int times = (listObjects.Count / this.maxImportPerTime) + 1;
                            bulkCopy.DestinationTableName = typeof(T).Name;
                            for (int iTimes = 1; iTimes <= times; iTimes++)
                            {
                                int skip = this.maxImportPerTime * (iTimes - 1);
                                List<T> listImports = listObjects.Skip(skip).Take(this.maxImportPerTime).ToList();
                                object[] values = new object[props.Count];
                                table.Rows.Clear();
                                foreach (T item in listImports)
                                {
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        if (props[i].Name.Equals("ID_CANBO"))
                                        {
                                            props[i].SetValue(item, userId);
                                        }
                                        values[i] = props[i].GetValue(item);
                                    }
                                    table.Rows.Add(values);
                                }
                                bulkCopy.WriteToServer(table);
                            }
                            destinationConnection.Close();
                        }
                    }
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        //download file excel
        public ExcelExportResult Export(HttpServerUtilityBase server, IList<T> listObjects, string templatePath, string fileName)
        {
            try
            {
                Application app = null;
                Workbook workbook = null;
                Worksheet worksheet = null;
                app = new Microsoft.Office.Interop.Excel.Application();
                workbook = app.Workbooks.Open(templatePath);
                worksheet = workbook.ActiveSheet;
                int rows = listObjects.Count;
                if (rows > 0)
                {
                    int columns = this.arrProperties.Length;
                    var data = new object[rows, columns];
                    for (var row = 0; row < rows; row++)
                    {
                        T currentObject = listObjects[row];
                        Type currentType = currentObject.GetType();
                        for (var column = 0; column < columns; column++)
                        {
                            PropertyInfo prop = currentType.GetProperty(this.arrProperties[column]);
                            data[row, column] = prop.GetValue(currentObject);
                        }
                    }
                    Range startCell = (Range)worksheet.Cells[this.startRow, this.startColumn];
                    Range endCell = (Range)worksheet.Cells[rows, columns];
                    Range writeRange = worksheet.Range[startCell, endCell];
                    writeRange.Value2 = data;
                    writeRange.WrapText = true;
                }
                fileName = fileName + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx";
                string targetPath = server.MapPath("~/Content/Export/LogExcelDoanhNghiep/" + fileName);
                workbook.SaveAs(targetPath);
                workbook.Close();
                app.DisplayAlerts = false;
                app.Quit();
                ExcelKiller.TerminateExcelProcess(app);
                return new ExcelExportResult(targetPath, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }


    public class ExcelObjectProperty
    {
        private PropertyInfo _propertyInfo { set; get; }
        public PropertyInfo propertyInfo
        {
            set
            {
                this._propertyInfo = value;
            }
            get
            {
                return this._propertyInfo;
            }
        }
    }

    public class ExcelExportResult
    {

        private string fileName { set; get; }
        private string filePath { set; get; }

        public ExcelExportResult(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
        }

        public string getFileName()
        {
            return this.fileName;
        }

        public string getFilePath()
        {
            return this.filePath;
        }
    }

    public class ExcelKiller
    {
        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);
        public static void TerminateExcelProcess(Application excelApp)
        {
            int id;
            GetWindowThreadProcessId(excelApp.Hwnd, out id);
            var process = Process.GetProcessById(id);
            if (process != null)
            {
                process.Kill();
            }
        }
    }
}