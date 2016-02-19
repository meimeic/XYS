using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class TableAttribute : Attribute
    {
        private bool flag;
        public TableAttribute()
        {
            this.flag = true;
        }
        public TableAttribute(bool flag)
        {
            this.flag = flag;
        }
        public bool IsTable
        {
            get { return this.flag; }
        }
    }
}
