using System;
using MongoDB.Bson.Serialization.Attributes;

namespace XYS.Mongo.Lab.Model
{
    public class Item
    {
        #region 私有字段
        private int m_itemNo;
        private int m_superNo;
        private string m_cName;
        private string m_eName;
        private string m_result;
        private string m_status;
        private string m_unit;
        private string m_refRange;
        #endregion

        public Item()
        {
        }

        #region 公共属性
        [BsonElement("itemNo", Order = 1)]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }
        [BsonElement("superNo", Order = 2)]
        public int SuperNo
        {
            get { return this.m_superNo; }
            set { this.m_superNo = value; }
        }
        [BsonElement("cname", Order = 3)]
        public string CName
        {
            get { return this.m_cName; }
            set { this.m_cName = value; }
        }
        [BsonElement("ename", Order = 4)]
        public string EName
        {
            get { return this.m_eName; }
            set { this.m_eName = value; }
        }
        [BsonElement("result", Order = 5)]
        public string Result
        {
            get { return this.m_result; }
            set { this.m_result = value; }
        }
        [BsonElement("status", Order = 6)]
        public string Status
        {
            get { return this.m_status; }
            set { this.m_status = value; }
        }
        [BsonElement("unit", Order = 7)]
        public string Unit
        {
            get { return this.m_unit; }
            set { this.m_unit = value; }
        }
        [BsonElement("refRange", Order = 8)]
        public string RefRange
        {
            get { return this.m_refRange; }
            set { this.m_refRange = value; }
        }
        #endregion
    }
}