using System;
using System.Collections;

using XYS.Util;
using XYS.Report;
namespace XYS.Lis.Report.Model
{
    public class LabReport : IReportElement
    {
        #region 私有字段
        private ReportPK m_reportPK;
        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 构造方法
        public LabReport()
        {
            this.m_reportItemTable = SystemInfo.CreateCaseInsensitiveHashtable(3);
        }
        #endregion

        #region 实现接口方法
        public IReportKey RK
        {
            get { return this.m_reportPK; }
        }
        #endregion

        #region 实例属性
        public ReportPK ReportPK
        {
            get { return this.m_reportPK; }
            set { this.m_reportPK = value; }
        }
        public Hashtable ItemTable
        {
            get { return this.m_reportItemTable; }
        }
        #endregion
    }
}
