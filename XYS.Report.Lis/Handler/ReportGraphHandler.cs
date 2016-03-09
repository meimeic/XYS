using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Report.Util;
using XYS.Report.Model.Lis;
namespace XYS.Report.Lis.Handler
{
    public class ReportGraphHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
        private readonly Hashtable m_parItemNo2NormalImage;
        #endregion

        #region 构造函数
        public ReportGraphHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportGraphHandler(string handlerName)
            : base(handlerName)
        {
            this.m_parItemNo2NormalImage = new Hashtable(20);
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(IReportElement element)
        {
            ReportGraphElement rge = element as ReportGraphElement;
            if (rge != null)
            {
                return true;
            }
            return false;
        }
        protected override bool OperateReport(ReportReportElement report)
        {
            return OperateGraph(report);
        }
        #endregion

        #region graph项的内部处理逻辑
        protected virtual bool OperateGraph(ReportReportElement rre)
        {
            switch (rre.SectionNo)
            {
                case 11:
                    AddImageByParItem(rre);
                    break;
                default:
                    break;
            }
            OperateElementList(rre.GetReportItem(typeof(ReportGraphElement).Name));
            Convert2Image(rre);
            return true;
        }
        #endregion

        #region
        protected void Convert2Image(ReportReportElement rre)
        {
            List<IReportElement> graphList = rre.GetReportItem(typeof(ReportGraphElement).Name);
            if (IsExist(graphList))
            {
            }
        }
        #endregion
        #region 图片项添加处理
        private void AddImageByParItem(ReportReportElement rre)
        {
            byte[] imageValue;
            ReportGraphElement rge;
            List<IReportElement> graphList = rre.GetReportItem(typeof(ReportGraphElement).Name);
            if (this.m_parItemNo2NormalImage.Count == 0)
            {
                this.InitParItem2NormalImage();
            }
            foreach (int parItemNo in rre.ParItemList)
            {
                imageValue = this.m_parItemNo2NormalImage[parItemNo] as byte[];
                if (imageValue != null)
                {
                    rge = new ReportGraphElement();
                    rge.GraphName = "normal";
                    rge.GraphImage = imageValue;
                    graphList.Add(rge);
                }
            }
        }
        private void InitParItem2NormalImage()
        {
            lock (this.m_parItemNo2NormalImage)
            {
                LisConfiguration.InitParItem2NormalImageTable(this.m_parItemNo2NormalImage);
            }
        }
        #endregion
    }
}
