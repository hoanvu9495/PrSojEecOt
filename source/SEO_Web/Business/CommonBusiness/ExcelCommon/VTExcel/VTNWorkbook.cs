using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.IO;

namespace VTUtils.Excel.Export
{
    public class VTNWorkbook
    {
        public HSSFWorkbook HWorkbook { get; set; }
        public List<HSSFSheet> GetSheets()
        {
            List<HSSFSheet> list = new List<HSSFSheet>();
            for (int i = 0; i < HWorkbook.NumberOfSheets; i++)
            {
                HSSFSheet sheetTemp = (HSSFSheet)HWorkbook.GetSheetAt(i);
                list.Add(sheetTemp);
            }
            return list;
        }
        public HSSFSheet GetSheet(int index)
        {
            return (HSSFSheet)HWorkbook.GetSheetAt(index + 1);
        }
        public HSSFSheet GetSheet(string name)
        {
            return (HSSFSheet)HWorkbook.GetSheet(name);
        }
        public void SaveToFile(string pathFile)
        {
            Stream fout = ToStream();
            FileInfo fileInf = new FileInfo(pathFile);
            fileInf.Directory.Create();
            if (Directory.Exists(Path.GetDirectoryName(pathFile)))
                using (Stream destination = File.Create(pathFile))
                {
                    for (int a = fout.ReadByte(); a != -1; a = fout.ReadByte())
                    {
                        destination.WriteByte((byte)a);
                    }
                }
        }

        public Stream ToStream()
        {
            String pathFile = AppDomain.CurrentDomain.BaseDirectory
                + "Uploads" + Path.DirectorySeparatorChar
                + "Temp" + Path.DirectorySeparatorChar
                + Guid.NewGuid().ToString().Replace("-", "").ToUpper() + ".xls";
            //Ex: C://root//website//Uploads//Temp//0928AA991A9919A888.xls
            try
            {
                FileStream file = new FileStream(pathFile, FileMode.Create);
                HWorkbook.Write(file);
                file.Close();

                MemoryStream memStream;
                using (FileStream fileStream = File.OpenRead(pathFile))
                {
                    memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                }
                FileInfo fi = new FileInfo(pathFile);
                fi.Delete();
                return memStream;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (System.IO.File.Exists(pathFile))
                {
                    FileInfo fi = new FileInfo(pathFile);
                    fi.Delete();
                }
            }
        }






    }
}
