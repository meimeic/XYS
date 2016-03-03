using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Model;
using XYS.Lis.Core;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
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
        protected override bool OperateReport(ReportReportElement report)
        {
            return OperateGraph(report);
        }
        protected override bool OperateElement(ILisReportElement element)
        {
            ReportGraphElement rge = element as ReportGraphElement;
            if (rge != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region graph项的内部处理逻辑
        protected virtual bool OperateGraph(ReportReportElement rre)
        {
            if (rre.ReportExam.SectionNo == 11)
            {
                List<ILisReportElement> graphList = rre.GetReportItem(typeof(ReportGraphElement).Name);
                AddImageByParItem(rre.ParItemList, graphList);
            }
            OperateElementList(rre.GetReportItem(typeof(ReportGraphElement).Name));
            return true;
        }
        #endregion

        #region 图片项添加处理
        private void AddImageByParItem(List<int> parItemList, List<ILisReportElement> graphElementList)
        {
            byte[] imageValue;
            ReportGraphElement rge;
            if (this.m_parItemNo2NormalImage.Count == 0)
            {
                this.InitParItem2NormalImage();
            }
            foreach (int parItemNo in parItemList)
            {
                imageValue = this.m_parItemNo2NormalImage[parItemNo] as byte[];
                if (imageValue != null)
                {
                    rge = new ReportGraphElement();
                    rge.GraphName = "normal";
                    rge.GraphImage = imageValue;
                    graphElementList.Add(rge);
                }
            }
        }
        private void InitParItem2NormalImage()
        {
            lock (this.m_parItemNo2NormalImage)
            {
                LisMap.InitParItem2NormalImageTable(this.m_parItemNo2NormalImage);
            }
        }
        #endregion
    }
}
