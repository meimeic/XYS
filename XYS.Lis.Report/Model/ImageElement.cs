using System;
using XYS.Report;
using XYS.Common;

namespace XYS.Lis.Report.Model
{
    public class ImageElement : IFillElement
    {
        #region
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
    }
}
