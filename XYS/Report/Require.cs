using System;
using System.Text;
using System.Collections.Generic;

namespace XYS.Report
{
    public class Require
    {
        //private string m_orderByField;
        private Dictionary<string, object> m_ltDictionary;
        private Dictionary<string, object> m_gtDictionary;
        private Dictionary<string, object> m_likeDictionary;
        private Dictionary<string, object> m_equalDictionary;
        private Dictionary<string, object> m_notEqualDictionary;

        #region 构造函数
        public Require()
        {
            this.m_ltDictionary = new Dictionary<string, object>(2);
            this.m_gtDictionary = new Dictionary<string, object>(2);
            this.m_equalDictionary = new Dictionary<string, object>(4);
            this.m_notEqualDictionary = new Dictionary<string, object>(2);
            this.m_likeDictionary = new Dictionary<string, object>(1);
        }
        #endregion

        #region 实例属性
        public Dictionary<string, object> LTFields
        {
            get { return this.m_ltDictionary; }
        }
        public Dictionary<string, object> GTFields
        {
            get { return this.m_gtDictionary; }
        }
        public Dictionary<string, object> EqualFields
        {
            get { return this.m_equalDictionary; }
        }
        public Dictionary<string, object> NoEqualFields
        {
            get { return this.m_notEqualDictionary; }
        }
        public Dictionary<string, object> LikeFields
        {
            get { return this.m_likeDictionary; }
        }
        #endregion

        #region 私有方法
        private bool IsExist(Dictionary<string, object> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.IsExist(this.m_equalDictionary))
            {
                sb.Append(this.GetWhere(this.m_equalDictionary, "="));
            }
            if (this.IsExist(this.m_notEqualDictionary))
            {
                sb.Append(this.GetWhere(this.m_notEqualDictionary, "!="));
            }
            if (this.IsExist(this.m_ltDictionary))
            {
                sb.Append(this.GetWhere(this.m_ltDictionary, ">"));
            }
            if (this.IsExist(this.m_gtDictionary))
            {
                sb.Append(this.GetWhere(this.m_gtDictionary, "<"));
            }
            if (sb.Length > 5)
            {
                sb.Remove(sb.Length - 5, 5);
            }
            return sb.ToString();
        }
        private string GetWhere(Dictionary<string, object> dic, string fg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> item in dic)
            {
                //int
                if (item.Value.GetType().FullName == "System.Int32")
                {
                    sb.Append(item.Key);
                    sb.Append(fg);
                    sb.Append(item.Value);
                }
                //datetime
                else if (item.Value.GetType().FullName == "System.DateTime")
                {
                    DateTime dt = (DateTime)item.Value;
                    sb.Append(item.Key);
                    sb.Append(fg);
                    sb.Append("'");
                    sb.Append(dt.Date.ToString("yyyy-MM-dd"));
                    sb.Append("'");
                }
                //其他类型
                else
                {
                    sb.Append(item.Key);
                    sb.Append(fg);
                    sb.Append("'");
                    sb.Append(item.Value.ToString());
                    sb.Append("'");
                }
                sb.Append(" and ");
            }
            return sb.ToString();
        }
        #endregion
    }
}
