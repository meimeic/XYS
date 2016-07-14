using System;
using XYS.Report;

namespace XYS.Lis.Report.Model
{
    public class ImageElement : IFillElement
    {
        private string m_reportID;
        private string m_name;
        private string m_url;

        public ImageElement()
        { }
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
    }
}
