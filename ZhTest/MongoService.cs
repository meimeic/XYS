using System;

using MongoDB.Bson;
using MongoDB.Driver;
namespace ZhTest
{
    class MongoService
    {
        private static readonly string MongoConnectionStr = "10.1.10.246:27017";
        private static MongoClient MClient = new MongoClient(MongoConnectionStr);
        private static IMongoDatabase MDB = MClient.GetDatabase("digitlab");
    }
}
