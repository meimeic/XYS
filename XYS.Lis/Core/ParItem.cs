using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Core
{
    public class ParItem
    {
        #region
        private string m_parItemName;
        private int m_parItemNo;
        private int m_printModelNo;
        private int m_orderNo;
        private int m_imageFlag;
        private string m_imagePath;
        #endregion
        
        #region
        public ParItem(int itemNo)
        {
            this.m_parItemNo = itemNo;
            this.m_printModelNo = -1;
            this.m_orderNo = 0;
            this.m_imageFlag = 0;
        }
        public ParItem(int itemNo, int printModelNo, int orderNo)
            : this(itemNo)
        {
            this.m_printModelNo = printModelNo;
            this.m_orderNo = orderNo;
        }
        public ParItem(int itemNo, int printModelNo, int orderNo, int imageFlag, string imagePath)
            : this(itemNo, printModelNo, orderNo)
        {
            this.m_imageFlag = imageFlag;
            this.m_imagePath = imagePath;
        }
        public ParItem(int itemNo, int printModelNo, int orderNo, int imageFlag, string imagePath, string parItemName)
            : this(itemNo, printModelNo, orderNo, imageFlag, imagePath)
        {
            this.m_parItemName = parItemName;
        }
        #endregion

        #region
        public string ParItemName
        {
            get { return this.m_parItemName; }
        }
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
        }
        public int ImageFlag
        {
            get { return this.m_imageFlag; }
        }
        public string ImagePath
        {
            get { return this.m_imagePath; }
        }
        #endregion
    }
}
