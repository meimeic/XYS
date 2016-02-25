using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Util
{
    public class PrintModel
    {
        #region
        private readonly int m_modelNo;
        private readonly string m_modelName;
        private string m_name;
        #endregion

        #region
        public PrintModel(int modelNo, string modelName)
        {
            this.m_modelNo = modelNo;
            this.m_modelName = modelName;
        }
        public PrintModel(int modelNo, string modelName, string name)
            : this(modelNo, modelName)
        {
            this.m_name = name;
        }
        #endregion

        #region
        public int ModelNo
        {
            get { return this.m_modelNo; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public string ModelName
        {
            get { return this.m_modelName; }
        }
        #endregion
    }
}
