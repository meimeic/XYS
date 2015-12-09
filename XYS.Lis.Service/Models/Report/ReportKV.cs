using System;
using System.Collections;

using Newtonsoft.Json;
using XYS.Model;
namespace XYS.Lis.Service.Models.Report
{
    public class ReportKV : IReportModel
    {
        private static readonly ReportElementTag Default_Element = ReportElementTag.KVElement;

        private string m_name;
        private readonly Hashtable m_kvTable;

        public ReportKV()
        {
            this.m_kvTable = new Hashtable(4);
        }

        #region 实现ireportmodel接口
        [JsonIgnore]
        public ReportElementTag ElementTag
        {
            get { return Default_Element; }
        }
        #endregion

        #region 属性
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Hashtable KVTable
        {
            get { return this.m_kvTable; }
        }
        #endregion
    }
}
