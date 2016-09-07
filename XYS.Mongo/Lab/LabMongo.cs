using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Configuration;

using log4net;

using XYS.Util;
using XYS.Mongo.Model;
using XYS.Mongo.Util;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
namespace XYS.Mongo.Lab
{
    delegate void InsertErrorHandler(MReport report);
    delegate void InsertSuccessHandler(MReport report);
    delegate void UpdateErrorHandler(MReport report);
    delegate void UpdateSuccessHandler(MReport report);
    public class LabMongo : AbstractMongo
    {
        #region 常量字段
        protected static readonly ILog LOG;
        private static readonly LabMongo MService;
        private static readonly string MongoDBName;

        private readonly IMongoDatabase LisMDB;
        private readonly FilterDefinitionBuilder<MReport> FilterBuiler;
        private readonly UpdateDefinitionBuilder<MReport> UpdateBuiler;
        private readonly ProjectionDefinitionBuilder<MReport> ProjectionBuiler;
        #endregion

        #region 事件字段
        private event InsertErrorHandler m_insertErrorEvent;
        private event InsertSuccessHandler m_insertSuccessEvent;
        private event UpdateErrorHandler m_updateErrorEvent;
        private event UpdateSuccessHandler m_updateSuccessEvent;
        #endregion

        #region 构造函数
        static LabMongo()
        {
            LOG = LogManager.GetLogger("LabMongo");
            MongoDBName = Config.GetDBName();

            MService = new LabMongo();
        }
        private LabMongo()
            : base()
        {
            this.FilterBuiler = Builders<MReport>.Filter;
            this.UpdateBuiler = Builders<MReport>.Update;
            this.ProjectionBuiler = Builders<MReport>.Projection;
            this.LisMDB = MClient.GetDatabase(MongoDBName);
        }
        #endregion

        #region 静态属性
        public static LabMongo MongoService
        {
            get { return MService; }
        }
        #endregion

        #region 事件属性
        internal event InsertErrorHandler InsertErrorEvent
        {
            add { this.m_insertErrorEvent += value; }
            remove { this.m_insertErrorEvent -= value; }
        }
        internal event InsertSuccessHandler InsertSuccessEvent
        {
            add { this.m_insertSuccessEvent += value; }
            remove { this.m_insertSuccessEvent -= value; }
        }
        internal event UpdateErrorHandler UpdateErrorEvent
        {
            add { this.m_updateErrorEvent += value; }
            remove { this.m_updateErrorEvent -= value; }
        }
        internal event UpdateSuccessHandler UpdateSuccessEvent
        {
            add { this.m_updateSuccessEvent += value; }
            remove { this.m_updateSuccessEvent -= value; }
        }
        #endregion

        #region 同步方法
        public void InsertReport(MReport report)
        {
            try
            {
                IMongoCollection<MReport> ReportCollection = LisMDB.GetCollection<MReport>("labs");
                ReportCollection.InsertOne(report);
                this.OnInsertSuccess(report);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateAndInsertReport(MReport report)
        {
            FilterDefinition<MReport> filter = this.FilterBuiler.Eq(r => r.ReportID, report.ReportID)
                                                                                     & FilterBuiler.Eq(r => r.ActiveFlag, 1);
            UpdateDefinition<MReport> updater = this.UpdateBuiler.Set(r => r.ActiveFlag, 0);
            try
            {
                IMongoCollection<MReport> ReportCollection = LisMDB.GetCollection<MReport>("labs");
                UpdateResult res = ReportCollection.UpdateOne(filter, updater);
                if (res.IsAcknowledged)
                {
                    if (res.MatchedCount == 0 && res.ModifiedCount == 0)
                    {
                        this.InsertReport(report);
                    }
                    else if (res.MatchedCount == res.ModifiedCount)
                    {
                        this.OnUpdateSuccess(report);
                        this.InsertReport(report);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReportActive(Guid guid)
        {
        }
        #endregion

        #region 触发事件
        protected void OnInsertError(MReport report)
        {
            InsertErrorHandler handler = this.m_insertErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnInsertSuccess(MReport report)
        {
            InsertSuccessHandler handler = this.m_insertSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnUpdateError(MReport report)
        {
            UpdateErrorHandler handler = this.m_updateErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnUpdateSuccess(MReport report)
        {
            UpdateSuccessHandler handler = this.m_updateSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion
    }
}
