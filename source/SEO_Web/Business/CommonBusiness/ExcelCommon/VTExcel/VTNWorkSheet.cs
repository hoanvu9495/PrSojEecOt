using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;


namespace Business.CommonBusiness.ExcelCommon.VTExcel
{
    public class VTNWorkSheet
    {
        public HSSFWorkbook HSWorkBook { get; set; }
        public HSSFSheet HSWorksheet { get; set; }
        public HSSFName SheetName { get; set; }
        public VTNWorkSheet(HSSFSheet hsworksheet)
        {
            HSWorksheet = hsworksheet;
        }

        /// <summary>
        /// Return number of rows used
        /// </summary>
        /// <returns></returns>
        public int GetCountUsedRows()
        {
            return HSWorksheet.LastRowNum;
        }

        /// <summary>
        /// Return number of columns used
        /// </summary>
        /// <returns></returns>
        public int GetCountUsedColumns()
        {
            int colmax = 1;
            int colTemp = 0;
            for (int i = 0; i <= HSWorksheet.LastRowNum; i++)
            {
                colTemp = HSWorksheet.GetRow(i).LastCellNum;
                if (colmax < colTemp)
                {
                    colmax = colTemp;
                }
            }
            return colmax;
        }

        /// <summary>
        /// Property Hiden of sheet
        /// <author>Hungnd 20/12/2013</author>
        /// </summary>
        public string Name
        {
            get
            {
                return HSWorksheet.SheetName;
            }
        }

        public void copyRow(int sourceRowNum, int destinationRowNum, bool copyStyleOnly = false)
        {
            // Get the source / new row
            IRow newRow = HSWorksheet.GetRow(destinationRowNum);
            IRow sourceRow = HSWorksheet.GetRow(sourceRowNum);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                HSWorksheet.ShiftRows(destinationRowNum, HSWorksheet.LastRowNum, 1);
            }
            else
            {
                newRow = HSWorksheet.CreateRow(destinationRowNum);
            }

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                HSSFCell oldCell = (HSSFCell)sourceRow.GetCell(i);
                HSSFCell newCell = (HSSFCell)newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }
                if (copyStyleOnly)
                {
                    // Copy style from old cell and apply to new cell
                    HSSFCellStyle newCellStyle = (HSSFCellStyle)HSWorkBook.CreateCellStyle();
                    newCellStyle.CloneStyleFrom(oldCell.CellStyle);
                    newCell.CellStyle.CloneStyleFrom(oldCell.CellStyle);
                }
                // If there is a cell comment, copy
                if (oldCell.CellComment != null)
                {
                    newCell.CellComment = oldCell.CellComment;
                }

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null)
                {
                    newCell.Hyperlink = oldCell.Hyperlink;
                }

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.BLANK:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    case CellType.BOOLEAN:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case CellType.ERROR:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;
                    case CellType.FORMULA:
                        newCell.SetCellFormula(oldCell.CellFormula);
                        break;
                    case CellType.NUMERIC:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case CellType.STRING:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                    case CellType.Unknown:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                }

            }

            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < HSWorksheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = HSWorksheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
                                                                                (newRow.RowNum +
                                                                                 (cellRangeAddress.FirstRow -
                                                                                  cellRangeAddress.LastRow)),
                                                                                cellRangeAddress.FirstColumn,
                                                                                cellRangeAddress.LastColumn);
                    HSWorksheet.AddMergedRegion(newCellRangeAddress);
                }
            }

        }

        public void SetCellValue(int row, int column, object value)
        {
            var cell = HSWorksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                HSWorksheet.GetRow(row).CreateCell(column);
            }
            if (cell.CellType == CellType.STRING)
            {
                cell.SetCellValue(value.ToString());
            }
            if (cell.CellType == CellType.NUMERIC)
            {
                cell.SetCellValue(Convert.ToDouble(value));
            }
        }

        public void InsertRow(int fromRowIndex, int rowCount)
        {
            HSWorksheet.ShiftRows(fromRowIndex, HSWorksheet.LastRowNum, rowCount, true, false, true);

            for (int rowIndex = fromRowIndex; rowIndex < fromRowIndex + rowCount; rowIndex++)
            {
                HSSFRow rowSource = (HSSFRow)HSWorksheet.GetRow(rowIndex + rowCount);
                HSSFRow rowInsert = (HSSFRow)HSWorksheet.CreateRow(rowIndex);
                rowInsert.Height = rowSource.Height;
                for (int colIndex = 0; colIndex < rowSource.LastCellNum; colIndex++)
                {
                    HSSFCell cellSource = (HSSFCell)rowSource.GetCell(colIndex);
                    HSSFCell cellInsert = (HSSFCell)rowInsert.CreateCell(colIndex);
                    if (cellSource != null)
                    {
                        cellInsert.CellStyle = cellSource.CellStyle;
                    }
                }
            }
        }

        public void DeleteRow(int row)
        {
            var rowDelete = HSWorksheet.GetRow(row);
            if (rowDelete == null)
            {
                HSWorksheet.RemoveRow(rowDelete);
            }

        }

        //public CellRangeAddress GetRange(int firstRow, int lastRow, int firstCol, int lastCol)
        //{
        //    return new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
        //}

    }
}
