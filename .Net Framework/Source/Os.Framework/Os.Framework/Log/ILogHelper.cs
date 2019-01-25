using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.Framework.Log
{
    public interface ILogHelper
    {
        void WriteLog(Type t, string info, LogType type, string fromIP);

    }

    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 正常日志
        /// </summary>
        Info = 0,
        /// <summary>
        /// 错误日志
        /// </summary>
        Error = 1,
        /// <summary>
        /// 调试日志
        /// </summary>
        Debug = 2
    }

    public enum LogTypeEnum
    {
        /// <summary>
        /// 文本日志
        /// </summary>
        Log4Net = 1,
        /// <summary>
        /// MongoDB数据库日志
        /// </summary>
        MongoDB = 2
    }
}
