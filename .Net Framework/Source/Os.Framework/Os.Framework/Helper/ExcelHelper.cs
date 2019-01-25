using Microsoft.Office.Interop.Word;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;

namespace Os.Framework.Helper
{
    public class ExcelHelper
    {
        /// <summary>
        /// Excel转换成DataTable
        /// </summary>
        /// <param name="excelFilePath">excel文件路径</param>
        /// <param name="sheetNum">sheet序号</param>
        /// <param name="headerRowNum">标题列序号</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string excelFilePath, int sheetNum = 0, int headerRowNum = 0)
        {

            IWorkbook workbook;
            DataTable dt;
            string extension = Path.GetExtension(excelFilePath).ToLower();
            try
            {
                using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".xlsx")
                    { workbook = new XSSFWorkbook(fileStream); }
                    else
                    { workbook = new HSSFWorkbook(fileStream); }
                    ISheet sheet = workbook.GetSheetAt(sheetNum);
                    dt = new DataTable(sheet.SheetName);
                    IRow headerRow = sheet.GetRow(headerRowNum);
                    string fieldName = "";
                    //ArrayList fieldArray = new ArrayList();
                    /*
                     增加标题列,author liwx 2016/08/25
                     */
                    for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                    {
                        if (headerRow.GetCell(i) != null)
                        {
                            fieldName = headerRow.GetCell(i).ToString().Trim();
                            //fieldArray.Add(fieldName);
                            DataColumn column = new DataColumn(fieldName);
                            dt.Columns.Add(column);
                        }
                        else
                        {
                            break;
                        }
                    }

                    DataRow dr;
                    IRow row;
                    ICell cell;
                    //short format;
                    for (int i = headerRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            dr = dt.NewRow();
                            for (int j = headerRow.FirstCellNum; j < headerRow.LastCellNum; j++)
                            {
                                cell = row.GetCell(j);
                                if (cell != null)
                                {
                                    //format = cell.CellStyle.DataFormat;
                                    //if (format == 14 || format == 31 || format == 57 || format == 58)
                                    //    dataRow[j] = cell.DateCellValue.ToString("yyyy/MM/dd");//日期转化格式如需要可解开
                                    dr[j] = cell.ToString().Trim() == "" ? null : cell.ToString().Trim();
                                }
                                else
                                {
                                    dr[j] = null;
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        /// <summary>
        /// Excel转换成DataTable
        /// </summary>
        /// <param name="excelFilePath">excel文件路径</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="headerRowNum">标题列序号</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string excelFilePath, string sheetName, int headerRowNum = 0)
        {

            IWorkbook workbook;
            DataTable dt;
            string extension = Path.GetExtension(excelFilePath).ToLower();
            try
            {
                using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".xlsx")
                    { workbook = new XSSFWorkbook(fileStream); }
                    else
                    { workbook = new HSSFWorkbook(fileStream); }
                    ISheet sheet;
                    //如果有指定工作表名称
                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        sheet = workbook.GetSheet(sheetName);
                        //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        if (sheet == null)
                        {
                            sheet = workbook.GetSheetAt(0);
                        }
                    }
                    else
                    {
                        //如果没有指定的sheetName，则尝试获取第一个sheet
                        sheet = workbook.GetSheetAt(0);
                    }

                    dt = new DataTable(sheet.SheetName);
                    IRow headerRow = sheet.GetRow(headerRowNum);
                    string fieldName = "";
                    //ArrayList fieldArray = new ArrayList();
                    /*
                     增加标题列,author liwx 2016/08/25
                     */
                    for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                    {
                        if (headerRow.GetCell(i) != null)
                        {
                            fieldName = headerRow.GetCell(i).ToString().Trim();
                            //fieldArray.Add(fieldName);
                            DataColumn column = new DataColumn(fieldName);
                            dt.Columns.Add(column);
                        }
                        else
                        {
                            break;
                        }
                    }

                    DataRow dr;
                    IRow row;
                    ICell cell;
                    //short format;
                    for (int i = headerRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            dr = dt.NewRow();
                            for (int j = headerRow.FirstCellNum; j < headerRow.LastCellNum; j++)
                            {
                                cell = row.GetCell(j);
                                if (cell != null)
                                {
                                    //format = cell.CellStyle.DataFormat;
                                    //if (format == 14 || format == 31 || format == 57 || format == 58)
                                    //    dataRow[j] = cell.DateCellValue.ToString("yyyy/MM/dd");//日期转化格式如需要可解开
                                    dr[j] = cell.ToString().Trim() == "" ? null : cell.ToString().Trim();
                                }
                                else
                                {
                                    dr[j] = null;
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RemoveEmpty(dt);
        }
        /// <summary>
        /// 获取Excel里Sheet总数
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static int GetExcelSheetTotal(string excelFilePath)
        {
            IWorkbook workbook;
            DataTable dt;
            string extension = Path.GetExtension(excelFilePath).ToLower();
            try
            {
                using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".xlsx")
                    { workbook = new XSSFWorkbook(fileStream); }
                    else
                    { workbook = new HSSFWorkbook(fileStream); }
                    return workbook.NumberOfSheets;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将datatable导入到exel
        /// </summary>
        /// <param name="datatemp"></param>
        /// <param name="fileName"></param>
        ///<param name="removeEmpty">是否去除所有值都为空的列</param>
        /// <returns></returns>
        public static int DataTableToExcel(DataTable datatemp, string fileName, bool removeEmpty = true)
        {
            DataTable data = removeEmpty ? RemoveEmpty(datatemp) : datatemp;
            bool isColumnWritten = true;
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            IWorkbook workbook = null;
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook();
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook();

                try
                {
                    if (workbook != null)
                    {
                        sheet = workbook.CreateSheet("Sheet1");
                    }
                    else
                    {
                        return -1;
                    }

                    if (isColumnWritten == true) //写入DataTable的列名
                    {
                        IRow row = sheet.CreateRow(0);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                        }
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }

                    for (i = 0; i < data.Rows.Count; ++i)
                    {
                        IRow row = sheet.CreateRow(count);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                        }
                        ++count;
                    }
                    workbook.Write(fs); //写入到excel
                    return count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    return -1;
                }
            }
        }



        /// <summary>
        /// Excel导出成内存流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream DataTableToExcel(DataTable data)
        {
            bool isColumnWritten = true;
            int i = 0;
            int j = 0;
            int count = 0;
            IWorkbook workbook = new HSSFWorkbook();
            try
            {
                //添加一个sheet
                ISheet sheet = workbook.CreateSheet("Sheet1");
                //将数据逐步写入sheet1各个行
                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                // 写入到客户端 
                MemoryStream ms = new System.IO.MemoryStream();
                workbook.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// Excel导出成内存流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream DataTableToExcel(List<DataTable> dtList, List<string> nameList)
        {
            IWorkbook workbook = new HSSFWorkbook();
            try
            {
                var data = dtList[0];
                for (var i = 0; i < dtList.Count(); i++)
                {
                    ISheet sheet = string.IsNullOrWhiteSpace(nameList[i]) ? workbook.CreateSheet("Sheet" + (i + 1)) : workbook.CreateSheet(nameList[i]);
                    WriteSheet(dtList[i], sheet);
                }
                // 写入到客户端 
                MemoryStream ms = new System.IO.MemoryStream();
                workbook.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private static void WriteSheet(DataTable data, ISheet sheet, bool isColumnWritten = true)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            //将数据逐步写入sheet1各个行
            if (isColumnWritten == true) //写入DataTable的列名
            {
                IRow row = sheet.CreateRow(0);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                }
                count = 1;
            }
            else
            {
                count = 0;
            }

            for (i = 0; i < data.Rows.Count; ++i)
            {
                IRow row = sheet.CreateRow(count);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                }
                ++count;
            }

        }
        /// <summary>
        /// 去除空行
        /// </summary>
        /// <param name="dtr"></param>
        /// <returns></returns>
        protected static DataTable RemoveEmpty(DataTable dtr)
        {
            DataTable dt = dtr;
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsNull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        IsNull = false;
                    }
                }
                if (IsNull)
                {
                    removelist.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }
    }
}
