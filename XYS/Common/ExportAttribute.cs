using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
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
        public bool IsExport
        {
            get { return this.flag; }
        }
    }
}
