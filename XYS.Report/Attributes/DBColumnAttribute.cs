using System;
namespace XYS.Report.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited=true)]
    public sealed class DBColumnAttribute:Attribute
    {
        private bool flag;
        public DBColumnAttribute()
        {
            this.flag = true;
        }
        public DBColumnAttribute(bool flag)
        {
            this.flag = flag;
        }
        public bool IsColumn
        {
            get { return this.flag; }
        }
    }
}
