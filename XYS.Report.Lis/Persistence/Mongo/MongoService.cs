using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;

using MongoDB.Bson;
using MongoDB.Driver;
namespace XYS.Report.Lis.Persistence.Mongo
{
    class MongoService
    {
        private static readonly string MongoConnectionStr = "mongodb://10.1.10.246:27017";
        static MongoClient MClient = new MongoClient(MongoConnectionStr);
        public static void Insert(ReportReportElement report)
        {
            IMongoDatabase db = MClient.GetDatabase("lis");
            IMongoCollection<ReportReportElement> ReportCollection = db.GetCollection<ReportReportElement>("report");
            ReportCollection.InsertOne(report);
        }
        public static void Insert(IEnumerable<ReportReportElement> reportList)
        {
            IMongoDatabase db = MClient.GetDatabase("lis");
            IMongoCollection<ReportReportElement> ReportCollection = db.GetCollection<ReportReportElement>("report");
            ReportCollection.InsertMany(reportList);
        }
    }
}