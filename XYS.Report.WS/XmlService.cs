using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Xml.Serialization;

using XYS.Report.Lis;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO.SQLServer;
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
    /// <summary>
    /// 委托定义
    /// </summary>
    /// <param name="applyInfo"></param>
    public delegate void ApplyReceivedHandler(LabApplyInfo applyInfo);
    
    /// <summary>
    /// 单例模式
    /// </summary>
    public class XmlService
    {
        #region 静态字段
        private static readonly XmlService ServiceInstance; 
        #endregion

        #region 只读字段
        private readonly XmlSerializer m_serializer;
        private readonly LisReportPKDAL m_pkDAL;
        private readonly ConcurrentBag<LabApplyInfo> m_applyInfoBag;
        private readonly ConcurrentBag<ReportReportElement> m_reportBag;
        #endregion

        #region 事件
        private event ApplyReceivedHandler m_applyReceivedEvent;
        #endregion

        #region 构造函数
        static XmlService()
        {
            ServiceInstance = new XmlService();
        }
        private XmlService()
        {
            this.m_pkDAL = new LisReportPKDAL();
            this.m_serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.m_applyInfoBag = new ConcurrentBag<LabApplyInfo>();
            this.m_reportBag = new ConcurrentBag<ReportReportElement>();
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
        public LisReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        #endregion

        #region 静态方法
        public static XmlService GetService()
        {
            return ServiceInstance;
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

        #region
        #region
        protected void InitReportPK(Require req, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        protected void InitReportPK(string where, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion
        #endregion

    }
}