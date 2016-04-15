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
        static MongoClient MClient;
        static IMongoDatabase LisMDB;
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
        static ReportMongoService()
        {
            MClient = new MongoClient(MongoConnectionStr);
            LisMDB = MClient.GetDatabase("lis");
        }

        public void Insert(ReportReportElement report)
        {
            report.Final = 1;
            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
            ReportCollection.InsertOne(report);
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

        #region
        public async Task<HandlerResult> InsertAsync(ReportReportElement report, Func<ReportReportElement, HandlerResult> callback)
        {
            IMongoCollection<ReportReportElement> ReportCollection = LisMDB.GetCollection<ReportReportElement>("report");
            try
            {
                await ReportCollection.InsertOneAsync(report);
                if (callback != null)
                {
                    return callback(report);
                }
                return new HandlerResult(1, this.GetType(), "save to mongo sucessfully", report.PK);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("fialed when save report to mongo,error message:");
                sb.Append(ex.Message);
                sb.Append(SystemInfo.NewLine);
                sb.Append(ex.ToString());
                return new HandlerResult(-1, this.GetType(), sb.ToString(), report.PK);
            }
        }
        #endregion
    }
}
