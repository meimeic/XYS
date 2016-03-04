using System;
namespace XYS.Report.Core
{
    public class LisParItem
    {
        #region
        private readonly int m_parItemNo;
        private string m_parItemName;
        private int m_ModelNo;
        private int m_orderNo;
        private bool m_imageFlag;
        private string m_imagePath;
        #endregion

        #region
        public LisParItem(int itemNo)
        {
            this.m_parItemNo = itemNo;
            this.m_ModelNo = -1;
            this.m_orderNo = 0;
            this.m_imageFlag = false;
        }
        public LisParItem(int itemNo, string parItemName)
            : this(itemNo)
        {
            this.m_parItemName = parItemName;
        }
        #endregion

        #region
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
        }
        public int ModelNo
        {
            get { return this.m_ModelNo; }
            set { this.m_ModelNo = value; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        public bool ImageFlag
        {
            get { return this.m_imageFlag; }
            set { this.m_imageFlag = value; }
        }
        public string ImagePath
        {
            get { return this.m_imagePath; }
            set { this.m_imagePath = value; }
        }
        #endregion
    }
}
