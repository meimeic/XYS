using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using XYS.Model;
namespace XYS.Lis.Service.Models.Report
{
    public class ReportGraph : IReportModel
    {
        private static readonly ReportElementTag Default_Element = ReportElementTag.GraphElement;

        private string m_graphName;
        private byte[] m_graphImage;
        private string m_graphValue;
        private string m_graphSrc;

        public ReportGraph()
        { }

        #region 实现ireportmodel接口
        [JsonIgnore]
        public ReportElementTag ElementTag
        {
            get { return Default_Element; }
        }
        #endregion

        #region
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }
        [JsonIgnore]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        public string GraphValue
        {
            get { return this.m_graphValue; }
            set { this.m_graphValue = value; }
        }
        public string GraphSrc
        {
            get { return this.m_graphSrc; }
            set { this.m_graphSrc = value; }
        }
        #endregion
    }
}
