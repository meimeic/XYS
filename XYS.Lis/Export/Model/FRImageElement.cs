using System;

using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    [Export()]
    public class FRImageElement:IExportElement
    {
        private byte[] m_image1;
        private byte[] m_image2;
        private byte[] m_image3;
        private byte[] m_image4;
        private byte[] m_image5;
        private byte[] m_image6;
        private byte[] m_image7;
        private byte[] m_image8;

        public FRImageElement()
        {
            this.m_image1 = null;
            this.m_image2 = null;
            this.m_image3 = null;
            this.m_image4 = null;
            this.m_image5 = null;
            this.m_image6 = null;
            this.m_image7 = null;
            this.m_image8 = null;
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
    }
}
