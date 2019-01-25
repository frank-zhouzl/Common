using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.Framework.Helper
{
    public class ImportHelper
    {
        private static volatile ImportHelper importHelper = null;
        private static object syncRoot = new Object();
        public ImportHelper()
        {

        }
        /// <summary>
        /// 初始化注册
        /// </summary>
        public static ImportHelper Instance
        {
            get
            {
                if (importHelper == null)
                {
                    lock (syncRoot)
                    {
                        if (importHelper == null)
                        {
                            if (true)
                            {
                            }
                            importHelper = new ImportHelper();
                        }
                    }
                }
                return importHelper;
            }
        }
        /// <summary>
        /// 获取本地存储地址
        /// </summary>
        /// <param name="dt">要导出的的内容的Datatable</param>
        /// <param name="prefix">前缀类别</param>
        /// <returns></returns>
        public string GetDwonLoadlPath(DataTable dt, string prefix)
        {
            string filename = System.Configuration.ConfigurationManager.AppSettings[prefix].ToString() + Guid.NewGuid() + ".xlsx";
            string filePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\Files\\Download\\" + filename;
            string returnpath = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath + "/Files/Download/" + filename;
            ExcelHelper.DataTableToExcel(dt, filePath);
            return System.Configuration.ConfigurationManager.AppSettings["DomainName"] + returnpath;
        }
    }
}
