using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property, Inherited = true)]
    public class FRExportAttribute : Attribute
    {
        private bool flag;
        public FRExportAttribute()
        {
            this.flag = true;
        }
        public FRExportAttribute(bool f)
        {
            this.flag = f;
        }
        public bool IsExport
        {
            get { return this.flag; }
        }
    }
}
