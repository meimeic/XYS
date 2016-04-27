using System;
using System.IO;
using System.Collections.Concurrent;
using System.Xml.Serialization;
namespace XYS.Report.WS
{
    public class ApplyInfoEventArgs : EventArgs
    {
        private LabApplyInfo m_applyInfo;
        public ApplyInfoEventArgs(LabApplyInfo applyInfo)
        {
            this.m_applyInfo = applyInfo;
        }
        public LabApplyInfo ApplyInfo
        {
            get { return this.m_applyInfo; }
            set { this.m_applyInfo = value; }
        }
    }
    public delegate void ApplyReceivedHandler(LabApplyInfo applyInfo);
    public class XmlService
    {
        #region 只读字段
        private readonly XmlSerializer m_serializer;
        private readonly ConcurrentBag<LabApplyInfo> m_applyInfoBag;
        #endregion

        #region 私有事件
        private event ApplyReceivedHandler m_applyReceivedEvent;
        #endregion

        #region
        public XmlService()
        {
            this.m_serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.m_applyInfoBag = new ConcurrentBag<LabApplyInfo>();
        }
        #endregion

        #region 事件属性
        public event ApplyReceivedHandler ApplyReceivedEvent
        {
            add { this.m_applyReceivedEvent += value; }
            remove { this.m_applyReceivedEvent -= value; }
        }
        #endregion

        #region 实例属性
        public XmlSerializer Serializer
        {
            get { return this.m_serializer; }
        }
        public ConcurrentBag<LabApplyInfo> ApplyInfoBag
        {
            get { return this.m_applyInfoBag; }
        }
        #endregion

        #region 方法
        public void Deserialize(string applyInfo)
        {
            if (applyInfo != null)
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    this.ApplyInfoBag.Add(info);
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            else
            {
            }
        }
        #endregion

        #region
        public void Start()
        {
            LabApplyInfo info = null;
            while (!this.ApplyInfoBag.IsEmpty)
            {
                if (this.ApplyInfoBag.TryTake(out info))
                {
                    this.OnApplyReceived(info);
                }
                else
                {
                    //
                }
            }
        }
        #endregion

        #region 触发事件
        protected void OnApplyReceived(LabApplyInfo applyInfo)
        {
            ApplyReceivedHandler handler = this.m_applyReceivedEvent;
            if (handler != null)
            {
                handler(applyInfo);
            }
        }
        #endregion

    }
}