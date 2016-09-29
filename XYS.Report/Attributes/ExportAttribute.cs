using System;
namespace XYS.Report.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ExportAttribute : Attribute
    {
        private bool flag;
        private string name;
        public ExportAttribute()
            : this(true, null)
        {
        }
        public ExportAttribute(bool f)
            : this(f, null)
        {
        }
        public ExportAttribute(bool flag, string name)
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
