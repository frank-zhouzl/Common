using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Os.Framework.Helper
{
    public static class ConfigWatcher
    {
        private static FileSystemWatcher _watcher;
        private static readonly object _lockObj = new object();
        /// <summary>
        /// 设置Config监控
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="path"></param>
        public static void SetupWacher(string path)
        {
            if (_watcher == null)
            {
                lock (_lockObj)
                {
                    if (_watcher == null)
                    {
                        FileInfo file = new FileInfo(path);
                        _watcher = new FileSystemWatcher(file.DirectoryName);
                        _watcher.Filter = file.Name;
                        _watcher.NotifyFilter = NotifyFilters.LastWrite;
                        _watcher.EnableRaisingEvents = true;
                        _watcher.Changed += new FileSystemEventHandler(_watcher_Changed);
                    }
                }
            }
        }

        private static void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            RestartAppDomain();
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        private static bool RestartAppDomain()
        {
            bool restarted = false;
            try
            {
                HttpRuntime.UnloadAppDomain();
                restarted = true;
            }
            catch
            {
                restarted = false;
            }

            return restarted;
        }
    }
}
