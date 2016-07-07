using System;

using XYS.Report;
using XYS.Common;
namespace XYS.Report.Lis.Model
{
    public class ReportGraphElement : IFillElement
    {
        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public ReportGraphElement()
        {
        }
        #endregion

        #region 公共属性
        [Column()]
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }

        [Column()]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        #endregion
    }
}
