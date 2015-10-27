using System;
using System.Collections.Generic;
namespace XYS.Lis.Core
{
    public class ReporterSection
    {
        #region
        private readonly int m_sectionNo;
        private string m_sectionName;
        private int m_printModelNo;
        private int m_orderNo;
        private List<string> m_elementNameList;
        #endregion

        #region 
        public ReporterSection(int sectionNo)
        {
            this.m_sectionNo = sectionNo;
            this.m_elementNameList = new List<string>(5);
            this.m_printModelNo = -1;
            this.m_orderNo = 0;
        }
        public ReporterSection(int sectionNo, string sectionName)
            : this(sectionNo)
        {
            this.m_sectionName = sectionName;
        }
        public ReporterSection(int sectionNo, int printModelNo, int orderNo)
            : this(sectionNo)
        {
            this.m_printModelNo = printModelNo;
            this.m_orderNo = orderNo;
        }
        public ReporterSection(int sectionNo, int printModelNo, int orderNo, string sectionName)
            : this(sectionNo,printModelNo,orderNo)
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
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
        }
        public List<string> ElementNameList
        {
            get { return this.m_elementNameList; }
        }
        #endregion

        #region
        public void AddElementTag(string elementName)
        {
            if (!this.m_elementNameList.Contains(elementName))
            {
                this.m_elementNameList.Add(elementName);
            }
        }
        public void ClearElementTagList()
        {
            this.m_elementNameList.Clear();
        }
        #endregion
    }
}
