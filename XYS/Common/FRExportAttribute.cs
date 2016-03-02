using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true)]
    public class FRExportAttribute : Attribute
    {
        private bool flag;
        private string name;
        public FRExportAttribute()
            : this(true, null)
        {
        }
        public FRExportAttribute(bool f)
            : this(f, null)
        {
        }
        public FRExportAttribute(bool flag, string name)
        {
            this.flag = flag;
            this.name = name;
        }
        public bool IsExport
        {
            get { return this.flag; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}
