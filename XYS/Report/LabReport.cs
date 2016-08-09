using System;
using System.Collections.Generic;

using XYS.Model.Lab;
namespace XYS.Report
{
    [Serializable]
    public class LabReport
    {
        #region 私有字段
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
