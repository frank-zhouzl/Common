using Microsoft.Office.Interop.Excel;
using Os.Framework.Helper;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace Os.Server.Controllers
{
    public class PreviewController : Controller
    {
        // GET: Preview
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// pdf文件预览
        /// </summary>
        /// <returns></returns>
        public ActionResult Preview()
        {
            return View();
        }
        /// <summary>
        /// Excel预览类
        /// </summary>
        /// <param name="p"></param>
        public void ExcelPriview()
        {
            string path = Server.MapPath("/") + @"Files\Excel工作表.xlsx";
            PriviewHelper.Instance.Priview(HttpContext,path, PriviewFileType.Excel);
        }
        /// <summary>
        /// pdf文件预览
        /// </summary>
        public void PdfPreview()
        {
            string path = Server.MapPath("/") + @"Files\协议.pdf" ;
            PriviewHelper.Instance.Priview(HttpContext, path, PriviewFileType.Pdf);
        }
        public void pdfGetimage()
        {
            string PDfFilePath = Server.MapPath("/") + @"Files\PDF创建.pdf";
            string path = Server.MapPath("/") + @"Files\";
            PDFHelper.Instance.ExtractImages_PDF(path, PDfFilePath);
        }
        /// <summary>
        /// text文件预览
        /// </summary>
        public void TxtPreview()
        {
            string fileName = "协议.txt";
            string PDfFilePath = Server.MapPath("/")+ @"Files\协议.pdf";
            string path = Server.MapPath("/") + "Files/"+ fileName;
            string moneyUp= MoneyCalculate.MoneyToUpper("1234.56");
            PriviewHelper.Instance.Priview(HttpContext, PDFHelper.Instance.ExtractText_PDF(path, PDfFilePath), PriviewFileType.Txt);
        }
        /// <summary>
        /// 创建Pdf文件
        /// </summary>
        public void PdfCreate()
        {
            string path = Server.MapPath("/") + @"Image/江城子密州出猎.JPEG"; 
            string SaveFileName = Server.MapPath("/") + @"Files\协议.pdf";
            PriviewHelper.Instance.Priview(HttpContext, PDFHelper.Instance.Create_PDF(path,SaveFileName), PriviewFileType.Pdf);
        }
        /// <summary>
        /// 加盖印章
        /// </summary>
        public void AddSeal()
        { 
            string FilePath = Server.MapPath("/") + @"Files\协议12.pdf";
            string text = "甲方：浙江华卫智能科技有限公司";
            string Imageurl = Server.MapPath("/") + @"Image\seal.png";
            PriviewHelper.Instance.Priview(HttpContext, PDFHelper.Instance.AddSeal_PDF(FilePath,text, Imageurl), PriviewFileType.Pdf);
        }
        /// <summary>
        /// 添加水印
        /// </summary>
        public void AddBrush_PDF()
        {
            string SaveFileName = Server.MapPath("/") + @"Files\协议12.pdf";
            PriviewHelper.Instance.Priview(HttpContext, PDFHelper.Instance.AddBrush_PDF(SaveFileName,"瓯速科技"), PriviewFileType.Pdf);
        }
        /// <summary>
        /// 打印
        /// </summary>
        public void Print_PDF()
        {
            string SaveFileName = Server.MapPath("/") + @"Files\协议.pdf";
            PDFHelper.Instance.Print_PDF(SaveFileName);
        }
    }
}