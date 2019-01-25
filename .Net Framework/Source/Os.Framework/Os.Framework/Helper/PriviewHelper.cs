using Microsoft.Office.Interop.Excel; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Os.Framework.Helper
{
    public class PriviewHelper
    {
        private static volatile PriviewHelper priviewHelper = null;
        private static object syncRoot = new Object();
        public PriviewHelper()
        {

        }
        /// <summary>
        /// 初始化注册
        /// </summary>
        public static PriviewHelper Instance
        {
            get
            {
                if (priviewHelper == null)
                {
                    lock (syncRoot)
                    {
                        if (priviewHelper == null)
                        {
                            if (true)
                            {
                            }
                            priviewHelper = new PriviewHelper();
                        }
                    }
                }
                return priviewHelper;
            }
        }
        /// <summary>
        /// 预览Excel文件
        /// </summary> 
        /// <param name="FilePath">文件所在文件夹相对于网站根目录的路径</param>
        /// <param name="priviewFileType">要预览的文件的类型</param>
        /// <param name="httpContext">当前请求的http上下文</param>
        public void Priview(HttpContextBase httpContext,string FilePath,PriviewFileType priviewFileType)
        {
            string FileDirectoryPath=null;
            string fileName=null;
            //截取路径获得文件夹和文件名
            List<string> ListString = FilePath.Split('\\').ToList();
            ListString.ForEach(l => {
                if (l!= ListString.Last())
                {
                    FileDirectoryPath += l + '\\';
                }
                else
                {
                    fileName = l;
                }
                
            });
            switch (priviewFileType)
            {
                case PriviewFileType.Excel:
                    ExcelPriview(fileName,FileDirectoryPath);
                    break;
                case PriviewFileType.Pdf:
                    PdfPreview(httpContext,fileName,FileDirectoryPath);
                    break;
                case PriviewFileType.Txt:
                    TxtPreview(httpContext, fileName, FileDirectoryPath);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 预览Excel文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="FileDirectory">文件所在文件夹相对于网站根目录的路径</param>
        private void ExcelPriview(string fileName, string FileDirectoryPath)
        {

            Application excel = null;
            Workbook xls = null;
            excel = new Application();
            object missing = Type.Missing;
            object trueObject = true;
            excel.Visible = false;
            excel.DisplayAlerts = false;
            string randomName = DateTime.Now.Ticks.ToString(); 
            //output fileName
            xls = excel.Workbooks.Open(FileDirectoryPath + @"\" + fileName, missing, trueObject, missing,
                                        missing, missing, missing, missing, missing, missing, missing, missing,
                                        missing, missing, missing);
            //Save Excel to Html
            object format = XlFileFormat.xlHtml;
            Workbook wsCurrent = xls;
            String outputFile =FileDirectoryPath + @"\" + randomName + ".html";
            wsCurrent.SaveAs(outputFile, format, missing, missing, missing,
                              missing, XlSaveAsAccessMode.xlNoChange, missing,
                              missing, missing, missing, missing);
            excel.Quit();
            //Open generated Html
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = outputFile;
            process.Start();
        }
        /// <summary>
        /// pdf文件预览
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="FileDirectory">文件所在文件夹相对于网站根目录的路径</param>
        /// <param name="httpContext">当前请求的http上下文</param>
        private void PdfPreview(HttpContextBase httpContext,string fileName,string FileDirectory)
        {
            httpContext.Response.ContentType = "Application/pdf";
            httpContext.Response.AddHeader("content-disposition", "inline;filename=" + fileName);
            httpContext.Response.WriteFile(FileDirectory + @"\"+fileName);
            httpContext.Response.End();
        }
        /// <summary>
        /// Txt文件预览
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="FileDirectory">文件所在文件夹相对于网站根目录的路径</param>
        /// <param name="httpContext">当前请求的http上下文</param>
        private void TxtPreview(HttpContextBase httpContext, string fileName, string FileDirectory)
        {
            httpContext.Response.ContentType = "text/plain";
            httpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;  //保持和文件的编码格式一致
            httpContext.Response.AddHeader("content-disposition", "inline;filename=" + fileName);
            httpContext.Response.WriteFile(FileDirectory + @"\" + fileName);
            httpContext.Response.End();
        }
        /// <summary>
        /// Txt文件下载
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="FileDirectory">文件所在文件夹相对于网站根目录的路径</param>
        /// <param name="httpContext">当前请求的http上下文</param>
        public void TxtDownLoad(HttpContextBase httpContext, string fileName, string FileDirectory)
        {
            httpContext.Response.ContentType = "text/plain";
            httpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;  //保持和文件的编码格式一致
            httpContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            httpContext.Response.WriteFile(FileDirectory + @"\" + fileName);
            httpContext.Response.End();
        }
    }
    /// <summary>
    /// 支持预览的文件类型枚举
    /// </summary>
    public enum PriviewFileType
    {
        Excel=0,
        Pdf=1,
        Txt=2
    }
}
