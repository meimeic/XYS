using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

using XYS.Util;
using XYS.Report.Lis;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistent;
namespace XYS.Report.WS
{
    public class LisService
    {
        #region 静态只读字段
        private static readonly int WorkerCount;
        private static readonly int QueueCapacity;
        private static readonly LisService ServiceInstance;
        #endregion

        #region 实例只读字段
        private readonly Reporter m_reporter;
        private readonly XmlSerializer m_serializer;
        private readonly ReportPKDAL m_pkDAL;
        private readonly BlockingCollection<ReportReportElement> m_reportQueue;
        #endregion

        #region 实例字段
        private object logLock;
        private List<Thread> m_workerPool;
        #endregion

        #region 构造函数
        static LisService()
        {
            WorkerCount = 10;
            QueueCapacity = 1000;
            ServiceInstance = new LisService();
        }
        private LisService()
        {
            this.logLock = new object();
            this.m_pkDAL = new ReportPKDAL();
            this.m_workerPool = new List<Thread>(WorkerCount);
            this.m_reporter = new DefaultReporter();
            this.m_serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.m_reportQueue = new BlockingCollection<ReportReportElement>(QueueCapacity);

            //
            this.InitWorkPool();
        }
        #endregion

        #region 静态属性
        public static LisService ReportService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 实例属性
        protected Reporter Reporter
        {
            get { return this.m_reporter; }
        }
        protected XmlSerializer Serializer
        {
            get { return this.m_serializer; }
        }
        protected ReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        protected BlockingCollection<ReportReportElement> ReportQueue
        {
            get { return this.m_reportQueue; }
        }
        #endregion

        #region 生产者方法
        public void Deserialize(string applyInfo)
        {
            if (applyInfo != null)
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    WriteLog(string.Format("处理xml报文的线程ID:{0}", SystemInfo.CurrentThreadId));
                    this.HandleApplyInfo(info);
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            else
            {
            }
        }
        protected void HandleApplyInfo(LabApplyInfo applyInfo)
        {
            List<Apply> applyCollection = applyInfo.ApplyCollection;
            if (applyCollection != null && applyCollection.Count > 0)
            {
                List<ReportPK> PKList = new List<ReportPK>(applyCollection.Count);
                foreach (Apply app in applyCollection)
                {
                    //获取审核的报告单主键
                    if (app.ApplyStatus == 7)
                    {
                        this.InitReportPK(app.ApplyNo, PKList);
                    }
                }
                //加入队列
                if (PKList.Count > 0)
                {
                    //
                    ReportReportElement report = null;
                    foreach (ReportPK pk in PKList)
                    {
                        report = new ReportReportElement();
                        report.ReportPK = pk;
                        this.ReportQueue.Add(report);
                    }
                }
            }
        }
        private void InitReportPK(string appNo, List<ReportPK> PKList)
        {
            string where = "where serialno='" + appNo.Trim() + "'";
            this.D_InitReportPK(where, PKList);
        }
        #endregion

        #region 消费者方法
        public void HandleReport()
        {
            foreach (ReportReportElement report in this.ReportQueue.GetConsumingEnumerable())
            {
                this.Reporter.OperateReport(report);
                WriteLog(string.Format("处理report的线程ID:{0},处理结果:{1}", SystemInfo.CurrentThreadId, report.HandleResult.Message));
            }
        }
        #endregion

        #region 停止

        #endregion

        #region 初始化处理线程池
        private void InitWorkPool()
        {
            Thread th = null;
            for (int i = 0; i < WorkerCount; i++)
            {
                th = new Thread(HandleReport);
                this.m_workerPool.Add(th);
                th.Start();
            }
            WriteLog(string.Format("创建report处理线程的线程ID为:{0}", SystemInfo.CurrentThreadId));
        }
        #endregion

        #region 数据持久层处理
        private void D_InitReportPK(Require req, List<ReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        private void D_InitReportPK(string where, List<ReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion

        #region
        protected void WriteLog(string message)
        {
            lock (this.logLock)
            {
                var file = System.IO.File.AppendText("D:\\log.txt");
                file.WriteLine(message);
                file.Close();
            }
        }
        #endregion
    }
}