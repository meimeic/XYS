using System;
using System.Collections.Generic;

using XYS.Lis.Report.Model;
namespace XYS.Lis.Report
{
    public class LabReport
    {
        #region 私有字段
        private LabPK m_reportPK;
        private InfoElement m_info;
        private List<ItemElement> m_itemList;
        private List<ImageElement> m_imageList;
        #endregion

        #region 构造方法
        public LabReport()
        {
            this.m_info = new InfoElement();
            this.m_itemList = new List<ItemElement>(16);
            this.m_imageList = new List<ImageElement>(8);
        }
        #endregion

        #region
        public LabPK ReportPK
        {
            get { return this.m_reportPK; }
            set { this.m_reportPK = value; }
        }
        public InfoElement Info
        {
            get { return this.m_info; }
            set { this.m_info = value; }
        }
        public List<ItemElement> ItemList
        {
            get { return this.m_itemList; }
            set { this.m_itemList = value; }
        }
        public List<ImageElement> ImageList
        {
            get { return this.m_imageList; }
            set { this.m_imageList = value; }
        }
        #endregion
    }
}
