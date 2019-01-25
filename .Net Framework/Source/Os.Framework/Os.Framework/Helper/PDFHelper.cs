using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Spire.Pdf.Annotations;
using Spire.Pdf.Annotations.Appearance;
using Spire.Pdf.General.Find;

namespace Os.Framework.Helper
{
    /// <summary>
    /// PDF文件操作类
    /// </summary>
    public class PDFHelper
    {
        private static volatile PDFHelper pdfHelper = null;
        private static object syncRoot = new Object();
        public PDFHelper()
        {

        }
        /// <summary>
        /// 初始化注册
        /// </summary>
        public static PDFHelper Instance
        {
            get
            {
                if (pdfHelper == null)
                {
                    lock (syncRoot)
                    {
                        if (pdfHelper == null)
                        {
                            if (true)
                            {
                            }
                            pdfHelper = new PDFHelper();
                        }
                    }
                }
                return pdfHelper;
            }
        }
        /// <summary>
        /// 创建Pdf文档
        /// </summary>
        /// <param name="ImagePath">图片路径</param>
        /// <param name="SaveFilePath">文件保存路径(相对于网站根目录)</param>
        /// <returns>返回创建的文件名称</returns>
        public string Create_PDF(string ImagePath, string SaveFilePath)
        {
            //初始化一个PdfDocument类实例
            PdfDocument document = new PdfDocument();
             
            StringBuilder stringBuilder = new StringBuilder(File.ReadAllText(@"D:\tools\.Net Framework\Source\Os.Server\Os.Server\Files\协议.txt"));
            AddPage(stringBuilder, ImagePath, document);
            //保存文档
            document.SaveToFile(SaveFilePath);
            return SaveFilePath;
        }
        /// <summary>
        /// 给PDf文档加盖印章
        /// </summary>
        /// <param name="FilePath">PDf文件的路径</param>
        /// <returns>返回加盖印章后的pdf文件的路径</returns>
        public string AddSeal_PDF(string FilePath, string text, string ImageUrl)
        {
            AddSeal(FilePath, text, ImageUrl);
            return FilePath;
        }
        /// <summary>
        /// 给Pdf文档添加水印
        /// </summary>
        /// <param name="FilePath">Pdf文档路径地址</param>
        /// <param name="Text">水印内容</param>
        /// <returns>返回添加水印后的Pdf文档路径</returns>
        public string AddBrush_PDF(string FilePath, string Text)
        {
            //初始化一个PdfDocument类实例
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(FilePath);
            foreach (PdfPageBase item in document.Pages)
            {
                AddBrush(item, Text);
            }
            //保存文档
            document.SaveToFile(FilePath);
            return FilePath;
        }
        /// <summary>
        /// 添加一个Pdf文档页
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="document"></param>
        /// <param name="margins"></param>
        private void AddPage(StringBuilder stringBuilder, string ImagePath, PdfDocument document)
        {
            PdfMargins margins = new PdfMargins(0,0,0,0);
            //新添加一个A4大小的页面
            PdfPageBase page = document.Pages.Add(PdfPageSize.A4, margins);

            //自定义PdfTrueTypeFont、PdfPen实例，设置字体类型、字号和字体颜色
            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("华文楷体", 10f), true);
            PdfBrush brush = PdfBrushes.Black;

