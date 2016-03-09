using System;
using System.Collections.Generic;
namespace XYS.Report.Lis.Core
{
    public class LisSection
    {
        #region 字段
        private string m_sectionName;
        private readonly int m_sectionNo;
        private readonly List<string> m_fillElementList;
        #endregion

        #region 构造函数
        public LisSection(int sectionNo)
            : this(sectionNo, null)
        {
        }
        public LisSection(int sectionNo, string sectionName)
        {
            this.m_sectionNo = sectionNo;
            this.m_sectionName = sectionName;
            this.m_fillElementList = new List<string>(3);
        }
        #endregion

        #region 属性
        public int SectionNo
        {
            get { return this.m_sectionNo; }
        }
        public string SectionName
        {
            get { return this.m_sectionName; }
            set { this.m_sectionName = value; }
        }
        public List<string> FillElementList
        {
            get { return this.m_fillElementList; }
        }
        #endregion

        #region 方法
        public void AddFillElement(string elementName)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                if (!this.m_fillElementList.Contains(elementName))
                {
                    this.m_fillElementList.Add(elementName);
                }
            }
        }
        public void ClearElements()
        {
            this.m_fillElementList.Clear();
        }
        #endregion
    }
}
