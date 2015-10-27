using System;
using System.Collections.Generic;

namespace XYS.Lis
{
    public class LisSearchRequire
    {
        private readonly static int TOP = 1000;
        private int m_max;
        private Dictionary<string, object> m_equalDictionary;
        private Dictionary<string, object> m_notEqualDictionary;
        private Dictionary<string, object> m_likeDictionary;

        public LisSearchRequire()
            : this(TOP)
        { }
        public LisSearchRequire(int max)
        {
            this.m_max = max;
            this.m_equalDictionary = new Dictionary<string, object>(10);
            this.m_notEqualDictionary = new Dictionary<string, object>(10);
            this.m_likeDictionary = new Dictionary<string, object>(10);
        }
        public Dictionary<string, object> EqualFields
        {
            get { return this.m_equalDictionary; }
        }
        public Dictionary<string, object> NoEqualFields
        {
            get { return this.m_notEqualDictionary; }
        }
        public Dictionary<string, object> LikeFields
        {
            get { return this.m_likeDictionary; }
        }
        public int MaxRecord
        {
            get { return this.m_max; }
            set 
            {
                if (value > TOP)
                {
                    this.m_max = TOP;
                }
                else
                {
                    this.m_max = value;
                }
            }
        }
    }
}
