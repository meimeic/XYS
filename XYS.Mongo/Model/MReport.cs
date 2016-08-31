using System;
using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Mongo.Model
{
    public class MReport
    {
        private ObjectId m_id;
        private int m_activeFlag;
        private string m_reportID;
        private string m_reportTitle;
        private DateTime m_createTime;

        private int m_reportNo;

        private Info m_info;
        private List<Item> m_itemCollection;
        private List<Custom> m_customCollection;
        private Dictionary<string, string> m_imageMap;

        public MReport()
        {
            this.m_info = new Info();
            this.m_itemCollection = new List<Item>(30);
            this.m_customCollection = new List<Custom>(3);
            this.m_imageMap = new Dictionary<string, string>(4);
        }


        [BsonId]
        public ObjectId ID
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }
        [BsonElement("reportID", Order = 1)]
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
        [BsonElement("activeFlag", Order = 2)]
        public int ActiveFlag
        {
            get { return m_activeFlag; }
            set { m_activeFlag = value; }
        }
        [BsonElement("reportTitle", Order = 3)]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        [BsonElement("createTime", Order = 4)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime
        {
            get
            {
                if (this.m_createTime == default(DateTime))
                {
                    this.m_createTime = DateTime.Now;
                }
                return this.m_createTime;
            }
            set { this.m_createTime = value; }
        }
        [BsonElement("reportNo", Order = 6)]
        public int ReportNo
        {
            get { return this.m_reportNo; }
            set { this.m_reportNo = value; }
        }

        [BsonElement("info", Order = 7)]
        public Info Info
        {
            get { return this.m_info; }
            set { this.m_info = value; }
        }
        [BsonElement("items", Order = 8)]
        public List<Item> ItemCollection
        {
            get { return this.m_itemCollection; }
            set { this.m_itemCollection = value; }
        }
        [BsonElement("customs", Order = 9)]
        public List<Custom> CustomCollection
        {
            get { return this.m_customCollection; }
            set { this.m_customCollection = value; }
        }
        [BsonElement("images", Order = 10)]
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, string> ImageMap
        {
            get { return this.m_imageMap; }
            set { this.m_imageMap = value; }
        }
    }
}