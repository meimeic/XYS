using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using XYS.Util;
using XYS.Report.Lis.Model;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
namespace XYS.Report.Lis.IO
{
    public class ReportMongoService
    {
        #region
        static MongoClient MClient;
        static IMongoDatabase LisMDB;
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
        #endregion
        
        #region
        static ReportMongoService()
        {
            MClient = new MongoClient(MongoConnectionStr);
            LisMDB = MClient.GetDatabase("lis");
        }
        public ReportMongoService()
        { 
        }
        #endregion

        #region 同步方法
        public void Insert(ReportReportElement report, HandlerResult result)
        {
            report.Final = 1;
            try
            {
                IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                ReportCollection.InsertOne(report);
                this.SetHandlerResult(result, 1, "save to mongo sucessfully");
                return;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("fialed when save report to mongo,error message:");
                sb.Append(ex.Message);
                sb.Append(SystemInfo.NewLine);
                sb.Append(ex.ToString());
                this.SetHandlerResult(result, -1, this.GetType(), sb.ToString(), report.PK);
                return;
            }
        }
        public void Insert(IEnumerable<ReportReportElement> reportList)
        {
            IMongoDatabase db = MClient.GetDatabase("lis");
            IMongoCollection<ReportReportElement> ReportCollection = db.GetCollection<ReportReportElement>("report");
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
        #endregion

        #region 异步方法
        public async Task InsertAsync(ReportReportElement report, HandlerResult result, Action<ReportReportElement, HandlerResult> callback = null)
        {
            if (result.Code != -1)
            {
                try
                {
                    IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
                    await ReportCollection.InsertOneAsync(report);
                    this.SetHandlerResult(result, 1, this.GetType(), "save to mongo sucessfully", report.PK);
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("fialed when save report to mongo,error message:");
                    sb.Append(ex.Message);
                    sb.Append(SystemInfo.NewLine);
                    sb.Append(ex.ToString());
                    this.SetHandlerResult(result, -1, this.GetType(), sb.ToString(), report.PK);
                }
            }
            if (callback != null)
            {
                callback(report, result);
            }
        }
        #endregion

        #region
        protected void SetHandlerResult(HandlerResult result, int code, string message)
        {
            result.Code = code;
            result.Message = message;
        }
        protected void SetHandlerResult(HandlerResult result, int code, Type type, string message)
        {
            SetHandlerResult(result, code, message);
            result.FinalType = type;
        }
        protected void SetHandlerResult(HandlerResult result, int code, Type type, string message, IReportKey key)
        {
            SetHandlerResult(result, code, type, message);
            result.ReportKey = key;
        }
        #endregion
    }
}
