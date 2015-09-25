using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XYS.Lis.Core;
namespace XYS.Lis.DAL
{
    public abstract class BasicDAL<T>:ILisBasicDAL<T>
    {
        public T Search(Hashtable equalTable)
        {
            throw new System.NotImplementedException();
        }

        public void Search(T t, Hashtable equalTable)
        {
            throw new System.NotImplementedException();
        }

        public List<T> SearchList(Hashtable equalTable)
        {
            throw new System.NotImplementedException();
        }

        public void SearchList(List<T> lt, Hashtable equalTable)
        {
            throw new System.NotImplementedException();
        }


        protected virtual string GetSQLWhere(Hashtable equalFields)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            foreach (DictionaryEntry de in equalFields)
            {
                //int
                if (de.Value.GetType().FullName == "System.Int32")
                {
                    sb.Append(de.Key);
                    sb.Append("=");
                    sb.Append(de.Value);
                }
                //datetime
                else if (de.Value.GetType().FullName == "System.DateTime")
                {
                    DateTime dt = (DateTime)de.Value;
                    sb.Append(de.Key);
                    sb.Append("='");
                    sb.Append(dt.Date.ToString("yyyy-MM-dd"));
                    sb.Append("'");
                }
                //其他类型
                else
                {
                    sb.Append(de.Key);
                    sb.Append("='");
                    sb.Append(de.Value.ToString());
                    sb.Append("'");
                }
                sb.Append(" and ");
            }
            sb.Remove(sb.Length - 5, 5);
            return sb.ToString();
        }
    }
}
