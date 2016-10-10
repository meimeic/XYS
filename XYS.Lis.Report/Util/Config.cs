using System;
using System.Configuration;
using System.IO;

namespace XYS.Lis.Report.Util
{
    class Config
    {
        public static string GetImageServer()
        {
            string result = "http://img.xys.com:8080/lab";
            string server = ConfigurationManager.AppSettings["LabImageServer"];
            if (!string.IsNullOrEmpty(server))
            {
                result = server;
            }
            return result;
        }
        public static string GetLabImageLocalRoot()
        {
            string result = "E:\\image\\report\\lab";
            string rootPath = ConfigurationManager.AppSettings["LabImageLocalDir"];
            if (!string.IsNullOrEmpty(rootPath))
            {
                if (Directory.Exists(rootPath))
                {
                    result = rootPath;
                }
            }
            return result;
        }
    }
}