            //调用DrawString()方法在指定位置写入文本
            string text = stringBuilder.ToString();
            page.Canvas.DrawString(text, font, brush,new PointF(0,0));
        }
        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="page"></param>
        /// <param name="text"></param>
        private void AddBrush(PdfPageBase page, string text)
        {
            //设置每个水印格占整页画布的宽高
            PdfTilingBrush brush = new PdfTilingBrush(new SizeF(page.Canvas.ClientSize.Width, page.Canvas.ClientSize.Height / 2));
            brush.Graphics.SetTransparency(0.3f);
            brush.Graphics.Save();
            brush.Graphics.TranslateTransform(brush.Size.Width / 2, brush.Size.Height / 2);
            brush.Graphics.RotateTransform(-45);
            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("华文楷体", 70f), true);
            brush.Graphics.DrawString(text, font, PdfBrushes.Red, 0, 0, new PdfStringFormat(PdfTextAlignment.Center));
            brush.Graphics.Restore();
            brush.Graphics.SetTransparency(1);
            page.Canvas.DrawRectangle(brush, new RectangleF(new PointF(0, 0), page.Canvas.ClientSize));
        }
        /// <summary>
        /// 加盖印章
        /// </summary> 
        /// <param name="FilePath">Pdf文件路径</param>
        /// <param name="ImageUrl">印章图片地址</param>
        /// <param name="text">关键词内容</param>
        private void AddSeal(string FilePath, string text, string ImageUrl)
        {
            //创建PdfDocument对象
            PdfDocument doc = new PdfDocument();

            //加载现有PDF文档
            doc.LoadFromFile(FilePath);

            //获取要添加动态印章的页面
            PdfPageBase page = doc.Pages[doc.Pages.Count - 1];
            PdfTextFind[] finds = page.FindText(text).Finds;
            //获取关键字坐标
            if (finds.Length > 0)
            {
                PointF pointf = finds[finds.Length - 1].Position;
                //加载印章
                PdfImage image = PdfImage.FromFile(ImageUrl);
                //设置印章大小比例
                double b =150.00/image.Width;
                //在画布上画上印章
                page.Canvas.DrawImage(image, new PointF(pointf.X,pointf.Y-10),new SizeF((float)(image.Width * b),(float)(image.Height * b)));
                //保存画布状态
                PdfGraphicsState state = page.Canvas.Save();
                page.Canvas.Restore(state);

            }
            //保存文档
            doc.SaveToFile(FilePath, FileFormat.PDF);
        }
        /// <summary>
        /// 读取部分文本
        /// </summary>
        /// <param name="TxtPath">文本文档保存的路径</param>
        /// <param name="PdfFilepath">Pdf文件的路径</param>
        /// <returns>返回文本文档保存的路径</returns>
        public string ExtractText_PDF(string TxtPath, string PdfFilepath)
        {
            //实例化PdfDocument类对象，并加载PDF文档
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(PdfFilepath);

            //实例化一个StringBuilder 对象
            StringBuilder content = new StringBuilder();

            //遍历文档所有PDF页面，提取文本
            foreach (PdfPageBase page in doc.Pages)
            {
                content.Append(page.ExtractText());
            }

            //将提取到的文本写为.txt格式并保存到本地路径
            String fileName = TxtPath;
            File.WriteAllText(fileName, content.ToString());
            // System.Diagnostics.Process.Start(TxtPath);
            return TxtPath;
        }
        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="SaveDirPath">图片将要保存的文件夹名称</param>
        /// <param name="PdfFilepath">PDF文件的路径</param>
        /// <returns>返回图片保存的路径集合</returns>
        public List<string> ExtractImages_PDF(string SaveDirPath, string PdfFilepath)
        {
            //创建一个PdfDocument类对象，加载PDF测试文档
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(PdfFilepath);
            List<string> FileList = new List<string>();

            //声明List类对象
            List<Image> ListImage = new List<Image>();

            //遍历PDF文档所有页面
            for (int i = 0; i < doc.Pages.Count; i++)
            {
                //获取文档所有页，并提取页面中的所有图片
                PdfPageBase page = doc.Pages[i];
                Image[] images = page.ExtractImages();
                if (images != null && images.Length > 0)
                {
                    ListImage.AddRange(images);
                }

            }
            //将获取到的图片保存到本地路径
            if (ListImage.Count > 0)
            {
                for (int i = 0; i < ListImage.Count; i++)
                {
                    Image image = ListImage[i];
                    FileList.Add(SaveDirPath + "image" + (i + 1).ToString() + ".png");
                    image.Save(SaveDirPath + "image" + (i + 1).ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
                //打开获取到的图片
                System.Diagnostics.Process.Start(SaveDirPath + "image1.png");
            }
            return FileList;
        }

        /// <summary>
        /// 打印PDF文档
        /// </summary>
        /// <param name="PdfFilepath">要打印的文档的路径</param>
        public void Print_PDF(string PdfFilepath)
        {
            //加载PDF文档
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(PdfFilepath);
            //使用默认打印机打印文档所有页面
            doc.PrintDocument.Print();
        }
    }
}
