using System;

namespace XYS.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class Convert2XmlAttribute : Attribute
    {
        private bool flag;
        public Convert2XmlAttribute()
        {
            this.flag = true;
        }
        public Convert2XmlAttribute(bool f)
        {
            this.flag = f;
        }
        public bool IsConvert
        {
            get { return this.flag; }
        }
    }
}
