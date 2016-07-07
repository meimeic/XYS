using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

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
        private readonly XmlSerializer m_serializer;
        private readonly BlockingCollection<string> m_ApplyQueue;
        #endregion

        #region 实例字段
        private object logLock;
        private List<Thread> m_workerPool;
        #endregion

        #region 构造函数
        static LisService()
        {
            WorkerCount = 4;
            QueueCapacity = 1000;
            ServiceInstance = new LisService();
        }
        private LisService()
        {
            this.logLock = new object();
            this.m_workerPool = new List<Thread>(WorkerCount);
            this.m_serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.m_ApplyQueue = new BlockingCollection<string>(QueueCapacity);

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
        protected XmlSerializer Serializer
        {
            get { return this.m_serializer; }
        }
        protected BlockingCollection<string> ApplyQueue
        {
            get { return this.m_ApplyQueue; }
        }
        #endregion

        #region 生产者方法
        public void Deserialize(string applyInfo)
        {
            WriteLog(applyInfo);
            if (!string.IsNullOrEmpty(applyInfo))
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    WriteLog("处理xml报文成功");
                    this.HandleApplyInfo(info);
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog("处理xml报文异常");
                }
            }
            else
            {
                WriteLog("报文为空");
            }
        }
        protected void HandleApplyInfo(LabApplyInfo applyInfo)
        {
            List<ApplyItem> applyCollection = applyInfo.ApplyCollection;
            if (applyCollection != null && applyCollection.Count > 0)
            {
                foreach (ApplyItem item in applyCollection)
                {
                    //获取审核的报告单主键
                    if (item.ApplyStatus == 7)
                    {
                        //
                        WriteLog("申请单号为" + item.ApplyNo + "的报告加入存储队列");
                        this.ApplyQueue.Add(item.ApplyNo);
                    }
                }
            }
        }
        #endregion

        #region 消费者方法
        public void HandleReport()
        {
            foreach (string appNo in this.ApplyQueue.GetConsumingEnumerable())
            {
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
        }
        #endregion

        #region
        protected void WriteLog(string message)
        {
            lock (this.logLock)
            {
                var file = System.IO.File.AppendText("D:\\log\\log.txt");
                file.WriteLine(message);
                file.Close();
            }
        }
        #endregion
    }
}