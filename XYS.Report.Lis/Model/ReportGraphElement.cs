using System;

using XYS.Report;
using XYS.Common;
namespace XYS.Report.Lis.Model
{
    public class ReportGraphElement : IFillElement, IComparable<ReportGraphElement>
    {
        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public ReportGraphElement()
        { }
        #endregion

        #region 公共属性
        [Column(true)]
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }

        [Column(true)]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        #endregion

       #region 实现比较方法
        public int CompareTo(ReportGraphElement element)
        {
            if (element == null)
            {
                return 1;
            }
            else
            {
                return this.GraphName.CompareTo(element.GraphName);
            }
        }
       #endregion
    }
}
