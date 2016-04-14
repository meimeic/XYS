using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
namespace XYS.Report.Lis.Persistence.Mongo
{
    public class MongoService
    {
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
        static MongoClient MClient;
        static IMongoDatabase LisMDB;
        static MongoService()
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
    }
}