using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using log4net;

using XYS.Util;
using XYS.Report;
using XYS.Lis.Report.Util;
using XYS.Lis.Report.Model;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report
{
    public delegate void HandleErrorHandler(LabReport report);
    public delegate void HandleSuccessHandler(LabReport report);
    public class ReportService
    {
        #region 静态变量
        private static ILog LOG;
        private static int WorkerCount;
        private static readonly Hashtable Type2HandleMap;
        private static readonly Hashtable Section2FillTypeMap;
        private static readonly ReportService ServiceInstance;

        private readonly ReportPKDAL PKDAL;
        private readonly BlockingCollection<LabReport> InitRequestQueue;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleCompleteEvent;
        #endregion

        #region 构造函数
        static ReportService()
        {
            WorkerCount = 10;
            Type2HandleMap = new Hashtable(5);
            Section2FillTypeMap = new Hashtable(20);
            ServiceInstance = new ReportService();
            LOG = LogManager.GetLogger("LisReportHandle");
        }
        private ReportService()
        {
            this.PKDAL = new ReportPKDAL();
            this.InitRequestQueue = new BlockingCollection<LabReport>(1000);
            this.Init();
        }
        #endregion

        #region 静态属性
        public static ReportService LabService
        {
            get { return ServiceInstance; }
        }
        #endregion
        
        #region 事件属性
        public event HandleErrorHandler HandleErrorEvent
        {
            add { this.m_handleErrorEvent += value; }
            remove { this.m_handleErrorEvent -= value; }
        }
        public event HandleSuccessHandler HandleCompleteEvent
        {
            add { this.m_handleCompleteEvent += value; }
            remove { this.m_handleCompleteEvent -= value; }
        }
        #endregion

        #region 公共方法
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            PKDAL.InitReportKey(req, PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            PKDAL.InitReportKey(where, PKList);
        }
        #endregion

        #region 生产者方法
        public void InitReport(LabReport report)
        {
            LOG.Info("将处理报告请求加入到处理请求队列");
            this.InitRequestQueue.Add(report);
        }
        #endregion

        #region 消费者方法
        protected void InitRequestConsumer()
        {
            foreach (LabReport report in this.InitRequestQueue.GetConsumingEnumerable())
            {
                LOG.Info("报告处理线程开始处理报告,报告ID为:" + report.ReportPK.ID);
                this.InnerHandle(report);
            }
        }
        #endregion

        #region 辅助方法
        protected List<Type> GetFillTypes(ReportPK RK)
        {
            return Section2FillTypeMap[RK.SectionNo] as List<Type>;
        }
        protected IHandle GetHandle(Type type)
        {
            return Type2HandleMap[type] as IHandle;
        }

        private static void InitFillTable()
        {
            lock (Section2FillTypeMap)
            {
                ConfigManager.InitSection2FillElementTable(Section2FillTypeMap);
            }
        }
        private static void InitHandleTable()
        {
            lock (Type2HandleMap)
            {
                ConfigManager.InitType2HandleTable(Type2HandleMap);
            }
        }
        #endregion

        #region 触发事件
        protected void OnError(LabReport report)
        {
            HandleErrorHandler handler = this.m_handleErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnSuccess(LabReport report)
        {
            HandleSuccessHandler handler = this.m_handleCompleteEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion

        #region 私有方法
        private void Init()
        {
        }
        private void InnerHandle(LabReport report)
        {
            bool result = false;
            IHandle handle = null;
            ReportPK RK = report.ReportPK;
            List<Type> lt = GetFillTypes(RK);
            List<IFillElement> elements = null;
            if (lt != null && lt.Count > 0)
            {
                foreach (Type type in lt)
                {
                    handle = GetHandle(type);
                    if (handle != null)
                    {
                        elements = new List<IFillElement>(10);
                        result = handle.InitElement(elements, RK, type);
                        if (!result)
                        {
                            break;
                        }
                        report.ItemTable.Add(type.Name, elements);
                    }
                }
            }
            if (result)
            {
                OnSuccess(report);
            }
            else
            {
                OnError(report);
            }
        }
        #endregion
    }
}
