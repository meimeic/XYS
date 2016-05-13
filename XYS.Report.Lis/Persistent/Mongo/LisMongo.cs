using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using log4net;
using XYS.Util;
using XYS.Report.Lis.Model;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
namespace XYS.Report.Lis.Persistent.Mongo
{
    public class LisMongo
    {
        #region 常量字段
        static ILog LOG;
        static MongoClient MClient;
        static IMongoDatabase LisMDB;
        static FilterDefinitionBuilder<ReportReportElement> FilterBuiler;
        static UpdateDefinitionBuilder<ReportReportElement> UpdateBuiler;
        static ProjectionDefinitionBuilder<ReportReportElement> ProjectionBuiler;
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
        private static readonly string MongoDataBaseStr = "lis";
        #endregion

        #region 事件字段
        private event InsertErrorHandler m_insertErrorEvent;
        private event InsertSuccessHandler m_insertSuccessEvent;
        private event UpdateErrorHandler m_updateErrorEvent;
        private event UpdateSuccessHandler m_updateSuccessEvent;
        #endregion

        #region 构造函数
        static LisMongo()
        {
            LOG = LogManager.GetLogger("LisReportMongo");
            MClient = new MongoClient(MongoConnectionStr);
            LisMDB = MClient.GetDatabase("report-node-test");
            FilterBuiler = Builders<ReportReportElement>.Filter;
            UpdateBuiler = Builders<ReportReportElement>.Update;
            ProjectionBuiler = Builders<ReportReportElement>.Projection;
        }
        public LisMongo()
        {
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
        public void InsertReport(ReportReportElement report)
        {
            try
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("reports");
                ReportCollection.InsertOne(report);
                this.OnInsertSuccess(report);
            }
            catch (Exception ex)
            {
                this.SetHandlerResult(report.HandleResult, -110, "保存报告内容到Mongo异常", this.GetType(), ex);
                this.OnInsertError(report);
            }
        }
        public void UpdateAndInsertReport(ReportReportElement report)
        {
            FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, report.ReportID)
                                                                                     & FilterBuiler.Eq(r => r.ActiveFlag, report.ActiveFlag);
            UpdateDefinition<ReportReportElement> updater = UpdateBuiler.Set(r => r.ActiveFlag, -1);
            try
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
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
                        this.SetHandlerResult(report.HandleResult, -121, "并非所有匹配记录完成修改异常", this.GetType(), new Exception("并非所有匹配记录完成修改,无法进行插入操作"));
                        this.OnUpdateError(report);
                    }
                }
                else
                {
                    this.SetHandlerResult(report.HandleResult, -122, "修改结果无法确认异常", this.GetType(), new Exception("修改结果无法确认，无法进行插入操作"));
                    this.OnUpdateError(report);
                }
            }
            catch (Exception ex)
            {
                this.SetHandlerResult(report.HandleResult, -120, "修改Mongo记录异常", this.GetType(), ex);
                this.OnUpdateError(report);
            }
        }
        
        public void InsertReportMany(IEnumerable<ReportReportElement> reportList)
        {
            try
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                ReportCollection.InsertMany(reportList);
                foreach (ReportReportElement report in reportList)
                {
                    this.SetHandlerResult(report.HandleResult, 200);
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
        public void Query()
        {
            FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportInfo.PatientID, "")
                                                                                       & FilterBuiler.Eq(r => r.ActiveFlag, 1);
            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
            using (var cursor = ReportCollection.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (ReportReportElement rre in cursor.Current)
                    {
                        // do something with the documents
                        Console.WriteLine(rre.SuperItemList.Count);
                    }
                }
            }
        }
        //public void Test()
        //{
        //    try
        //    {
        //        ProjectionDefinitionBuilder<ReportReportElement> projectionBuiler = Builders<ReportReportElement>.Projection;
        //        ProjectionDefinition<ReportReportElement> projection = projectionBuiler.Include(r => r.SerialNo)
        //                                                                                                                                  .Include(r => r.SectionNo)
        //                                                                                                                                  .Include(r => r.SampleNo)
        //                                                                                                                                  .Include(r => r.ID);
        //        var rprojection = projectionBuiler.Expression(r => new { SerialNo = r.SerialNo, SectionNo = r.SectionNo, SampleNo = r.SampleNo, Final = r.ActiveFlag });

        //        //var projectionBuiler = Builders<ReportReportElement>.Projection;
        //        //var projection = projectionBuiler.Include(r => r.SerialNo)
        //        //                                                                                                                          .Include(r => r.SectionNo)
        //        //                                                                                                                          .Include(r => r.SampleNo)
        //        //                                                                                                                          .Exclude(r => r.ID);

        //        Guid guid = new Guid("18623c51-083b-4fa9-909c-4c06a64c32df");

        //        FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ID, guid);
        //        //& FilterBuiler.Eq(r => r.Final, -1);

        //        IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");

        //        long count = ReportCollection.Count(filter);

        //        Console.WriteLine(count);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        #endregion

        //#region 异步方法
        //public async Task InsertReportAsync(ReportReportElement report, Func<ReportReportElement, Task> callback = null)
        //{
        //    if (report.HandleResult.ResultCode != -1)
        //    {
        //        try
        //        {
        //            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
        //            await ReportCollection.InsertOneAsync(report);
        //            this.SetHandlerResult(report.HandleResult, 200, "insert into mongo sucessfully");
        //        }
        //        catch (Exception ex)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("fialed when insert report into mongo,error message:");
        //            sb.Append(ex.Message);
        //            sb.Append(SystemInfo.NewLine);
        //            sb.Append(ex.ToString());
        //            this.SetHandlerResult(report.HandleResult, -201, this.GetType(), sb.ToString());
        //        }
        //    }
        //    if (callback != null)
        //    {
        //        await callback(report);
        //    }
        //}
        //public async Task InsertReportCurrentlyAsync(ReportReportElement report, Func<ReportReportElement, Task> callback = null)
        //{
        //    if (report.HandleResult.ResultCode != -1)
        //    {
        //        FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, report.ReportID)
        //                                                                                 & FilterBuiler.Eq(r => r.ActiveFlag, 1);

        //        UpdateDefinition<ReportReportElement> updater = UpdateBuiler.Set(r => r.ActiveFlag, -1);
        //        try
        //        {
        //            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
        //            UpdateResult res = await ReportCollection.UpdateManyAsync(filter, updater);
        //            if (res.IsAcknowledged)
        //            {
        //                //新插入
        //                if (res.ModifiedCount == 0)
        //                {
        //                    await ReportCollection.InsertOneAsync(report);
        //                    this.SetHandlerResult(report.HandleResult, 200, " insert into mongo sucessfully");
        //                }
        //                // 修改后插入
        //                else if (res.MatchedCount > 0 && res.MatchedCount == res.ModifiedCount)
        //                {
        //                    //
        //                    await ReportCollection.InsertOneAsync(report);
        //                    this.SetHandlerResult(report.HandleResult, 201, " update old report(s) and insert new report into mongo sucessfully");
        //                }
        //                //修改未完全成功
        //                else
        //                {
        //                    this.SetHandlerResult(report.HandleResult, 202, "not all the old report(s) has been updated,so can not insert the new report");
        //                }
        //            }
        //            //修改失败
        //            else
        //            {
        //                this.SetHandlerResult(report.HandleResult, 203, "when update old report(s) occure some unkown things,so can not insert the new report");
        //            }
        //        }
        //        //异常
        //        catch (Exception ex)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("fialed when save report to mongo,error message:");
        //            sb.Append(ex.Message);
        //            sb.Append(SystemInfo.NewLine);
        //            sb.Append(ex.ToString());
        //            this.SetHandlerResult(report.HandleResult, -201, this.GetType(), sb.ToString());
        //        }
        //        if (callback != null)
        //        {
        //            await callback(report);
        //        }
        //    }
        //}
        //public async Task TestAsync()
        //{
        //    IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");

        //    FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, "20160104-11-1-1600024")
        //                                                                              & FilterBuiler.Eq(r => r.ActiveFlag, 1);

        //    UpdateDefinition<ReportReportElement> finalUpdate = UpdateBuiler.Set(r => r.ActiveFlag, -1);

        //    //ProjectionDefinition<ReportReportElement> projection = ProjectionBuiler.Include(r => r.ReportID)
        //    //                                                                                                                          .Include(r => r.Final)
        //    //                                                                                                                          .Include(r => r.ID);

        //    //var projection = ProjectionBuiler.Expression(r => new { SerialNo = r.SerialNo, SectionNo = r.SectionNo, SampleNo = r.SampleNo, Final = r.Final });

        //    ProjectionDefinition<ReportReportElement, ReportStatusProjection> projection = ProjectionBuiler.Expression(r => new ReportStatusProjection() { ID = r.ID, ReportID = r.ReportID, Final = r.ActiveFlag });

        //    FindOneAndUpdateOptions<ReportReportElement, ReportStatusProjection> options = new FindOneAndUpdateOptions<ReportReportElement, ReportStatusProjection>()
        //    {
        //        Projection = ProjectionBuiler.Expression(r => new ReportStatusProjection() { ID = r.ID, ReportID = r.ReportID, Final = r.ActiveFlag })
        //    };

        //    Expression<Func<ReportReportElement, bool>> expFilter = rep => rep.ReportID.Equals("20160104-11-1-1600024");

        //    //var result =await ReportCollection.FindOneAndUpdateAsync(expFilter, finalUpdate, options);
        //    //if (result != null)
        //    //{
        //    //    Console.WriteLine(result.ID.ToString());
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine("these is null");
        //    //}

        //    UpdateResult res = await ReportCollection.UpdateManyAsync(filter, finalUpdate);

        //    if (res != null)
        //    {
        //        Console.WriteLine(res.IsAcknowledged);
        //        Console.WriteLine(res.MatchedCount);
        //        Console.WriteLine(res.ModifiedCount);
        //    }
        //    else
        //    {
        //        Console.WriteLine("these is null");
        //    }
        //}
        //#endregion

        #region 辅助方法
        protected void SetHandlerResult(HandleResult result, int code, string message = null, Type type = null, Exception ex = null)
        {
            result.ResultCode = code;
            result.Message = message;
            result.HandleType = type;
            result.Exception = ex;
        }
        #endregion

        #region 触发事件
        protected void OnInsertError(ReportReportElement report)
        {
            InsertErrorHandler handler = this.m_insertErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnInsertSuccess(ReportReportElement report)
        {
            InsertSuccessHandler handler = this.m_insertSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnUpdateError(ReportReportElement report)
        {
            UpdateErrorHandler handler = this.m_updateErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnUpdateSuccess(ReportReportElement report)
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
