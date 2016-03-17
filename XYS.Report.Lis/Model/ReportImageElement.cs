using System;
using System.Collections.Generic;

namespace XYS.Report.Lis.Model
{
    public class ReportImageElement : AbstractInnerElement
    {
        #region 私有字段
        private string m_name;
        private Dictionary<string, string> m_imageMap;
        #endregion

        #region 构造函数
        public ReportImageElement()
            : base()
        {
            this.m_imageMap = null;
            this.m_name = "ImageCollection";
        }
        #endregion

        #region 实例属性

        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Dictionary<string, string> ImageMap
        {
            get { return this.m_imageMap; }
        }
        public void AddImage(string imageName, string imageUrl)
        {
            if (this.m_imageMap == null)
            {
                this.m_imageMap = new Dictionary<string, string>(2);
            }
            this.m_imageMap[imageName] = imageUrl;
        }
        #endregion
    }
}
