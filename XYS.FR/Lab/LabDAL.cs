using System;
using System.Configuration;
using System.Data;

using XYS.Model.Lab;
namespace XYS.FR.Lab
{
    public class LabDAL
    {
        private readonly string m_connectionString;

        public LabDAL()
        {
            this.m_connectionString = ConfigurationManager.ConnectionStrings["ReportMSSQL"].ConnectionString;
        }

        public void SaveRecord(InfoElement info, int order, string filePath)
        {
        }
    }
}