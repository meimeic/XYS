using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Util;

namespace XYS.Lis.Handler
{
    public class ReportGraphHandler:ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
        private readonly Hashtable m_parItem2NormalImage;
        #endregion

        #region 构造函数
        public ReportGraphHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportGraphHandler(string handlerName)
            : base(handlerName)
        {
            this.m_parItem2NormalImage = new Hashtable(20);
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.Report)
            {
                ReportReportElement rre = element as ReportReportElement;
                return OperateGraphList(rre);
            }
            if (elementTag == ReportElementTag.GraphElement)
            {
                ReportGraphElement rge = element as ReportGraphElement;
                return OperateGraph(rge);
            }
            return true;
        }
        #endregion

        #region graph项的内部处理逻辑
        protected virtual bool OperateGraphList(ReportReportElement rre)
        {
            if (rre.SectionNo == 11)
            {
                List<ILisReportElement> graphList = rre.GetReportItem(ReportElementTag.GraphElement);
                AddImageByParItem(rre.ParItemList, graphList);
            }
            return true;
        }
        protected virtual bool OperateGraph(ReportGraphElement rge)
        {
            return true;
        }
        #endregion

        #region 图片项添加处理
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
