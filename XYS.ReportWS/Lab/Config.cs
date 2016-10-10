using System;
using System.Configuration;

namespace XYS.ReportWS.Lab
{
    class Config
    {
        public static int GetWorkerCount()
        {
            string str = ConfigurationManager.AppSettings["LabWorkerCount"];
            if (!string.IsNullOrEmpty(str))
            {
                int count;
                if (int.TryParse(str, out count))
                {
                    return count;
                }
            }
            return 2;
        }
    }
}