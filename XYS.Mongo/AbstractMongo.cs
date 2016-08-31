using System;
using System.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace XYS.Mongo
{
    public abstract class AbstractMongo
    {
        private static readonly string MongoConnectionStr;
        protected static readonly MongoClient MClient;
        static AbstractMongo()
        {
            MongoConnectionStr = ConfigurationManager.AppSettings["MongoConnStr"].ToString();
            MClient = new MongoClient(MongoConnectionStr);
        }
        protected AbstractMongo()
        {
        }
    }
}