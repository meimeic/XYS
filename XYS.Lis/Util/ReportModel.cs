using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Util
{
    public class ReportModel
    {
        #region
        private readonly int m_modelNo;
        private string m_modelName;
        private string m_modelPath;
        #endregion

        #region
        public ReportModel(int modelNo, string modelPath)
        {
            this.m_modelNo = modelNo;
            this.m_modelPath = modelPath;
        }
        public ReportModel(int modelNo, string modelPath, string modelName)
            : this(modelNo, modelPath)
        {
            this.m_modelName = modelName;
        }
        #endregion

        #region
        public int ModelNo
        {
            get { return this.m_modelNo; }
        }
        public string ModelName
        {
            get { return this.m_modelName; }
            set { this.m_modelName = value; }
        }
        public string ModelPath
        {
            get { return this.m_modelPath; }
            set { this.m_modelPath = value; }
        }
        #endregion
    }
}
