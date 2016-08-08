using System;
using System.Configuration;

using System.Data;

using XYS.Lis.Report.Model;
namespace XYS.FR.Service.Lab
{
    public class PDFDAL
    {
        private readonly string m_connectionString;

        public PDFDAL()
        {
            this.m_connectionString = ConfigurationManager.ConnectionStrings["ReportMSSQL"].ConnectionString;
        }

        public void SaveRecord(InfoElement info,int order,string filePath)
        {
        }
    }
}