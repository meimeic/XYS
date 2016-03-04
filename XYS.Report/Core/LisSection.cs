using System;
using System.Collections.Generic;
namespace XYS.Report.Core
{
    public class LisSection
    {
         #region 字段
        private int m_orderNo;
        private int m_ModelNo;
        private string m_sectionName;
        private readonly int m_sectionNo;
        private readonly List<string> m_innerElementList;
        private readonly List<string> m_extendElementList;
        #endregion

        #region 构造函数
        public LisSection(int sectionNo)
        {
            this.m_orderNo = 0;
            this.m_ModelNo = -1;
            this.m_sectionNo = sectionNo;
            this.m_innerElementList = new List<string>(3);
            this.m_extendElementList = new List<string>(2);
        }
        public LisSection(int sectionNo, string sectionName)
            : this(sectionNo)
        {
            this.m_sectionName = sectionName;
        }
        #endregion

        #region 属性
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        public int ModelNo
        {
            get { return this.m_ModelNo; }
            set { this.m_ModelNo = value; }
        }
        public int SectionNo
        {
            get { return this.m_sectionNo; }
        }
        public string SectionName
        {
            get { return this.m_sectionName; }
            set { this.m_sectionName = value; }
        }
        public List<string> InnerElementList
        {
            get { return this.m_innerElementList; }
        }
        public List<string> ExtendElementList
        {
            get { return this.m_extendElementList; }
        }
        #endregion

        #region 方法
        public void AddInnerElement(string elementName)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                if (!this.m_innerElementList.Contains(elementName))
                {
                    this.m_innerElementList.Add(elementName);
                }
            }
        }
        public void AddExtendElement(string elementName)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                if (!this.m_extendElementList.Contains(elementName))
                {
                    this.m_extendElementList.Add(elementName);
                }
            }
        }
        public void ClearElements()
        {
            this.m_innerElementList.Clear();
            this.m_extendElementList.Clear();
        }
        #endregion
    }
}
