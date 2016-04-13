using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace XYS.Report.Lis.Persistence.Mongo
{
    class MongoService
    {
        private static readonly string MongoConnectionStr = "mongodb://10.1.11.10:27017";
        static MongoClient MClient;
        static MongoService()
        {
            MClient = new MongoClient(MongoConnectionStr);
        }
        public static void Insert(ReportReportElement report)
        {
            report.Final = 1;
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