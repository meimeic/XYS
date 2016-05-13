using XYS.Report;
using XYS.Common;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportCustomElement : IFillElement
    {
        #region 私有字段
        private string m_c0;
        private string m_c1;
        private string m_c2;
        private string m_c3;
        private string m_c4;
        private string m_c5;
        private string m_c6;
        private string m_c7;
        private string m_c8;
        private string m_c9;
        #endregion

        #region 公共构造函数
        public ReportCustomElement()
        {
        }
        #endregion

        #region 公共属性
        [BsonElement(Order = 2)]
        public string C0
        {
            get { return this.m_c0; }
            set { this.m_c0 = value; }
        }

        [BsonElement(Order = 3)]
        public string C1
        {
            get { return this.m_c1; }
            set { this.m_c1 = value; }
        }

        [BsonElement(Order = 4)]
        public string C2
        {
            get { return this.m_c2; }
            set { this.m_c2 = value; }
        }

        [BsonElement(Order = 5)]
        public string C3
        {
            get { return this.m_c3; }
            set { this.m_c3 = value; }
        }

        [BsonElement(Order = 6)]
        public string C4
        {
            get { return this.m_c4; }
            set { this.m_c4 = value; }
        }

        [BsonElement(Order = 7)]
        public string C5
        {
            get { return this.m_c5; }
            set { this.m_c5 = value; }
        }

        [BsonElement(Order = 8)]
        public string C6
        {
            get { return this.m_c6; }
            set { this.m_c6 = value; }
        }

        [BsonElement(Order = 9)]
        public string C7
        {
            get { return this.m_c7; }
            set { this.m_c7 = value; }
        }

        [BsonElement(Order = 10)]
        public string C8
        {
            get { return this.m_c8; }
            set { this.m_c8 = value; }
        }

        [BsonElement(Order = 11)]
        public string C9
        {
            get { return this.m_c9; }
            set { this.m_c9 = value; }
        }
        #endregion
    }
}