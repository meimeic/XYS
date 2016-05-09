using System;

namespace XYS.FR.Lis.Util
{
    public class LisPrintModel
    {
        #region
        private string m_name;
        private readonly int m_no;
        private string m_path;
        #endregion

        public LisPrintModel(int no)
            : this(no, null)
        {
        }
        public LisPrintModel(int no, string name)
        {
            this.m_no = no;
            this.m_name = name;
        }

        #region
        public int No
        {
            get { return this.m_no; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public string Path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }
        #endregion
    }
}
