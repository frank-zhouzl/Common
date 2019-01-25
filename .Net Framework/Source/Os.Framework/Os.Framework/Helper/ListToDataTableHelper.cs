using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Os.Framework.Helper
{
    /// <summary>
    /// List转换为DataTable对象
    /// </summary>
    public class ListTranTableModel
    {
        /// <summary>
        /// 新增的列名称
        /// </summary>
        public string addColumName { get; set; }
        /// <summary>
        /// 新增列的默认信息
        /// </summary>
        public TableCloumDInfo tableCloumDInfo { get; set; }
    }



    /// <summary>
    /// 列的默认信息
    /// </summary>
    public class TableCloumDInfo
    {
        //列的数据类型
        public Type dataType { get; set; }
        //列的默认值
        public object defaultValue { get; set; }
    }


    /// <summary>
    /// 批量导入信息基类
    /// </summary>
    public class DatableBulk_ImportBase
    {
        /// <summary>
        /// 重命名的列集合
        /// </summary>
        public Dictionary<string, string> PropertyRenameDic { get; set; }
        /// <summary>
        /// 要指定输出的列名称
        /// </summary>
        public string[] PropertyName { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 批量导入信息时的说明信息
        /// </summary>
        public string ErrorInfo { get; set; }
    }


    /// <summary>
    /// 批量导入信息数据Table属性操作对象
    /// </summary>
    public class DatableProperty : DatableBulk_ImportBase
    {
    }


    public static class ListToDataTableHelper
    {
        /// <summary>    
        /// 将集合类转换成DataTable    
        /// </summary>    
        /// <param name="list">集合</param>    
        /// <returns></returns>    
        private static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                foreach (object t in list)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(t, null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>    
        /// DataTable 转换为List 集合    
        /// </summary>    
        /// <typeparam name="TResult">类型</typeparam>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表    
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口    
            Type t = typeof(T);

            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            //创建返回的集合    
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例    
                T ob = new T();
                //找到对应的数据  并赋值    
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.    
                oblist.Add(ob);
            }
            return oblist;
        }

        /// <summary>     
        /// 转化一个DataTable    
        /// </summary>      
        /// <typeparam name="T"></typeparam>    
        /// <param name="list"></param>    
        /// <returns></returns>    
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    
            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>    
        /// 将泛型集合类转换成DataTable    

        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    

        /// <param name="list">集合</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable<T>(list, new Dictionary<string, string>(), null);

        }

        /**/

        /// <summary>    
        /// 将泛型集合类转换成DataTable    
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="="propertyRenameDic">需要指定重命名的列集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ToDataTable<T>(IList<T> list, Dictionary<string, string> propertyRenameDic, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
                if (propertyRenameDic.Count > 0)
                {
                    foreach (var item in propertyRenameDic)
                    {
                        propertyNameList.Remove(item.Key);
                        propertyNameList.Add(item.Value);
                    }
                }
            }
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        if (propertyRenameDic.Keys.Contains(pi.Name))
                        {
                            if (propertyNameList.Contains(propertyRenameDic[pi.Name]))
                                result.Columns.Add(propertyRenameDic[pi.Name], pi.PropertyType);
                        }

                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyRenameDic.Keys.Contains(pi.Name))
                            {
                                if (propertyNameList.Contains(pi.Name) || propertyNameList.Contains(propertyRenameDic[pi.Name]))
                                {
                                    object obj = pi.GetValue(list[i], null);
                                    tempList.Add(obj);
                                }
                            }
                            else
                            {
                                if (propertyNameList.Contains(pi.Name))
                                {
                                    object obj = pi.GetValue(list[i], null);
                                    tempList.Add(obj);
                                }
                            }

                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将DataTable新增几列并指定默认值
        /// </summary>
        /// <param name="vTable">源表</param>
        /// <param name="list">增加的列集合</param>
        /// <returns></returns>
        public static DataTable AddColums(DataTable vTable, List<ListTranTableModel> list)
        {
            foreach (var item in list)
            {
                //添加一新列，其值为默认值
                DataColumn dc = new DataColumn(item.addColumName, item.tableCloumDInfo.dataType);
                dc.DefaultValue = item.tableCloumDInfo.defaultValue;
                dc.AllowDBNull = false;//这在创建表的时候，起作用，在为已有表新增列的时候，不起作用
                vTable.Columns.Add(dc);
            }
            return vTable;
        }
    }
}
