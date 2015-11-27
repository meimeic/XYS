using System;
using System.Collections;

using Newtonsoft.Json;

using XYS.Model;
using XYS.Lis.Core;

namespace XYS.Lis.Model.Export
{
    public class ReporterKV : ILisExportElement
    {
        private static readonly ReportElementTag Default_Tag = ReportElementTag.CustomElement;

        private string m_name;
        private readonly Hashtable m_kvTable;

        public ReporterKV()
        {
            this.m_kvTable = new Hashtable(4);
        }

        [JsonIgnore]
        public ReportElementTag ElementTag
        {
            get { return Default_Tag; }
        }

        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Hashtable KVTable
        {
            get { return this.m_kvTable; }
        }
    }
}
