using System;
using System.Collections.Generic;

using XYS.Model.Lab;
namespace XYS.Model
{
    [Serializable]
    public class LabReport
    {
        #region 私有字段
        private InfoElement m_info;
        private List<int> m_superList;
        private List<ItemElement> m_itemList;
        private List<ImageElement> m_imageList;
        private List<CustomElement> m_customList;
        #endregion

        #region 构造方法
        public LabReport()
        {
            this.m_info = new InfoElement();
            this.m_superList = new List<int>(8);
            this.m_itemList = new List<ItemElement>(16);
            this.m_imageList = new List<ImageElement>(8);
            this.m_customList = new List<CustomElement>(3);
        }
        #endregion

        #region
        public InfoElement Info
        {
            get { return this.m_info; }
            set { this.m_info = value; }
        }
        public List<int> SuperList
        {
            get { return this.m_superList; }
        }
        public List<ItemElement> ItemList
        {
            get { return this.m_itemList; }
        }
        public List<ImageElement> ImageList
        {
            get { return this.m_imageList; }
        }
        public List<CustomElement> CustomList
        {
            get { return this.m_customList; }
        }
        #endregion
    }
}
