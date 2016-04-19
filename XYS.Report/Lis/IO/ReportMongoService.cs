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
        #region 字段
        static MongoClient MClient;
        static IMongoDatabase LisMDB;
        static FilterDefinitionBuilder<ReportReportElement> FilterBuiler;
        static UpdateDefinitionBuilder<ReportReportElement> UpdateBuiler;
        static ProjectionDefinitionBuilder<ReportReportElement> ProjectionBuiler;
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
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
        public void Insert(ReportReportElement report)
        {
            try
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                ReportCollection.InsertOne(report);
                this.SetHandlerResult(report.HandleResult, 2, "save to mongo sucessfully");
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("fialed when save report to mongo,error message:");
                sb.Append(ex.Message);
                sb.Append(SystemInfo.NewLine);
                sb.Append(ex.ToString());
                this.SetHandlerResult(report.HandleResult, -1, this.GetType(), sb.ToString());
            }
        }
        public void Insert(IEnumerable<ReportReportElement> reportList)
        {
            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
            ReportCollection.InsertMany(reportList);
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
                                                                                                                                          .Exclude(r => r.ID);
                var rprojection = projectionBuiler.Expression(r => new { SerialNo = r.SerialNo, SectionNo = r.SectionNo, SampleNo = r.SampleNo, Final = r.Final });

                //var projectionBuiler = Builders<ReportReportElement>.Projection;
                //var projection = projectionBuiler.Include(r => r.SerialNo)
                //                                                                                                                          .Include(r => r.SectionNo)
                //                                                                                                                          .Include(r => r.SampleNo)
                //                                                                                                                          .Exclude(r => r.ID);

                FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, "20160104-11-1-1600024")
                                                                                          & FilterBuiler.Eq(r => r.Final, -1);

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
        public async Task InsertAsync(ReportReportElement report, Func<ReportReportElement, Task> callback = null)
        {
            if (report.HandleResult.ResultCode != -1)
            {
                try
                {
                    IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                    await ReportCollection.InsertOneAsync(report);
                    this.SetHandlerResult(report.HandleResult, 2, this.GetType(), "save to mongo sucessfully");
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("fialed when save report to mongo,error message:");
                    sb.Append(ex.Message);
                    sb.Append(SystemInfo.NewLine);
                    sb.Append(ex.ToString());
                    this.SetHandlerResult(report.HandleResult, -1, this.GetType(), sb.ToString());
                }
            }
            if (callback != null)
            {
                await callback(report);
            }
        }
        public async Task InsertCurrentlyAsync(ReportReportElement report, Func<ReportReportElement, Task> callback = null)
        {
            if (report.HandleResult.ResultCode != -1)
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");

                FilterDefinition<ReportReportElement> filter = FilterBuiler.Eq(r => r.ReportID, report.ReportID)
                                                                                      & FilterBuiler.Eq(r => r.Final, 1);

                long count = await ReportCollection.CountAsync(filter);


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

            ProjectionDefinition<ReportReportElement, ReportStatusProjection> projection = ProjectionBuiler.Expression(r => new ReportStatusProjection() { ID=r.ID,ReportID=r.ReportID,Final=r.Final});

            FindOneAndUpdateOptions<ReportReportElement, ReportStatusProjection> options = new FindOneAndUpdateOptions<ReportReportElement, ReportStatusProjection>()
            {
                Projection = ProjectionBuiler.Expression(r => new ReportStatusProjection() { ID = r.ID, ReportID = r.ReportID, Final = r.Final })
            };

            Expression<Func<ReportReportElement, bool>> expFilter = rep => rep.ReportID.Equals("20160104-11-1-1600024") ;

            var result =await ReportCollection.FindOneAndUpdateAsync(expFilter, finalUpdate, options);
            if (result != null)
            {
                Console.WriteLine(result.Final);
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