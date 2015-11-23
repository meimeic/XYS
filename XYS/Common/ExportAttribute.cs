using System;

namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ExportAttribute : Attribute
    {
        private bool flag;
        public ExportAttribute()
        {
            this.flag = true;
        }
        public ExportAttribute(bool f)
        {
            this.flag = f;
        }
        public bool IsConvert
        {
            get { return this.flag; }
        }
    }
}
