using System;
namespace XYS.Report.Lis.Util
{
    public class LisParItem
    {
        #region 字段
        private string m_imageName;
        private string m_parItemName;
        private readonly int m_parItemNo;
        #endregion

        #region 构造函数
        public LisParItem(int itemNo)
            : this(itemNo, null)
        {
        }
        public LisParItem(int itemNo, string parItemName)
        {
            this.m_parItemNo = itemNo;
            this.m_parItemName = parItemName;
        }
        #endregion

        #region 属性
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
        }
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }
        public string ImageName
        {
            get { return this.m_imageName; }
            set { this.m_imageName = value; }
        }
        #endregion
    }
}
