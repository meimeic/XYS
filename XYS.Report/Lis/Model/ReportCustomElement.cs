using XYS.Report;
using XYS.Common;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportCustomElement : AbstractFillElement
    {
        #region 私有字段
        private string m_name;
        private string m_column0;
        private string m_column1;
        private string m_column2;
        private string m_column3;
        private string m_column4;
        private string m_column5;
        private string m_column6;
        private string m_column7;
        private string m_column8;
        private string m_column9;
        #endregion

        #region 公共构造函数
        public ReportCustomElement()
        {
        }
        #endregion

        #region 公共属性
        [BsonElement(Order = 1)]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        [BsonElement(Order = 2)]
        public string Column0
        {
            get { return this.m_column0; }
            set { this.m_column0 = value; }
        }
        [BsonElement(Order = 3)]
        public string Column1
        {
            get { return this.m_column1; }
            set { this.m_column1 = value; }
        }
        [BsonElement(Order = 4)]
        public string Column2
        {
            get { return this.m_column2; }
            set { this.m_column2 = value; }
        }
        [BsonElement(Order = 5)]
        public string Column3
        {
            get { return this.m_column3; }
            set { this.m_column3 = value; }
        }
        [BsonElement(Order = 6)]
        public string Column4
        {
            get { return this.m_column4; }
            set { this.m_column4 = value; }
        }
        [BsonElement(Order = 7)]
        public string Column5
        {
            get { return this.m_column5; }
            set { this.m_column5 = value; }
        }
        [BsonElement(Order = 8)]
        public string Column6
        {
            get { return this.m_column6; }
            set { this.m_column6 = value; }
        }
        [BsonElement(Order = 9)]
        public string Column7
        {
            get { return this.m_column7; }
            set { this.m_column7 = value; }
        }
        [BsonElement(Order = 10)]
        public string Column8
        {
            get { return this.m_column8; }
            set { this.m_column8 = value; }
        }
        [BsonElement(Order = 11)]
        public string Column9
        {
            get { return this.m_column9; }
            set { this.m_column9 = value; }
        }
        #endregion
    }
}