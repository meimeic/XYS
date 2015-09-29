using System;
namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited=true)]
    public sealed class TableColumnAttribute:Attribute
    {
        private bool flag;
        public TableColumnAttribute()
        {
            this.flag = false;
        }
        public TableColumnAttribute(bool flag)
        {
            this.flag = flag;
        }
        public bool IsColumn
        {
            get { return this.flag; }
        }
    }
}
