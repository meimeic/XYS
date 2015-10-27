using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Util
{
    public class PropertyEntry
    {
        private string m_key = null;
        private object m_value = null;
        public string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }
        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
        public override string ToString()
        {
            return "PropertyEntry(Key=" + m_key + ", Value=" + m_value + ")";
        }
    }
}
