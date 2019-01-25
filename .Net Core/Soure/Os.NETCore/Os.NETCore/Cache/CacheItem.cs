using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.NETCore.Cache
{
    public class CacheItem<T> : IComparable<CacheItem<T>>, ICacheItem
    {
        #region [  Fileds  ]

        private T obj;

        #endregion

        #region [  Constructors  ]

        private CacheItem() { }

        public CacheItem(string key, T obj)
        {
            this.Key = key;
            this.Created = DateTime.Now;
            this.LastActivity = DateTime.Now;
            this.Frequency = 0;
            this.obj = obj;
        }

        #endregion

        #region [  Properties  ]

        private string m_Key;
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        private DateTime m_Created;
        public DateTime Created
        {
            get { return m_Created; }
            set { m_Created = value; }
        }
        private DateTime m_LastActivity;
        public DateTime LastActivity
        {
            get { return m_LastActivity; }
            set { m_LastActivity = value; }
        }

        private int m_Frequency;
        public int Frequency
        {
            get { return m_Frequency; }
            set { m_Frequency = value; }
        }

        public T Object
        {
            get
            {
                this.LastActivity = DateTime.Now;
                this.Frequency++;
                return obj;
            }
            private set
            {
                this.Created = DateTime.Now;
                this.LastActivity = DateTime.Now;
                this.obj = value;
            }
        }

        #endregion

        #region [  IComparable<CacheItem<T>> Members  ]

        public int CompareTo(CacheItem<T> compareTo)
        {
            if (this.Frequency == compareTo.Frequency)
            {
                return this.LastActivity.CompareTo(compareTo.LastActivity);
            }
            return this.Frequency.CompareTo(compareTo.Frequency);
        }

        #endregion

        #region [  ICacheItem Members  ]

        string ICacheItem.Key
        {
            get { return this.Key; }
        }

        DateTime ICacheItem.Created
        {
            get { return this.Created; }
        }

        DateTime ICacheItem.LastActivity
        {
            get { return this.LastActivity; }
        }

        int ICacheItem.Frequency
        {
            get { return this.Frequency; }
        }

        object ICacheItem.Object
        {
            get { return this.Object; }
        }

        #endregion
    }

    public interface ICacheItem
    {
        string Key { get; }

        DateTime Created { get; }

        DateTime LastActivity { get; }

        int Frequency { get; }

        object Object { get; }
    }
}
