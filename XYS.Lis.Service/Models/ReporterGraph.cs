using System;
using System.Collections.Generic;
using Newtonsoft.Json; 

namespace XYS.Lis.Model.Export
{
    public class ReporterGraph
    { 
        private string m_graphName;
        private byte[] m_graphImage;
        private string m_graphValue;
        private string m_graphPath;
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
            get { return this.m_graphPath; }
            set { this.m_graphPath = value; }
        }
    }
}
