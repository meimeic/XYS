using System;
using System.Collections;

using XYS.Lis.Core;
using XYS.Lis.Util;

namespace XYS.Lis.Model
{
    public class ElementType
    {
        private static readonly string m_defaultElementName = "defaultElementName";
        #region
        private string m_name;
        private string m_typeName;
        private string m_sql;
        #endregion

        #region
        //public static readonly ReportElementType DEFAULTITEM = new ReportElementType(typeof(ReportItemElement), ReportElementTag.ItemElement);
        //public static readonly ReportElementType DEFAULTGRAPH = new ReportElementType(typeof(ReportGraphElement), ReportElementTag.GraphElement);
        //public static readonly ReportElementType DEFAULTCUSTOM = new ReportElementType(typeof(ReportCustomElement), ReportElementTag.CustomElement);
        //public static readonly ReportElementType DEFAULTREPORT = new ReportElementType(typeof(ReportReportElement), ReportElementTag.Report);
        //public static readonly ReportElementType DEFAULTINFO = new ReportElementType(typeof(ReportInfoElement), ReportElementTag.InfoElement);
        #endregion

        #region
        public ElementType(string name,string typeName)
            : this(name, typeName, null)
        {
        }
        public ElementType(string name, string typeName, string sql)
        {
            this.m_name = name;
            this.m_typeName = typeName;
            this.m_sql = sql;
        }
        #endregion

        #region
        public string Name
        {
            get
            {
                if (this.m_name != null && !this.m_name.Equals(""))
                {
                    return this.m_name.ToLower();
                }
                return this.m_name;
            }
        }
        public string TypeName
        {
            get { return this.m_typeName; }
        }
        public string SQL
        {
            get { return this.m_sql; }
            set { this.m_sql = value; }
        }
        #endregion
    }
}
