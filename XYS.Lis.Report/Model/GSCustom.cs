using System;

using XYS.Common;
using XYS.Report;
namespace XYS.Lis.Report.Model
{
    public class GSCustom : IFillElement
    {
        private string m_reportID;
        private int m_itemNo;
        private string m_reportDescribe;
        private string m_reportComment;
        private int m_isFile;
        private byte[] m_graph;
        private string m_graphFileName;
        private DateTime m_graphFileTime;

        public GSCustom()
        { }

        [Column]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }
        [Column]
        public string ReportDescribe
        {
            get { return this.m_reportDescribe; }
            set { this.m_reportDescribe = value; }
        }
        [Column]
        public string ReportComment
        {
            get { return this.m_reportComment; }
            set { this.m_reportComment = value; }
        }
        [Column]
        public int IsFile
        {
            get { return this.m_isFile; }
            set { this.m_isFile = value; }
        }
        public byte[] Graph
        {
            get { return this.m_graph; }
            set { this.m_graph = value; }
        }
        [Column]
        public string GraphFileName
        {
            get { return this.m_graphFileName; }
            set { this.m_graphFileName = value; }
        }
        [Column]
        public DateTime GraphFileTime
        {
            get { return this.m_graphFileTime; }
            set { this.m_graphFileTime = value; }
        }
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
    }
}
