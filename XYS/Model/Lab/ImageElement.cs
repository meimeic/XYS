using System;
using XYS.Report;
using XYS.Common;

namespace XYS.Model.Lab
{
    [Serializable]
    public class ImageElement : IFillElement, IComparable<ImageElement>
    {
        #region 私有字段
        private string m_reportID;
        private string m_name;
        private string m_url;
        #endregion
        
        #region
        public ImageElement()
        { }
        #endregion
        
        #region
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public string Url
        {
            get { return this.m_url; }
            set { this.m_url = value; }
        }
        #endregion

        public int CompareTo(ImageElement other)
        {
            if (other == null)
            {
                return -1;
            }
            return String.CompareOrdinal(this.Name, other.Name);
        }
    }
}
