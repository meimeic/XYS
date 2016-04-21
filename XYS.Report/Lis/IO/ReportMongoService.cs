using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using XYS.Util;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO.Mongo;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
namespace XYS.Report.Lis.IO
{

    public class ReportMongoService
    {
        #region 常量字段
        static MongoClient MClient;
        static IMongoDatabase LisMDB;
        static FilterDefinitionBuilder<ReportReportElement> FilterBuiler;
        static UpdateDefinitionBuilder<ReportReportElement> UpdateBuiler;
        static ProjectionDefinitionBuilder<ReportReportElement> ProjectionBuiler;
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
        #endregion

        #region 实例字段

        #endregion

        #region 构造函数
        static ReportMongoService()
        {
            MClient = new MongoClient(MongoConnectionStr);
            LisMDB = MClient.GetDatabase("lis");
            FilterBuiler = Builders<ReportReportElement>.Filter;
            UpdateBuiler = Builders<ReportReportElement>.Update;
            ProjectionBuiler = Builders<ReportReportElement>.Projection;
        }
        public ReportMongoService()
        {
        }
        #endregion

        #region 同步方法
        public void InsertReport(ReportReportElement report)
        {
            try
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                ReportCollection.InsertOne(report);
                this.SetHandlerResult(report.HandleResult, 200, "insert into mongo sucessfully");
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("fialed when insert report into mongo,error message:");
                sb.Append(ex.Message);
                sb.Append(SystemInfo.NewLine);
                sb.Append(ex.ToString());
                this.SetHandlerResult(report.HandleResult, -201, this.GetType(), sb.ToString());
            }
        }
        public void InsertReportCurrently(ReportReportElement report)
        {
            if (report.HandleResult.ResultCode != -1)
            {
                FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, report.ReportID)
                                                                                      & FilterBuiler.Eq(r => r.Final, 1);
                UpdateDefinition<ReportReportElement> updater = UpdateBuiler.Set(r => r.Final, -1);

                try
                {
                    IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                    UpdateResult res = ReportCollection.UpdateMany(filter, updater);
                    if (res.IsAcknowledged)
                    {
                        //新插入
                        if (res.ModifiedCount == 0)
                        {
                            ReportCollection.InsertOne(report);
                            this.SetHandlerResult(report.HandleResult, 200, " insert into mongo sucessfully");
                        }
                        // 成功修改
                        else if (res.MatchedCount > 0 && res.MatchedCount == res.ModifiedCount)
                        {
                            //
                            ReportCollection.InsertOne(report);
                            this.SetHandlerResult(report.HandleResult, 201, " update old report(s) and insert new report into mongo sucessfully");
                        }
                        //失败修改
                        else
                        {
                            this.SetHandlerResult(report.HandleResult, 202, "not all the old report(s) has been updated,so can not insert the new report");
                        }
                    }
                    else
                    {
                        this.SetHandlerResult(report.HandleResult, 203, "when update old report(s) occure some unkown things,so can not insert the new report");
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("fialed when save report to mongo,error message:");
                    sb.Append(ex.Message);
                    sb.Append(SystemInfo.NewLine);
                    sb.Append(ex.ToString());
                    this.SetHandlerResult(report.HandleResult, -201, this.GetType(), sb.ToString());
                }
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
                    this.SetHandlerResult(report.HandleResult, 2, "save to mongo sucessfully");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void Query()
        {
            FilterDefinition<ReportReportElement> filter = "{}";
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
        public void Test()
        {
            try
            {
                ProjectionDefinitionBuilder<ReportReportElement> projectionBuiler = Builders<ReportReportElement>.Projection;
                ProjectionDefinition<ReportReportElement> projection = projectionBuiler.Include(r => r.SerialNo)
                                                                                                                                          .Include(r => r.SectionNo)
                                                                                                                                          .Include(r => r.SampleNo)
                                                                                                                                          .Include(r => r.ID);
                var rprojection = projectionBuiler.Expression(r => new { SerialNo = r.SerialNo, SectionNo = r.SectionNo, SampleNo = r.SampleNo, Final = r.Final });

                //var projectionBuiler = Builders<ReportReportElement>.Projection;
                //var projection = projectionBuiler.Include(r => r.SerialNo)
                //                                                                                                                          .Include(r => r.SectionNo)
                //                                                                                                                          .Include(r => r.SampleNo)
                //                                                                                                                          .Exclude(r => r.ID);

                Guid guid = new Guid("18623c51-083b-4fa9-909c-4c06a64c32df");

                FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ID, guid);
                //& FilterBuiler.Eq(r => r.Final, -1);

                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");

                long count = ReportCollection.Count(filter);

                Console.WriteLine(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 异步方法
        public async Task InsertReportAsync(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            if (report.HandleResult.ResultCode != -1)
            {
                try
                {
                    IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                    await ReportCollection.InsertOneAsync(report);
                    this.SetHandlerResult(report.HandleResult, 200, "insert into mongo sucessfully");
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("fialed when insert report into mongo,error message:");
                    sb.Append(ex.Message);
                    sb.Append(SystemInfo.NewLine);
                    sb.Append(ex.ToString());
                    this.SetHandlerResult(report.HandleResult, -201, this.GetType(), sb.ToString());
                }
            }
            if (callback != null)
            {
                callback(report);
            }
        }
        public async Task InsertReportCurrentlyAsync(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            if (report.HandleResult.ResultCode != -1)
            {
                FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, report.ReportID)
                                                                                         & FilterBuiler.Eq(r => r.Final, 1);

                UpdateDefinition<ReportReportElement> updater = UpdateBuiler.Set(r => r.Final, -1);
                try
                {
                    IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                    UpdateResult res = await ReportCollection.UpdateManyAsync(filter, updater);
                    if (res.IsAcknowledged)
                    {
                        //新插入
                        if (res.ModifiedCount == 0)
                        {
                            await ReportCollection.InsertOneAsync(report);
                            this.SetHandlerResult(report.HandleResult, 200, " insert into mongo sucessfully");
                        }
                        // 成功修改
                        else if (res.MatchedCount > 0 && res.MatchedCount == res.ModifiedCount)
                        {
                            //
                            await ReportCollection.InsertOneAsync(report);
                            this.SetHandlerResult(report.HandleResult, 201, " update old report(s) and insert new report into mongo sucessfully");
                        }
                        //失败修改
                        else
                        {
                            this.SetHandlerResult(report.HandleResult, 202, "not all the old report(s) has been updated,so can not insert the new report");
                        }
                    }
                    else
                    {
                        this.SetHandlerResult(report.HandleResult, 203, "when update old report(s) occure some unkown things,so can not insert the new report");
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("fialed when save report to mongo,error message:");
                    sb.Append(ex.Message);
                    sb.Append(SystemInfo.NewLine);
                    sb.Append(ex.ToString());
                    this.SetHandlerResult(report.HandleResult, -201, this.GetType(), sb.ToString());
                }
                if (callback != null)
                {
                    callback(report);
                }
            }
        }
        public async Task TestAsync()
        {
            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");

            FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, "20160104-11-1-1600024")
                                                                                      & FilterBuiler.Eq(r => r.Final, 1);

            UpdateDefinition<ReportReportElement> finalUpdate = UpdateBuiler.Set(r => r.Final, -1);

            //ProjectionDefinition<ReportReportElement> projection = ProjectionBuiler.Include(r => r.ReportID)
            //                                                                                                                          .Include(r => r.Final)
            //                                                                                                                          .Include(r => r.ID);

            //var projection = ProjectionBuiler.Expression(r => new { SerialNo = r.SerialNo, SectionNo = r.SectionNo, SampleNo = r.SampleNo, Final = r.Final });

            ProjectionDefinition<ReportReportElement, ReportStatusProjection> projection = ProjectionBuiler.Expression(r => new ReportStatusProjection() { ID = r.ID, ReportID = r.ReportID, Final = r.Final });

            FindOneAndUpdateOptions<ReportReportElement, ReportStatusProjection> options = new FindOneAndUpdateOptions<ReportReportElement, ReportStatusProjection>()
            {
                Projection = ProjectionBuiler.Expression(r => new ReportStatusProjection() { ID = r.ID, ReportID = r.ReportID, Final = r.Final })
            };

            Expression<Func<ReportReportElement, bool>> expFilter = rep => rep.ReportID.Equals("20160104-11-1-1600024");

            //var result =await ReportCollection.FindOneAndUpdateAsync(expFilter, finalUpdate, options);
            //if (result != null)
            //{
            //    Console.WriteLine(result.ID.ToString());
            //}
            //else
            //{
            //    Console.WriteLine("these is null");
            //}

            UpdateResult res = await ReportCollection.UpdateManyAsync(filter, finalUpdate);

            if (res != null)
            {
                Console.WriteLine(res.IsAcknowledged);
                Console.WriteLine(res.MatchedCount);
                Console.WriteLine(res.ModifiedCount);
            }
            else
            {
                Console.WriteLine("these is null");
            }
        }
        #endregion

        #region 辅助方法

        protected void SetHandlerResult(HandleResult result, int code, string message)
        {
            result.ResultCode = code;
            result.Message = message;
        }
        protected void SetHandlerResult(HandleResult result, int code, Type type, string message)
        {
            SetHandlerResult(result, code, message);
            result.HandleType = type;
        }
        #endregion
    }
}