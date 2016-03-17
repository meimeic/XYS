using System;
using XYS.Common;
namespace XYS.Report.Lis.Model
{
    [Export()]
    public class ReportImageByteElement : AbstractInnerElement
    {
        #region 私有字段
        private string m_name;
        private byte[] m_image0;
        private byte[] m_image1;
        private byte[] m_image2;
        private byte[] m_image3;
        private byte[] m_image4;
        private byte[] m_image5;
        private byte[] m_image6;
        private byte[] m_image7;
        private byte[] m_image8;
        #endregion

        #region 构造函数
        public ReportImageByteElement()
            : base()
        {
            this.m_image0 = null;
            this.m_image1 = null;
            this.m_image2 = null;
            this.m_image3 = null;
            this.m_image4 = null;
            this.m_image5 = null;
            this.m_image6 = null;
            this.m_image7 = null;
            this.m_image8 = null;
            this.m_name = "ImageByteCollection";
        }
        #endregion

        #region 实例属性
        [Export()]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        [Export()]
        public byte[] Image0
        {
            get { return this.m_image0; }
            set { this.m_image0 = value; }
        }
        [Export()]
        public byte[] Image1
        {
            get { return this.m_image1; }
            set { this.m_image1 = value; }
        }
        [Export()]
        public byte[] Image2
        {
            get { return this.m_image2; }
            set { this.m_image2 = value; }
        }
        [Export()]
        public byte[] Image3
        {
            get { return this.m_image3; }
            set { this.m_image3 = value; }
        }
        [Export()]
        public byte[] Image4
        {
            get { return this.m_image4; }
            set { this.m_image4 = value; }
        }
        [Export()]
        public byte[] Image5
        {
            get { return this.m_image5; }
            set { this.m_image5 = value; }
        }
        [Export()]
        public byte[] Image6
        {
            get { return this.m_image6; }
            set { this.m_image6 = value; }
        }
        [Export()]
        public byte[] Image7
        {
            get { return this.m_image7; }
            set { this.m_image7 = value; }
        }
        [Export()]
        public byte[] Image8
        {
            get { return this.m_image8; }
            set { this.m_image8 = value; }
        }
        #endregion
    }
}
