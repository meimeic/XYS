using System;
using System.Configuration;

namespace XYS.Mongo.Util
{
    public class Config
    {
        public static string GetMongoServer()
        {
            string result = "mongodb://127.0.0.1:27017";
            string server = ConfigurationManager.AppSettings["DBServer"];
            if (!string.IsNullOrEmpty(server))
            {
                result = server;
            }
            return result;
        }
        public static string GetDBName()
        {
            string result = "test";
            string str = ConfigurationManager.AppSettings["DBName"];
            if (!string.IsNullOrEmpty(str))
            {
                result = str;
            }
            return result;
        }
    }
}