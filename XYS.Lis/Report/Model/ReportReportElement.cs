using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Report;
using XYS.Common;

namespace XYS.Lis.Report.Model
{
    public class ReportReportElement : IReportElement
    {
        #region 私有实例字段

        private ReportPK m_reportPK;
        private HandleResult m_handleResult;

        //报告名
        private string m_reportTitle;
        private DateTime m_createTime;
        //附加备注
        private int m_remarkFlag;
        private string m_remark;

        private int m_reportNo;
        private ReportInfoElement m_reportInfo;
        private Dictionary<string, string> m_reportImageMap;
        private List<ReportItemElement> m_reportItemCollection;

        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
        {
            this.m_remarkFlag = 0;
            this.m_handleResult = new HandleResult();
            this.m_reportInfo = new ReportInfoElement();
            this.m_reportItemCollection = new List<ReportItemElement>(20);
            this.m_reportItemTable = SystemInfo.CreateCaseInsensitiveHashtable(3);
        }
        #endregion

        #region 实现IReportElement接口
        public IReportKey RK
        {
            get { return this.m_reportPK; }
        }
        public HandleResult HandleResult
        {
            get { return this.m_handleResult; }
        }
        #endregion

        #region 实例属性
        public string ReportID
        {
            get
            {
                if (this.ReportPK == null)
                {
                    return "unkown";
                }
                else
                {
                    return this.ReportPK.ID;
                }
            }
        }
        public ReportPK ReportPK
        {
            get { return this.m_reportPK; }
            set { this.m_reportPK = value; }
        }

        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }

        //备注标识 0表示没有备注 1 表示备注已设置  2 表示备注未设置
        public int RemarkFlag
        {
            get { return this.m_remarkFlag; }
            set { this.m_remarkFlag = value; }
        }

        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }

        public int ReportNo
        {
            get { return this.m_reportNo; }
            set { this.m_reportNo = value; }
        }

        public ReportInfoElement ReportInfo
        {
            get { return this.m_reportInfo; }
            set { this.m_reportInfo = value; }
        }

        public List<ReportItemElement> ReportItemCollection
        {
            get { return this.m_reportItemCollection; }
            set { this.m_reportItemCollection = value; }
        }

        public Dictionary<string, string> ReportImageMap
        {
            get { return this.m_reportImageMap; }
            set { this.m_reportImageMap = value; }
        }
        #endregion

        #region 公共方法
        public void Init()
        {
        }
        public void Clear()
        {
            this.HandleResult.Clear();
            this.ReportImageMap = null;
            this.m_reportItemTable.Clear();
            this.ReportItemCollection.Clear();
            //非填充项清空

            //
            this.ReportPK = null;
            this.ReportTitle = "";
            this.RemarkFlag = 0;
            this.Remark = "";
        }
        public List<IFillElement> GetReportItem(Type type)
        {
            if (type != null)
            {
                List<IFillElement> result = this.m_reportItemTable[type] as List<IFillElement>;
                if (result == null)
                {
                    result = new List<IFillElement>(10);
                    lock (this.m_reportItemTable)
                    {
                        this.m_reportItemTable[type] = result;
                    }
                }
                return result;
            }
            return null;
        }
        #endregion
    }
}