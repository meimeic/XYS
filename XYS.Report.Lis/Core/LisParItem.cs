using System;
namespace XYS.Report.Lis.Core
{
    public class LisParItem
    {
        #region 字段
        private readonly int m_parItemNo;

        private bool m_imageFlag;
        private string m_imageName;
        private string m_parItemName;
        #endregion

        #region 构造函数
        public LisParItem(int itemNo)
            : this(itemNo, null)
        {
        }
        public LisParItem(int itemNo, string parItemName)
        {
            this.m_imageFlag = false;
            this.m_parItemNo = itemNo;
            this.m_parItemName = parItemName;
        }
        #endregion

        #region 属性
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
        }
        public bool ImageFlag
        {
            get { return this.m_imageFlag; }
            set { this.m_imageFlag = value; }
        }
        public string ImageName
        {
            get { return this.m_imageName; }
            set { this.m_imageName = value; }
        }
        #endregion
    }
}
