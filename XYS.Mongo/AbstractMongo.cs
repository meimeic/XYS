using System;
using System.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

using XYS.Mongo.Util;
namespace XYS.Mongo
{
    public abstract class AbstractMongo
    {
        private static readonly string MongoServer;
        protected static readonly MongoClient MClient;
        static AbstractMongo()
        {
            MongoServer = Config.GetMongoServer();
            MClient = new MongoClient(MongoServer);
        }
        protected AbstractMongo()
        {
        }
    }
}