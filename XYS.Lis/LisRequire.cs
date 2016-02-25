using System;
using System.Collections.Generic;

namespace XYS.Lis
{
    public class LisRequire
    {
        private readonly static int TOP = 100;
        private readonly static int m_defaultInterval = 30;

        private int m_max;// 最大记录数
        private int m_interval;  //时间间隔
        private bool m_dateLimit; //是否启用时间限制
        private DateTime m_startDateTime; //起始日期
        private DateTime m_endDateTime; // 终止日期

        private Dictionary<string, object> m_equalDictionary;
        private Dictionary<string, object> m_notEqualDictionary;
        private Dictionary<string, object> m_likeDictionary;
        private string m_orderByField;
        public LisRequire()
            : this(TOP)
        { }
        public LisRequire(int max)
            : this(max, m_defaultInterval)
        {
        }
        public LisRequire(int max, int interval)
        {
            this.m_max = max;
            this.m_dateLimit = true;
            this.m_interval = interval;
            this.m_startDateTime = System.DateTime.Now.AddDays(0 - this.m_interval);
            this.m_endDateTime = System.DateTime.Now;
            this.m_equalDictionary = new Dictionary<string, object>(4);
            this.m_notEqualDictionary = new Dictionary<string, object>(2);
            this.m_likeDictionary = new Dictionary<string, object>(1);
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
        public int Interval
        {
            get { return this.m_interval; }
            set { this.m_interval = value; }
        }
        public bool DateLimit
        {
            get { return this.m_dateLimit; }
            set { this.m_dateLimit = value; }
        }
        public DateTime StartDateTime
        {
            get { return this.m_startDateTime; }
            set { this.m_startDateTime = value; }
        }
        public DateTime EndDateTime
        {
            get { return this.m_endDateTime; }
            set { this.m_endDateTime = value; }
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
    }
}
