using System;
using System.Collections.Generic;
namespace XYS.Lis.Core
{
    public class ReporterSection
    {
        #region
        private readonly int m_sectionNo;
        private string m_sectionName;
        private int m_ModelNo;
        private int m_orderNo;
        private List<string> m_elementNameList;
        #endregion

        #region 
        public ReporterSection(int sectionNo)
        {
            this.m_sectionNo = sectionNo;
            this.m_elementNameList = new List<string>(5);
            this.m_ModelNo = -1;
            this.m_orderNo = 0;
        }
        public ReporterSection(int sectionNo, string sectionName)
            : this(sectionNo)
        {
            this.m_sectionName = sectionName;
        }
        #endregion

        #region 
        public int SectionNo
        {
            get { return this.m_sectionNo; }
        }
        public string SectionName
        {
            get { return this.m_sectionName; }
            set { this.m_sectionName = value; }
        }
        public int ModelNo
        {
            get { return this.m_ModelNo; }
            set { this.m_ModelNo = value; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        public List<string> ElementNameList
        {
            get { return this.m_elementNameList; }
        }
        #endregion

        #region
        public void AddElementName(string elementName)
        {
            if (!this.m_elementNameList.Contains(elementName))
            {
                this.m_elementNameList.Add(elementName);
            }
        }
        public void ClearElementNameList()
        {
            this.m_elementNameList.Clear();
        }
        #endregion
    }
}
