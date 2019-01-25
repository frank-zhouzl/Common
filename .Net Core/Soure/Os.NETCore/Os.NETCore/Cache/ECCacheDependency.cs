using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.NETCore.Cache
{
    public class ECCacheDependency
    {
        private string[] m_FileNames;
        public string[] FileNames
        {
            get
            {
                return m_FileNames;
            }
            set
            {
                m_FileNames = value;
            }
        }

        private ECCacheDependency() { }

        public ECCacheDependency(string fileName)
        {
            m_FileNames = new string[1];
            m_FileNames[0] = fileName;
        }

        public ECCacheDependency(params string[] filenames)
        {
            m_FileNames = filenames;
        }
    }
}
