using System;

using XYS.Report;
using XYS.Common;
namespace XYS.Lis.Report.Model
{
    public class GeneCustom : IFillElement
    {
        private string m_no;
        private string m_age;
        private string m_name;
        private string m_count;
        private string m_relation;
        private string m_reportID;

        [Column]
        public string Age
        {
            get { return this.m_age; }
            set { this.m_age = value; }
        }
        [Column]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        [Column]
        public string SampleNo
        {
            get { return this.m_no; }
            set { this.m_no = value; }
        }
        [Column]
        public string EqualCount
        {
            get { return this.m_count; }
            set { this.m_count = value; }
        }
        [Column]
        public string Relation
        {
            get { return this.m_relation; }
            set { this.m_relation = value; }
        }
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
    }
}
