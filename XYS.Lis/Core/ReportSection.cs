using System;
using System.Collections.Generic;

using XYS.Lis.Fill;
namespace XYS.Lis.Core
{
    public class ReportSection
    {
        #region 字段
        private int m_orderNo;
        private int m_ModelNo;
        private FillTypeTag m_fillTag;
        private string m_sectionName;
        private readonly int m_sectionNo;
        private readonly List<string> m_insideElementList;
        private ElementTypeCollection m_elementCollection;
        #endregion

        #region 构造函数
        public ReportSection(int sectionNo)
        {
            this.m_orderNo = 0;
            this.m_ModelNo = -1;
            this.m_sectionNo = sectionNo;
            this.m_fillTag = FillTypeTag.DB;
            this.m_insideElementList = new List<string>(3);
            //this.m_elementCollection = new ElementTypeCollection(3);
        }
        public ReportSection(int sectionNo, string sectionName)
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
        public FillTypeTag FillTag
        {
            get { return this.m_fillTag; }
            set { this.m_fillTag = value; }
        }
        public string SectionName
        {
            get { return this.m_sectionName; }
            set { this.m_sectionName = value; }
        }
        public List<string> InsideElementList
        {
            get { return this.m_insideElementList; }
        }
        public ElementTypeCollection ElementCollection
        {
            get { return this.m_elementCollection; }
        }
        #endregion

        #region 方法
        public void AddElement(ElementType element)
        {
            if (!this.m_elementCollection.Contains(element))
            {
                this.m_elementCollection.Add(element);
            }
        }
        public void AddElement(string elementName)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                string name = elementName.ToLower();
                if (!this.m_insideElementList.Contains(name))
                {
                    this.m_insideElementList.Add(name);
                }
            }
        }
        public void ClearElementCollection()
        {
            this.m_elementCollection.Clear();
        }
        #endregion
    }
}
