using System;
using System.IO;
using System.Collections.Generic;
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
        #region
        private readonly ReportService m_report;
        private readonly XmlSerializer m_serializer;
        #endregion

        #region
        private readonly List<LabApplyInfo> m_applyList;
        //private event EventHandler<ApplyInfoEventArgs> m_applyReceivedEvent;
        private event ApplyReceivedHandler m_applyReceivedEvent;
        #endregion

        #region
        public XmlService()
        {
            this.m_report = new ReportService();
            this.m_applyList = new List<LabApplyInfo>(1000);
            this.m_serializer = new XmlSerializer(typeof(LabApplyInfo));
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
        public ReportService ReportService
        {
            get { return this.m_report; }
        }
        public XmlSerializer Serializer
        {
            get { return this.m_serializer; }
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
                    this.AddApply(info);
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
        private void AddApply(LabApplyInfo applyInfo)
        {
            lock (this.m_applyList)
            {
                this.m_applyList.Add(applyInfo);
            }
            this.OnApplyReceived(applyInfo);
        }
        private void RemoveApply(LabApplyInfo applyInfo)
        {
            lock (this.m_applyList)
            {
                this.m_applyList.Remove(applyInfo);
            }
        }
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