using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited=true)]
    public sealed class ColumnAttribute:Attribute
    {
        private bool flag;
        public ColumnAttribute()
        {
            this.flag = true;
        }
        public ColumnAttribute(bool flag)
        {
            this.flag = flag;
        }
        public bool IsColumn
        {
            get { return this.flag; }
        }
    }
}
