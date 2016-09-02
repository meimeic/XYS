using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Configuration;
using System.Reflection;
using System.Threading;

using log4net;

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
        private static readonly ILog LOG;
        private static readonly LabService ServiceInstance;
        private static readonly string ImageServer;
       
        private readonly IHandle InfoHandler;
        private readonly IHandle ItemHandler;
        private readonly IHandle ImageHandler;
        private readonly IHandle GSItemHandler;
        private readonly IHandle GSCustomHandler;
        private readonly IHandle GeneCustomHandler;

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
            LOG = LogManager.GetLogger("LabReport");
            ImageServer = ConfigurationManager.AppSettings["LabImageServer"].ToString();

            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.PKDAL = new LabPKDAL();
            this.ReportDAL = new LabReportDAL();

            this.InfoHandler = new InfoHandle(this.ReportDAL);
            this.ItemHandler = new ItemHandle(this.ReportDAL);
            this.ImageHandler = new ImageHandle(this.ReportDAL);
            this.GSItemHandler = new GSItemHandle(this.ReportDAL);
            this.GSCustomHandler = new GSCustomHandle(this.ReportDAL);
            this.GeneCustomHandler = new GeneCustomHandle(this.ReportDAL);

            this.Item2CustomMap = new Hashtable(20);
            this.ImageNormalMap = new Hashtable(40);
            this.RequestQueue = new BlockingCollection<LabPK>(5000);

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
        public void InitReport(string where)
        {
            LOG.Info("根据where语句获取主键集合:"+where);
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(where, PKList);
            this.InitReport(PKList);
        }
        public void InitReport(List<LabPK> PKList)
        {
            if (PKList != null)
            {
                foreach (LabPK pk in PKList)
                {
                    this.InitReport(pk);
                }
            }
        }
        public void InitReport(LabPK PK)
        {
            if (PK != null && PK.Configured)
            {
                LOG.Info("将ID为" + PK.ID + "的主键添加到待处理队列");
                this.RequestQueue.Add(PK);
            }
        }
        #endregion

        #region 消费者方法
        protected void Consumer()
        {
            foreach (LabPK pk in this.RequestQueue.GetConsumingEnumerable())
            {
                LOG.Info("开始处理报告,报告ID为:" + pk.ID);
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
                LOG.Info("报告后续处理");
                this.HandleAfter(report, RK);
                LOG.Info("触发处理完成事件");
                this.OnSuccess(report);
            }
            else
            {
                LOG.Info("触发处理失败事件");
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
                //无图无特殊内容
                case 2:
                case 27:
                case 62:
                case 17:
                case 23:
                case 29:
                case 34:
                case 5:
                case 19:
                case 20:
                case 21:
                case 25:
                case 30:
                case 33:
                case 63:
                case 14:
                case 4:
                case 24:
                case 18:
                    LOG.Info("通用无图类型报告处理");
                    result = this.InnerHandleItem(report, RK);
                    break;
                //基因配型
                case 45:
                    LOG.Info("基因配型报告处理");
                    result = this.InnerHandleGene(report, RK);
                    break;
                case 39:
                    LOG.Info("骨髓报告处理");
                    result = this.InnerHandleGS(report, RK);
                    break;
                //默认
                default:
                    LOG.Info("通用有图类型报告处理（默认处理方式）");
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
            LOG.Info("报告明细项处理");
            result = this.ItemHandler.HandleElement(ls, RK, typeof(ItemElement));
            if (result)
            {
                this.ItemConvert(report, ls);
                report.ItemList.Sort();
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
                result = this.GeneCustomHandler.HandleElement(ci, key);
                if (result)
                {
                    this.GeneConvert(custom, ci);
                    items = new List<IFillElement>(5);
                    result = this.ItemHandler.HandleElement(items, key, typeof(ItemElement));
                    if (result)
                    {
                        this.GeneConvert(custom, report.SuperList, items);
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
        private bool InnerHandleGS(LabReport report, LabPK RK)
        {
            bool result = false;
            List<IFillElement> ls = new List<IFillElement>(50);
            result = this.GSItemHandler.HandleElement(ls, RK, typeof(GSItem));
            if (result)
            {
                this.GSItemConvert(ls, report.CustomList);
                ls.Clear();
                result = this.GeneCustomHandler.HandleElement(ls, RK, typeof(GSCustom));
                if (result)
                {
                    this.GSCustomConvert(report.Info, report.ImageList, ls);
                }
            }
            return result;
        }
        //将内部数据转换成通讯数据
        private void ItemConvert(LabReport lr, List<IFillElement> ItemList)
        {
            bool cb = false;
            ItemElement item = null;
            CustomElement custom = new CustomElement();
            foreach (IFillElement element in ItemList)
            {
                item = element as ItemElement;
                if (item != null)
                {
                    if (!lr.SuperList.Contains(item.SuperNo))
                    {
                        lr.SuperList.Add(item.SuperNo);
                    }
                    if (this.ConvertCustom(item, custom))
                    {
                        cb = true;
                        continue;
                    }
                    if (this.IsItemLegal(item))
                    {
                        lr.ItemList.Add(item);
                    }
                }
            }
            if (cb)
            {
                lr.CustomList.Add(custom);
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
        private void GeneConvert(CustomElement custom, List<int> superList, List<IFillElement> ls)
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
            propName = GetPropNameByIndex(index);
            this.SetProperty(propName, item.CName, custom);
            string[] values = item.Result.Split(new char[] { ';' });
            if (values.Length > 1)
            {
                index++;
                propName = GetPropNameByIndex(index);
                this.SetProperty(propName, values[0], custom);

                index++;
                propName = GetPropNameByIndex(index);
                this.SetProperty(propName, values[0], custom);
            }
        }
        private void GSItemConvert(List<IFillElement> ls, List<CustomElement> customs)
        {
            GSItem item = null;
            string propName = null;
            CustomElement blood = new CustomElement();
            CustomElement marrow = new CustomElement();
            foreach (IFillElement element in ls)
            {
                item = element as GSItem;
                if (item != null)
                {
                    propName = GetPropNameByIndex(item.ItemNo);
                    this.SetProperty(propName, item.BloodValue.ToString(), blood);
                    this.SetProperty(propName, item.MarrowValue.ToString(), marrow);
                }
            }
            customs.Add(blood);
            customs.Add(marrow);
        }
        private void GSCustomConvert(InfoElement info, List<ImageElement> images, List<IFillElement> ls)
        {
            GSCustom custom = null;
            ImageElement image = null;
            foreach (IFillElement element in ls)
            {
                custom = element as GSCustom;
                if (custom != null)
                {
                    if (custom.ItemNo == 43)
                    {
                        //结论
                        info.Comment = custom.ReportComment;
                    }
                    else if (custom.ItemNo == 44)
                    {
                        //描述
                        info.Description = custom.ReportDescribe;
                    }
                    else if (custom.IsFile == 1)
                    {
                        if (custom.Graph != null)
                        {
                            image = new ImageElement();
                            image.ReportID = custom.ReportID;
                            image.Name = custom.ItemNo.ToString();
                            image.Value = custom.Graph;
                            images.Add(image);
                        }
                    }
                }
            }
        }
        private string GetPropNameByIndex(int no)
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
        private bool IsItemLegal(ItemElement rie)
        {
            if (rie.SecretGrade > 0)
            {
                return false;
            }
            return true;
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
        #endregion

        #region 后续处理
        //11小组添加标准图片（通讯数据整体处理）
        private void HandleAfter(LabReport report, IReportKey RK)
        {
            this.HandleAfterImage(report, RK.ID);
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
            lock (Item2CustomMap)
            {
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
        }
        private void InitImageNormalMap()
        {
            lock (ImageNormalMap)
            {
                ImageNormalMap.Clear();
                ImageNormalMap.Add(50006570, "/normal/AML1-ETO.jpg");
                ImageNormalMap.Add(90009363, "/normal/ATM-CEP11.jpg");
                ImageNormalMap.Add(90008499, "/normal/BCL2.jpg");
                ImageNormalMap.Add(90008738, "/normal/BCL6.jpg");
                ImageNormalMap.Add(50006569, "/normal/BCR-ABL.jpg");
                ImageNormalMap.Add(90009026, "/normal/BCR-ABL-ASS1.jpg");
                ImageNormalMap.Add(50006576, "/normal/CBFB.jpg");
                ImageNormalMap.Add(50006583, "/normal/CCND1-IgH.jpg");
                ImageNormalMap.Add(90009367, "/normal/CDKN2A.jpg");
                ImageNormalMap.Add(90008735, "/normal/CEP12.jpg");
                ImageNormalMap.Add(90008495, "/normal/D7S486-CEP7.jpg");
                ImageNormalMap.Add(50006573, "/normal/CEPX-CEPY.jpg");
                ImageNormalMap.Add(90008729, "/normal/CKS1B-CDKN2C.jpg");
                ImageNormalMap.Add(90009373, "/normal/CRLF2.jpg");
                ImageNormalMap.Add(90009370, "/normal/D13S319.jpg");
                ImageNormalMap.Add(90008497, "/normal/D20S108.jpg");
                ImageNormalMap.Add(90008494, "/normal/EGR1-D5S721.jpg");
                ImageNormalMap.Add(90008741, "/normal/EVI1.jpg");
                ImageNormalMap.Add(90008730, "/normal/FGFR1-D8Z2.jpg");
                ImageNormalMap.Add(50006574, "/normal/IgH.jpg");
                ImageNormalMap.Add(90009041, "/normal/IGH-BCL2.jpg");
                ImageNormalMap.Add(50006582, "/normal/FGFR3-IgH.jpg");
                ImageNormalMap.Add(50006581, "/normal/MAF-IgH.jpg");
                ImageNormalMap.Add(90009364, "/normal/IGH-MAFB.jpg");
                ImageNormalMap.Add(90009038, "/normal/IGH-MYC.jpg");
                ImageNormalMap.Add(50006575, "/normal/MLL.jpg");
                ImageNormalMap.Add(50006579, "/normal/MYC.jpg");
                ImageNormalMap.Add(90008744, "/normal/MYC.jpg");
                ImageNormalMap.Add(90009376, "/normal/P2RY8.jpg");
                ImageNormalMap.Add(90009362, "/normal/P53-CEP17.jpg");
                ImageNormalMap.Add(90009032, "/normal/PDGFRA.jpg");
                ImageNormalMap.Add(90009035, "/normal/PDGFRB.jpg");
                ImageNormalMap.Add(50006571, "/normal/PML-RARA.jpg");
                ImageNormalMap.Add(90009029, "/normal/RARA.jpg");
                ImageNormalMap.Add(50006578, "/normal/RB-1.jpg");
                ImageNormalMap.Add(90008496, "/normal/TCF3-PBX1.jpg");
                ImageNormalMap.Add(50006572, "/normal/TEL-AML1.jpg");
                ImageNormalMap.Add(50006577, "/normal/SANTI8.jpg");
            }
        }
        #endregion
    }
}