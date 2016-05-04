using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Report;
using XYS.Common;

using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportReportElement : IReportElement
    {
        #region 私有实例字段
        private Guid m_id;

        private int m_activeFlag;
        private string m_reportID;
     
        private ReportPK m_reportPK;
        private HandleResult m_handleResult;
        
        //报告名
        private string m_reportTitle;
        private DateTime m_createDateTime;
        //附加备注
        private int m_remarkFlag;
        private string m_remark;

        private List<int> m_superItemList;
        private ReportInfoElement m_reportInfo;
        private Dictionary<string, string> m_reportImageMap;
        private List<ReportItemElement> m_reportItemCollection;
        private List<ReportCustomElement> m_reportCustomCollection;

        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
        {
            this.m_remarkFlag = 0;
            this.m_superItemList = new List<int>(3);
            this.m_handleResult = new HandleResult();
            this.m_reportInfo = new ReportInfoElement();
            this.m_reportImageMap = new Dictionary<string, string>(4);
            this.m_reportItemCollection = new List<ReportItemElement>(20);
            this.m_reportCustomCollection = new List<ReportCustomElement>(2);
            this.m_reportItemTable = SystemInfo.CreateCaseInsensitiveHashtable(3);
        }
        #endregion

        #region 实现IReportElement接口
        [BsonIgnore]
        public IReportKey PK
        {
            get { return this.m_reportPK; }
        }
        [BsonIgnore]
        public HandleResult HandleResult
        {
            get { return this.m_handleResult; }
        }
        #endregion

        #region 实例属性
        [BsonId]
        public Guid ID
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        [BsonElement(Order = 1)]
        public string ReportID
        {
            get
            {
                if (this.m_reportID == null)
                {
                    if (this.ReportPK != null)
                    {
                        this.m_reportID = this.ReportPK.ID;
                    }
                    else
                    {
                        this.m_reportID = "Unkown";
                    }
                }
                return this.m_reportID;
            }
            set { this.m_reportID = value; }
        }
        [BsonElement(Order = 2)]
        public int ActiveFlag
        {
            get { return m_activeFlag; }
            set { m_activeFlag = value; }
        }

        [BsonIgnore]
        public ReportPK ReportPK
        {
            get { return this.m_reportPK; }
            set { this.m_reportPK = value; }
        }

        [BsonElement(Order = 3)]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        [BsonElement(Order = 4)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime
        {
            get
            {
                if (this.m_createDateTime == default(DateTime))
                {
                    this.m_createDateTime = DateTime.Now;
                }
                return this.m_createDateTime;
            }
            set { this.m_createDateTime = value; }
        }

        //备注标识 0表示没有备注 1 表示备注已设置  2 表示备注未设置
        [BsonIgnore]
        public int RemarkFlag
        {
            get { return this.m_remarkFlag; }
            set { this.m_remarkFlag = value; }
        }
        [BsonElement(Order = 5)]
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }

        [BsonElement(Order = 6)]
        public List<int> SuperItemList
        {
            get { return this.m_superItemList; }
            set { this.m_superItemList = value; }
        }
         
        [BsonElement(Order = 7)]
        public ReportInfoElement ReportInfo
        {
            get { return this.m_reportInfo; }
            set { this.m_reportInfo = value; }
        }

        [BsonElement(Order = 8)]
        public List<ReportItemElement> ReportItemCollection
        {
            get { return this.m_reportItemCollection; }
            set { this.m_reportItemCollection = value; }
        }
        [BsonElement(Order = 9)]
        public List<ReportCustomElement> ReportCustomCollection
        {
            get { return this.m_reportCustomCollection; }
            set { this.m_reportCustomCollection = value; }
        }
        [BsonElement(Order = 10)]
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, string> ReportImageMap
        {
            get { return this.m_reportImageMap; }
            set { this.m_reportImageMap = value; }
        }
        #endregion

        #region 公共方法
        public void Init()
        {
        }
        public void Clear()
        {
            this.HandleResult.Clear();
            this.ReportImageMap.Clear();
            this.m_reportItemTable.Clear();
            this.ReportItemCollection.Clear();
            this.ReportCustomCollection.Clear();
            //非填充项清空

            this.ID = default(Guid);
            //
            this.ReportPK = null;
            this.ReportTitle = "";
            this.RemarkFlag = 0;
            this.ActiveFlag = 0;
            this.Remark = "";
        }
        public List<IFillElement> GetReportItem(Type type)
        {
            if (type != null)
            {
                List<IFillElement> result = this.m_reportItemTable[type] as List<IFillElement>;
                if (result == null)
                {
                    result = new List<IFillElement>(10);
                    lock (this.m_reportItemTable)
                    {
                        this.m_reportItemTable[type] = result;
                    }
                }
                return result;
            }
            return null;
        }
        #endregion
    }
}