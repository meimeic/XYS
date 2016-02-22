using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class, Inherited = true)]
    public class ConvertAttribute : Attribute
    {
        private bool flag;
        public ConvertAttribute()
        {
            this.flag = true;
        }
        public ConvertAttribute(bool f)
        {
            this.flag = f;
        }
        public bool IsExport
        {
            get { return this.flag; }
        }
    }
}
