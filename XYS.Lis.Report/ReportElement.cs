using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Model.Lab;
namespace XYS.Lis.Report
{
    internal class ReportElement
    {
        #region 私有字段
        private IFillElement m_info;
        private LabPK m_reportPK;
        private List<IFillElement> m_itemList;
        private List<IFillElement> m_imageList;
        #endregion

        #region 构造方法
        public ReportElement()
        {
            this.m_info = new InfoElement();
            this.m_itemList = new List<IFillElement>(16);
            this.m_imageList = new List<IFillElement>(8);
        }
        #endregion

        #region 实例属性
        public IFillElement Info
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
        public LabPK ReportPK
        {
            get { return this.m_reportPK; }
            set { this.m_reportPK = value; }
        }
        #endregion
    }
}