using System;
using System.Collections.Generic;

using XYS.Report.Entities;
namespace XYS.Report.Lab.Entities
{
    public class DBReportLab : DBReportBase
    {
        private List<Type> m_fillTypes;
        public DBReportLab()
        {
            m_fillTypes = new List<Type>(3);
        }

        public override List<Type> FillTypes
        {
            get { return m_fillTypes; }
        }
        public override List<IDBReportItem> ItemCollection(Type type)
        {
            throw new NotImplementedException();
        }
    }
}