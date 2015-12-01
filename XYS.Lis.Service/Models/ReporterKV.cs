using System;
using System.Collections;

using Newtonsoft.Json;
namespace XYS.Lis.Model.Export
{
    public class ReporterKV
    {
        private string m_name;
        private readonly Hashtable m_kvTable;

        public ReporterKV()
        {
            this.m_kvTable = new Hashtable(4);
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
