using System;
using System.Collections;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Util;
namespace XYS.Lis.Model
{
    public class ReportElementType
    {
        #region
        private string m_elementName;
        private ReportElementTag m_elementTag;
        private Type m_elementType;
        #endregion

        #region
        public static readonly ReportElementType DEFAULTITEM = new ReportElementType(typeof(ReportItemElement), ReportElementTag.ItemElement);
        public static readonly ReportElementType DEFAULTGRAPH = new ReportElementType(typeof(ReportGraphElement), ReportElementTag.GraphElement);
        public static readonly ReportElementType DEFAULTCUSTOM = new ReportElementType(typeof(ReportCustomElement), ReportElementTag.CustomElement);
        public static readonly ReportElementType DEFAULTREPORT = new ReportElementType(typeof(ReportReportElement), ReportElementTag.ReportElement);
        public static readonly ReportElementType DEFAULTINFO = new ReportElementType(typeof(ReportInfoElement), ReportElementTag.InfoElement);
        #endregion

        #region
        public ReportElementType(Type elementType)
        {
            this.m_elementTag = ReportElementTag.NoneElement;
            this.m_elementType = elementType;
        }
        public ReportElementType(Type elementType, ReportElementTag elementTag)
            : this(elementType)
        {
            this.m_elementTag = elementTag;
        }
        public ReportElementType(string elementName, Type elementType)
            : this(elementType)
        {
            this.m_elementName = elementName;
        }
        public ReportElementType(string typeName)
        {
            this.m_elementTag = ReportElementTag.NoneElement;
            this.m_elementType = SystemInfo.GetTypeFromString(typeName, true, true);
        }
        #endregion

        #region
        public string ElementName
        {
            get
            {
                if (this.m_elementName != null && !this.m_elementName.Equals(""))
                {
                    return this.m_elementName.ToLower();
                }
                else
                {
                    return this.m_elementType.FullName.ToLower();
                }
            }
            set { this.m_elementName = value; }
        }
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
            set { this.m_elementTag = value; }
        }
        public Type ElementType
        {
            get { return this.m_elementType; }
        }
        #endregion
    }
}
