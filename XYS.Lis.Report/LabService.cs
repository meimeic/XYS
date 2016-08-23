using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using XYS.Util;
using XYS.Model;
using XYS.Model.Lab;
using XYS.Report;

using XYS.Lis.Report.Handler;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report
{
    public delegate void HandleErrorHandler(LabReport report);
    public delegate void HandleSuccessHandler(LabReport report);
    public class LabService
    {
        #region 静态变量
        private static int WorkerCount;
        private static readonly LabService ServiceInstance;
        private static readonly string ImageServer = "http://img.xys.com:8080/lab/normal";

        private readonly IHandle InfoHandler;
        private readonly IHandle ItemHandler;
        private readonly IHandle ImageHandler;

        private Thread[] m_workerPool;
        private readonly LabPKDAL PKDAL;
        private readonly LabReportDAL ReportDAL;
        private readonly Hashtable ImageNormalMap;
        private readonly BlockingCollection<ReportElement> RequestQueue;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleCompleteEvent;
        #endregion

        #region 构造函数
        static LabService()
        {
            WorkerCount = 2;
            //LOG = LogManager.GetLogger("Lab");

            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.PKDAL = new LabPKDAL();
            this.ReportDAL = new LabReportDAL();

            this.InfoHandler = new InfoHandle(this.ReportDAL);
            this.ItemHandler = new ItemHandle(this.ReportDAL);
            this.ImageHandler = new ImageHandle(this.ReportDAL);

            this.ImageNormalMap = new Hashtable(40);
            this.RequestQueue = new BlockingCollection<ReportElement>(1000);

            this.Init();
        }
        #endregion

        #region 静态属性
        public static LabService LService
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
        public void InitReport(Require req)
        {
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(req, PKList);
            this.HandleReport(PKList);
        }
        public void InitReport(string where)
        {
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(where, PKList);
            this.HandleReport(PKList);
        }
        public void InitReport(LabPK PK)
        {
            if (PK != null && PK.Configured)
            {
                ReportElement report = new ReportElement();
                report.ReportPK = PK;
                this.HandleReport(report);
            }
        }
        #endregion

        #region 生产者方法
        private void HandleReport(List<LabPK> PKList)
        {
            ReportElement report = null;
            foreach (LabPK pk in PKList)
            {
                report = new ReportElement();
                report.ReportPK = pk;
                this.HandleReport(report);
            }
        }
        private void HandleReport(ReportElement report)
        {
            //LOG.Info("将处理报告请求加入到处理请求队列");
            this.RequestQueue.Add(report);
        }
        #endregion

        #region 消费者方法
        protected void Consumer()
        {
            foreach (ReportElement report in this.RequestQueue.GetConsumingEnumerable())
            {
                //LOG.Info("报告处理线程开始处理报告,报告ID为:" + report.ReportPK.ID);
                this.InnerHandle(report);
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
        //内部操作，处理成功后触发成功事件，失败后触发失败事件
        private void InnerHandle(ReportElement RE)
        {
            LabPK RK = RE.ReportPK;
            LabReport report = new LabReport();
            bool result = this.InnerHandle(RE, RK);
            this.InnerConvert(report, RE);
            if (result)
            {
                //后续处理
                this.HandleAfter(report, RK);
                this.OnSuccess(report);
            }
            else
            {
                this.OnError(report);
            }
        }
        //数据填充、清洗、转换等操作
        private bool InnerHandle(ReportElement report, LabPK RK)
        {
            bool result = false;
            //处理info
            result = this.InfoHandler.HandleElement(report.Info, RK);
            if (result)
            {
                //处理item
                result = this.ItemHandler.HandleElement(report.ItemList, RK, typeof(ItemElement));
                if (result)
                {
                    //处理image
                    result = this.ImageHandler.HandleElement(report.ImageList, RK, typeof(ImageElement));
                }
            }
            return result;
        }
        //11小组添加标准图片（通讯数据整体处理）
        private void HandleAfter(LabReport report, IReportKey RK)
        {
            this.HandleAfterItem(report);
            this.HandleAfterImage(report, RK.ID);
        }
        //将内部数据转换成通讯数据
        private void InnerConvert(LabReport lr, ReportElement re)
        {
            ItemElement item = null;
            ImageElement image = null;

            lr.Info = re.Info as InfoElement;
            foreach (IFillElement element in re.ItemList)
            {
                item = element as ItemElement;
                if (item != null)
                {
                    lr.ItemList.Add(item);
                }
            }
            foreach (IFillElement element in re.ImageList)
            {
                image = element as ImageElement;
                if (image != null)
                {
                    lr.ImageList.Add(image);
                }
            }
        }
        //后续处理
        private void HandleAfterInfo(LabReport report)
        {
        }
        private void HandleAfterItem(LabReport report)
        {
            ItemElement item = null;
            List<int> superList = report.SuperList;
            List<ItemElement> items = report.ItemList;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                item = items[i];

                //
                if (!superList.Contains(item.SuperNo))
                {
                    superList.Add(item.SuperNo);
                }
                //
                if (!IsItemLegal(item))
                {
                    items.RemoveAt(i);
                }
            }
        }
        private void HandleAfterImage(LabReport report, string id)
        {
            if (report.Info.SectionNo == 11)
            {
                if (report.SuperList.Count > 0)
                {
                    ImageElement normal = this.GetNormalImage(report.SuperList[0], id);
                    if (normal != null)
                    {
                        report.ImageList.Add(normal);
                    }
                }
            }
        }
        private bool IsItemLegal(ItemElement rie)
        {
            if (rie.SecretGrade > 0)
            {
                return false;
            }
            return true;
        }
        private ImageElement GetNormalImage(int no, string id)
        {
            string path = ImageNormalMap[no] as string;
            if (!string.IsNullOrEmpty(path))
            {
                ImageElement element = new ImageElement();
                element.Name = "normal";
                element.ReportID = id;
                element.Url = ImageServer + path;
                return element;
            }
            return null;
        }
        //初始化
        private void Init()
        {
            this.InitWorkerPool();
            this.InitImageNormalMap();
        }
        private void InitWorkerPool()
        {
            Thread th = null;
            this.m_workerPool = new Thread[WorkerCount];
            for (int i = 0; i < WorkerCount; i++)
            {
                th = new Thread(Consumer);
                this.m_workerPool[i] = th;
                th.Start();
            }
        }
        private void InitImageNormalMap()
        {
            lock (ImageNormalMap)
            {
                ImageNormalMap.Clear();
                ImageNormalMap.Add(50006570, "/AML1-ETO.jpg");
                ImageNormalMap.Add(90009363, "/ATM-CEP11.jpg");
                ImageNormalMap.Add(90008499, "/BCL2.jpg");
                ImageNormalMap.Add(90008738, "/BCL6.jpg");
                ImageNormalMap.Add(50006569, "/BCR-ABL.jpg");
                ImageNormalMap.Add(90009026, "/BCR-ABL-ASS1.jpg");
                ImageNormalMap.Add(50006576, "/CBFB.jpg");
                ImageNormalMap.Add(50006583, "/CCND1-IgH.jpg");
                ImageNormalMap.Add(90009367, "/CDKN2A.jpg");
                ImageNormalMap.Add(90008735, "/CEP12.jpg");
                ImageNormalMap.Add(90008495, "/D7S486-CEP7.jpg");
                ImageNormalMap.Add(50006573, "/CEPX-CEPY.jpg");
                ImageNormalMap.Add(90008729, "/CKS1B-CDKN2C.jpg");
                ImageNormalMap.Add(90009373, "/CRLF2.jpg");
                ImageNormalMap.Add(90009370, "/D13S319.jpg");
                ImageNormalMap.Add(90008497, "/D20S108.jpg");
                ImageNormalMap.Add(90008494, "/EGR1-D5S721.jpg");
                ImageNormalMap.Add(90008741, "/EVI1.jpg");
                ImageNormalMap.Add(90008730, "/FGFR1-D8Z2.jpg");
                ImageNormalMap.Add(50006574, "/IgH.jpg");
                ImageNormalMap.Add(90009041, "/IGH-BCL2.jpg");
                ImageNormalMap.Add(50006582, "/FGFR3-IgH.jpg");
                ImageNormalMap.Add(50006581, "/MAF-IgH.jpg");
                ImageNormalMap.Add(90009364, "/IGH-MAFB.jpg");
                ImageNormalMap.Add(90009038, "/IGH-MYC.jpg");
                ImageNormalMap.Add(50006575, "/MLL.jpg");
                ImageNormalMap.Add(50006579, "/MYC.jpg");
                ImageNormalMap.Add(90008744, "/MYC.jpg");
                ImageNormalMap.Add(90009376, "/P2RY8.jpg");
                ImageNormalMap.Add(90009362, "/P53-CEP17.jpg");
                ImageNormalMap.Add(90009032, "/PDGFRA.jpg");
                ImageNormalMap.Add(90009035, "/PDGFRB.jpg");
                ImageNormalMap.Add(50006571, "/PML-RARA.jpg");
                ImageNormalMap.Add(90009029, "/RARA.jpg");
                ImageNormalMap.Add(50006578, "/RB-1.jpg");
                ImageNormalMap.Add(90008496, "/TCF3-PBX1.jpg");
                ImageNormalMap.Add(50006572, "/TEL-AML1.jpg");
                ImageNormalMap.Add(50006577, "/SANTI8.jpg");
            }
        }
        #endregion
    }
}