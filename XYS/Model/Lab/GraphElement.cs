using System;

using XYS.Report;
using XYS.Common;
namespace XYS.Model.Lab
{
    [Serializable]
    public class GraphElement : IFillElement
    {
        #region 私有字段
        private string m_reportID;
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public GraphElement()
        {
        }
        #endregion

        #region 公共属性
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
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