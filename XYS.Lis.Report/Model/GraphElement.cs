using System;

using XYS.Report;
using XYS.Common;
namespace XYS.Lis.Report.Model
{
    public class GraphElement : IFillElement
    {
        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public GraphElement()
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

        public string ReportID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
