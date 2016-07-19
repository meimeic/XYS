using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
namespace XYS.Lis.Report.Model
{
    public class LabReport : IReportElement
    {
        #region 私有字段
        private ReportPK m_reportPK;
        private InfoElement m_info;
        private List<IFillElement> m_itemList;
        private List<IFillElement> m_imageList;
        private List<IFillElement> m_customList;
        #endregion

        #region 构造方法
        public LabReport()
        {
            this.m_info = new InfoElement();
            this.m_itemList = new List<IFillElement>(16);
            this.m_imageList = new List<IFillElement>(8);
            this.m_customList = new List<IFillElement>(4);
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
        public InfoElement Info
        {
            get { return this.m_info; }
            set { this.m_info = value; }
        }
        public List<IFillElement> ItemList
        {
            get { return this.m_itemList; }
            set { this.m_itemList = value; }
        }
        public List<IFillElement> ImageList
        {
            get { return this.m_imageList; }
            set { this.m_imageList = value; }
        }
        public List<IFillElement> CustomList
        {
            get { return this.m_customList; }
            set { this.m_customList = value; }
        }
        #endregion
    }
}
