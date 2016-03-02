using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class FRConvertAttribute : Attribute
    {
        private bool flag;
        private string name;
        public FRConvertAttribute()
            : this(true, null)
        {
        }
        public FRConvertAttribute(bool f)
            : this(f, null)
        {
        }
        public FRConvertAttribute(bool flag, string name)
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
