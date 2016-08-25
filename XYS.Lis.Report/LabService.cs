using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

using XYS.Util;
using XYS.Model;
using XYS.Model.Lab;
using XYS.Report;

using XYS.Lis.Report.Handler;
using XYS.Lis.Report.Persistent;
using XYS.Lis.Report.Model;
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
        private readonly IHandle CustomHandler;

        private Thread[] m_workerPool;
        private readonly LabPKDAL PKDAL;
        private readonly LabReportDAL ReportDAL;
        private readonly Hashtable Item2CustomMap;
        private readonly Hashtable ImageNormalMap;
        private readonly BlockingCollection<LabPK> RequestQueue;
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
            this.CustomHandler = new GeneCustomHandle(this.ReportDAL);

            this.Item2CustomMap = new Hashtable(20);
            this.ImageNormalMap = new Hashtable(40);
            this.RequestQueue = new BlockingCollection<LabPK>(1000);

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

        #region 公共方法/生产者方法
        public void InitReport(Require req)
        {
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(req, PKList);
            this.InitReport(PKList);
        }
        public void InitReport(string where)
        {
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(where, PKList);
            this.InitReport(PKList);
        }
        public void InitReport(List<LabPK> PKList)
        {
            foreach (LabPK pk in PKList)
            {
                this.InitReport(pk);
            }
        }
        public void InitReport(LabPK PK)
        {
            if (PK != null && PK.Configured)
            {
                this.RequestQueue.Add(PK);
            }
        }
        #endregion

        #region 消费者方法
        protected void Consumer()
        {
            foreach (LabPK pk in this.RequestQueue.GetConsumingEnumerable())
            {
                //LOG.Info("报告处理线程开始处理报告,报告ID为:" + report.ReportPK.ID);
                this.InnerHandle(pk);
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

        #region 内部处理方法
        //内部操作，处理成功后触发成功事件，失败后触发失败事件
        private void InnerHandle(LabPK RK)
        {
            LabReport report = new LabReport();
            bool result = this.InnerHandle(report, RK);
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
        private bool InnerHandle(LabReport report, LabPK RK)
        {
            bool result = false;
            //处理info
            result = this.InfoHandler.HandleElement(report.Info, RK);
            switch (RK.SectionNo)
            {
                //无图
                //临检
                case 2:
                case 27:
                case 62:
                //生化
                case 17:
                case 23:
                case 29:
                case 34:
                //免疫
                case 5:
                case 19:
                case 20:
                case 21:
                case 25:
                case 30:
                case 33:
                case 63:
                case 14:
                //止血
                case 4:
                case 24:
                //溶血
                case 18:
                    result = this.InnerHandleItem(report, RK);
                    break;
                //基因配型
                case 45:
                    result = this.InnerHandleGene(report, RK);
                    break;
                //默认
                default:
                    result = this.InnerHandleItem(report, RK);
                    result = this.InnerHandleImage(report, RK);
                    break;
            }
            return result;
        }
        private bool InnerHandleItem(LabReport report, LabPK RK)
        {
            bool result = false;
            List<IFillElement> ls = new List<IFillElement>(32);
            result = this.ItemHandler.HandleElement(ls, RK, typeof(ItemElement));
            if (result)
            {
                this.ItemConvert(report, ls);
            }
            return result;
        }
        private bool InnerHandleImage(LabReport report, LabPK RK)
        {
            bool result = false;
            List<IFillElement> ls = new List<IFillElement>(6);
            //处理image
            result = this.ImageHandler.HandleElement(ls, RK, typeof(ImageElement));
            if (result)
            {
                this.ImageConvert(report, ls);
            }
            return result;
        }
        private bool InnerHandleGene(LabReport report, LabPK RK)
        {
            bool result = false;
            GeneCustom ci = null;
            CustomElement custom = null;
            List<IFillElement> items = null;
            List<LabPK> PKList = new List<LabPK>(3);
            this.PKDAL.InitKey(RK, PKList);
            foreach (LabPK key in PKList)
            {
                ci = new GeneCustom();
                custom = new CustomElement();
                result = this.CustomHandler.HandleElement(ci, RK);
                if (result)
                {
                    this.GeneConvert(custom, ci);
                    items = new List<IFillElement>(5);
                    result = this.ItemHandler.HandleElement(items, RK, typeof(ItemElement));
                    if (result)
                    {
                        this.GeneConvert(custom, items, report.SuperList);
                    }
                }
                else
                {
                    break;
                }
                report.CustomList.Add(custom);
            }
            return result;
        }
        //将内部数据转换成通讯数据
        private void ItemConvert(LabReport lr, List<IFillElement> ItemList)
        {
            ItemElement item = null;
            foreach (IFillElement element in ItemList)
            {
                item = element as ItemElement;
                if (item != null)
                {
                    lr.ItemList.Add(item);
                }
            }
        }
        private void ImageConvert(LabReport lr, List<IFillElement> ImageList)
        {
            ImageElement image = null;
            foreach (IFillElement element in ImageList)
            {
                image = element as ImageElement;
                if (image != null)
                {
                    lr.ImageList.Add(image);
                }
            }
        }
        private void GeneConvert(CustomElement custom, GeneCustom item)
        {
            custom.C0 = item.SampleNo;
            custom.C1 = item.Name;
            custom.C2 = item.Age;
            custom.C3 = item.Relation;
            custom.C4 = item.EqualCount;
        }
        private void GeneConvert(CustomElement custom, List<IFillElement> ls, List<int> superList)
        {
            //排序
            ls.Sort((x, y) =>
            {
                ItemElement x1 = x as ItemElement;
                ItemElement y1 = y as ItemElement;
                if (x1 != null && y1 != null)
                {
                    return x1.DispOrder - y1.DispOrder;
                }
                else
                {
                    return 0;
                }
            });
            //转换
            int index = 5;
            ItemElement item = null;
            foreach (IFillElement ele in ls)
            {
                item = ele as ItemElement;
                if (item != null)
                {
                    if (!superList.Contains(item.SuperNo))
                    {
                        superList.Add(item.SuperNo);
                    }
                    this.GeneConvert(custom, item, index);
                    index = index + 3;
                }
            }
        }
        private void GeneConvert(CustomElement custom, ItemElement item, int index)
        {
            string propName = null;
            propName = GetGenePropName(index);
            this.SetProperty(propName, item.CName, custom);
            string[] values = item.Result.Split(new char[] { ';' });
            if (values.Length > 1)
            {
                index++;
                propName = GetGenePropName(index);
                this.SetProperty(propName, values[0], custom);

                index++;
                propName = GetGenePropName(index);
                this.SetProperty(propName, values[0], custom);
            }
        }
        private string GetGenePropName(int no)
        {
            int m = no % CustomElement.PropertyCount;
            return "C" + m;
        }
        private void SetProperty(string propName, object value, CustomElement element)
        {
            try
            {
                PropertyInfo pro = element.GetType().GetProperty(propName);
                if (pro != null)
                {
                    pro.SetValue(element, value, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region 后续处理
        //11小组添加标准图片（通讯数据整体处理）
        private void HandleAfter(LabReport report, IReportKey RK)
        {
            this.HandleAfterItem(report);
            this.HandleAfterImage(report, RK.ID);
        }
        private void HandleAfterItem(LabReport report)
        {
            ItemElement item = null;
            List<int> superList = report.SuperList;
            List<ItemElement> items = report.ItemList;
            if (items.Count > 0)
            {
                CustomElement custom = new CustomElement();
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    item = items[i];
                    if (!superList.Contains(item.SuperNo))
                    {
                        superList.Add(item.SuperNo);
                    }
                    if (ConvertCustom(item, custom))
                    {
                        items.RemoveAt(i);
                        continue;
                    }
                    if (!IsItemLegal(item))
                    {
                        items.RemoveAt(i);
                        continue;
                    }
                }
                report.CustomList.Add(custom);
            }
        }
        private bool ConvertCustom(ItemElement item, CustomElement custom)
        {
            string key = Item2CustomMap[item.ItemNo] as string;
            if (key != null)
            {
                this.SetProperty(key, item.Result, custom);
                return true;
            }
            return false;
        }
        private bool IsItemLegal(ItemElement rie)
        {
            if (rie.SecretGrade > 0)
            {
                return false;
            }
            return true;
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
        #endregion
     
        #region 初始化
        private void Init()
        {
            this.InitWorkerPool();
            this.InitItem2CustomMap();
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
        private void InitItem2CustomMap()
        {
            this.Item2CustomMap.Clear();

            this.Item2CustomMap.Add(90009288, "C0");
            this.Item2CustomMap.Add(90009289, "C1");
            this.Item2CustomMap.Add(90009290, "C2");
            this.Item2CustomMap.Add(90009291, "C3");
            this.Item2CustomMap.Add(90009292, "C4");
            this.Item2CustomMap.Add(90009293, "C5");
            this.Item2CustomMap.Add(90009294, "C6");
            this.Item2CustomMap.Add(90009295, "C7");
            this.Item2CustomMap.Add(90009296, "C8");
            this.Item2CustomMap.Add(90009297, "C9");
            this.Item2CustomMap.Add(90009300, "C10");
            this.Item2CustomMap.Add(90009301, "C11");

            this.Item2CustomMap.Add(90008528, "C0");
            this.Item2CustomMap.Add(90008797, "C1");
            this.Item2CustomMap.Add(90008798, "C2");
            this.Item2CustomMap.Add(90008799, "C3");
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