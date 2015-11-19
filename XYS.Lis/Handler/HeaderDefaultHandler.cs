using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
{
    /// <summary>
    /// 通用处理，删除各个集合中错误类型对象
    /// </summary>
    public class HeaderDefaultHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "HeaderDefaultHandler";
        private readonly Hashtable m_parItem2NormalImage;
        #endregion

        #region 构造函数
        public HeaderDefaultHandler()
            : this(m_defaultHandlerName)
        {
        }
        public HeaderDefaultHandler(string handlerName)
            : base(handlerName)
        {
            this.m_parItem2NormalImage = new Hashtable(20);
        }
        #endregion

        #region 实现父类继承接口的抽象方法

        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            ReportReportElement rre;
            bool result = IsElement(reportElement, reportElement.ElementTag);
            if (result)
            {
                if (reportElement.ElementTag == ReportElementTag.ReportElement)
                {
                    rre = reportElement as ReportReportElement;
                    OperateReport(rre);
                }
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }
        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag != ReportElementTag.NoneElement)
            {
                OperateElementList(reportElementList, elementTag);
                if (reportElementList.Count > 0)
                {
                    return HandlerResult.Continue;
                }
                else
                {
                    return HandlerResult.Fail;
                }
            }
            return HandlerResult.Fail;
        }
        #endregion

        #region 实现父类受保护的抽象方法
        protected override void OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            ReportReportElement rre;
            if (elementTag == ReportElementTag.ReportElement)
            {
                rre = element as ReportReportElement;
                OperateReport(rre);
            }
        }
        #endregion

        #region
        protected virtual void OperateReport(ReportReportElement rre)
        {
            OperateInfoList(rre);
            OperateItemList(rre);
            OperateGraphList(rre);
        }
        protected virtual void OperateInfoList(ReportReportElement rre)
        {
            //exam 处理
            ReportInfoElement rie;
            List<ILisReportElement> infoList = rre.GetReportItem(ReportElementTag.InfoElement);
            OperateElementList(infoList, ReportElementTag.InfoElement);
            if (infoList.Count > 0)
            {
                rie = infoList[0] as ReportInfoElement;
                rre.SectionNo = rie.SectionNo;
                rre.ParItemName = rie.ParItemName;
                //设置签名图片
                if (rie.Checker != null && !rie.Checker.Equals(""))
                {
                    rre.CheckerImage = LisPUser.GetSignImage(rie.Checker);
                }
                if (rie.Technician != null && !rie.Technician.Equals(""))
                {
                    rre.TechnicianImage = LisPUser.GetSignImage(rie.Technician);
                }
                rre.CollectDateTime = rie.CollectDateTime;
                rre.InceptDateTime = rie.InceptDateTime;
                rre.TestDateTime = rie.TestDateTime;
                rre.CheckDateTime = rie.CheckDateTime;
                rre.SecondeCheckDateTime = rie.SecondeCheckDateTime;
                rre.ClinicType = rie.ClinicType;
            }
        }
        protected virtual void OperateItemList(ReportReportElement rre)
        {
            //item 处理
            ReportItemElement rie;
            List<ILisReportElement> itemElementList = rre.GetReportItem(ReportElementTag.ItemElement);
            List<ILisReportElement> customElementList = rre.GetReportItem(ReportElementTag.CustomElement);
            if (itemElementList.Count > 0)
            {
                for (int i = itemElementList.Count - 1; i >= 0; i--)
                {
                    rie = itemElementList[i] as ReportItemElement;
                    if (rie != null)
                    {
                        //设置ParItemList
                        if (!rre.ParItemList.Contains(rie.ParItemNo))
                        {
                            rre.ParItemList.Add(rie.ParItemNo);
                        }
                        //设置是否存在备注
                        if (rie.ItemNo == 90008462)
                        {
                            rre.RemarkFlag = true;
                        }
                        //item是否转换为custom
                        if (ItemConvert2Custom(rie, customElementList))
                        {
                            itemElementList.RemoveAt(i);
                        }
                    }
                    else
                    {
                        itemElementList.RemoveAt(i);
                    }
                }
            }
        }
        protected virtual void OperateGraphList(ReportReportElement rre)
        {
            //graph 处理
            List<ILisReportElement> graphElementList = rre.GetReportItem(ReportElementTag.GraphElement);
            OperateElementList(graphElementList, ReportElementTag.GraphElement);
            AddImageByParItem(rre.ParItemList, graphElementList);
        }
        #endregion

        #region item转换成custom
        private bool ItemConvert2Custom(ReportItemElement rie,List<ILisReportElement> customList)
        {
            bool result = false;
            switch (rie.ItemNo)
            {
                case 90009288:     //血常规项目c8
                case 90009289:      //c9
                case 90009290:     //c10
                case 90009291:     //c11
                case 90009292:     //c12
                case 90009293:    //c13
                case 90009294:    //c14
                case 90009300:    //c0  
                case 90009295:    //c15
                case 90009296:    //c16
                case 90009297:   //c17
                case 90009301:   //c1
                case 90008528:    //染色体
                case 90008797:
                case 90008798:
                case 90008799:
                    result = true;
                    AddItem2CustomList(rie, customList);
                    break;
                default:
                    break;
            }
            return result;
        }
        private void AddItem2CustomList(ReportItemElement item, List<ILisReportElement> customElementList)
        {
            string propertyName;
            if (customElementList.Count == 0)
            {
                customElementList.Add(new ReportCustomElement());
            }
            ReportCustomElement rce = customElementList[0] as ReportCustomElement;
            if (rce != null)
            {
                propertyName = GetCustomPropertyName(item.ItemNo);
                SetCustomProperty(propertyName, item.ItemResult, rce);
            }
        }
        private void SetCustomProperty(string propertyName, object value, ReportCustomElement rce)
        {
            try
            {
                PropertyInfo pro = rce.GetType().GetProperty(propertyName);
                if (pro != null)
                {
                    pro.SetValue(rce, value, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetCustomPropertyName(int itemNo)
        {
            int m = itemNo % ReportCustomElement.COLUMN_COUNT;
            return "Column" + m;
        }
        #endregion

        #region 图片项处理
        private void AddImageByParItem(List<int> parItemList, List<ILisReportElement> graphElementList)
        {
            byte[] imageValue;
            ReportGraphElement rge;
            if (this.m_parItem2NormalImage.Count == 0)
            {
                this.InitParItem2NormalImage();
            }
            foreach (int parItemNo in parItemList)
            {
                imageValue = this.m_parItem2NormalImage[parItemNo] as byte[];
                if (imageValue != null)
                {
                    rge = new ReportGraphElement();
                    rge.GraphName = parItemNo.ToString();
                    rge.GraphImage = imageValue;
                    graphElementList.Add(rge);
                }
            }
        }
        private void InitParItem2NormalImage()
        {
            lock (this.m_parItem2NormalImage)
            {
                LisMap.InitParItem2NormalImageTable(this.m_parItem2NormalImage);
            }
        }
        #endregion
    }
}
