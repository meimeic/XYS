using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Configuration;

using log4net;

using XYS.Util;
using XYS.Mongo.Lab.Model;
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
        private readonly IMongoCollection<MReport> LabCollection;
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
            MongoDBName = Config.GetDBName();
            LOG = LogManager.GetLogger("LabMongo");

            MService = new LabMongo();
        }
        private LabMongo()
            : base()
        {
            this.FilterBuiler = Builders<MReport>.Filter;
            this.UpdateBuiler = Builders<MReport>.Update;
            this.ProjectionBuiler = Builders<MReport>.Projection;
            this.LisMDB = MClient.GetDatabase(MongoDBName);
            this.LabCollection = LisMDB.GetCollection<MReport>("labs");
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

        #region
        protected IMongoCollection<MReport> LabTable
        {
            get { return this.LabCollection; }
        }
        #endregion

        #region 同步方法
        public void InsertReport(MReport report)
        {
            try
            {
                LabTable.InsertOne(report);
                this.OnInsertSuccess(report);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertReport(List<MReport> reports)
        {
            try
            {
                LabTable.InsertMany(reports);
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message);
            }
        }
        public void UpdateAndInsertReport(MReport report)
        {
            FilterDefinition<MReport> filter = this.FilterBuiler.Eq(r => r.ReportID, report.ReportID)
                                                                                     & FilterBuiler.Eq(r => r.ActiveFlag, 1);
            try
            {
                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
                long count = LabTable.Count(filter);
                TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
                LOG.Info("查询报告时间为" + end.Subtract(start).TotalMilliseconds.ToString() + "ms");
                if (count > 0)
                {
                    UpdateDefinition<MReport> updater = this.UpdateBuiler.Set(r => r.ActiveFlag, 0);
                    //LabTable.FindOneAndUpdate(filter, updater);
                    start = new TimeSpan(DateTime.Now.Ticks);
                    UpdateResult res = LabTable.UpdateOne(filter, updater);
                    if (res.IsAcknowledged)
                    {
                        //if (res.MatchedCount == 0 && res.ModifiedCount == 0)
                        //{
                        //    this.InsertReport(report);
                        //}
                        //else if (res.MatchedCount == res.ModifiedCount)
                        //{
                        //    this.OnUpdateSuccess(report);
                        //    this.InsertReport(report);
                        //}
                        //else
                        //{
                        //    throw new Exception("未知错误");
                        //}
                    }
                    else
                    {
                        LOG.Info("修改未确认");
                    }
                    end = new TimeSpan(DateTime.Now.Ticks);
                    LOG.Info("修改报告时间为" + end.Subtract(start).TotalMilliseconds.ToString() + "ms");
                }
                start = new TimeSpan(DateTime.Now.Ticks);
                this.InsertReport(report);
                end = new TimeSpan(DateTime.Now.Ticks);
                LOG.Info("插入报告时间为" + end.Subtract(start).TotalMilliseconds.ToString() + "ms");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReportActive(Guid guid)
        {
        }

        public void MakeIndex()
        {
            IndexKeysDefinition<MReport> reportIDIndexDesc = Builders<MReport>.IndexKeys.Descending(r=>r.ReportID);
            IndexKeysDefinition<MReport> activeFlagIndexDesc = Builders<MReport>.IndexKeys.Descending(r => r.ActiveFlag);
            var indexBuiler = Builders<MReport>.IndexKeys.Combine(reportIDIndexDesc, activeFlagIndexDesc);
            LabCollection.Indexes.CreateOne(indexBuiler);
        }
        #endregion

        #region
        public void FindOneAndReplace(MReport newReport)
        {
            FilterDefinition<MReport> filter = this.FilterBuiler.Eq(r => r.ReportID, newReport.ReportID)
                                                                                    & FilterBuiler.Eq(r => r.ActiveFlag, 1);
            FindOneAndReplaceOptions<MReport> options = new FindOneAndReplaceOptions<MReport>()
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.Before
            };
            MReport oldReport = LabCollection.FindOneAndReplace(filter, newReport);
            if (oldReport == null)
            {
                LOG.Info("不存在报告");
            }
            else 
            {
                LOG.Info("存在报告ID为:" + oldReport.ReportID);
            }
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
