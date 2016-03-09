using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Report.Lis.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportGraphHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
        private readonly Dictionary<int,byte[]> m_parItemNo2NormalImage;
        private readonly Dictionary<string,string> m_graphName2PropertyName;
        #endregion

        #region 构造函数
        public ReportGraphHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportGraphHandler(string handlerName)
            : base(handlerName)
        {
            this.m_parItemNo2NormalImage = new Dictionary<int, byte[]>(20);
            this.m_graphName2PropertyName = new Dictionary<string, string>(10);
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
            List<IReportElement> graphList = rre.GetReportItem(typeof(ReportGraphElement).Name);
            if (IsExist(graphList))
            {
                rre.CreateImageCollection();
                Convert2ImageCollection(graphList, rre.ImageCollection);
            }
            rre.RemoveReportItem(typeof(ReportGraphElement).Name);

            switch (rre.SectionNo)
            {
                case 11:
                    AddImageByParItem(rre);
                    break;
                default:
                    break;
            }
            OperateElementList(rre.GetReportItem(typeof(ReportGraphElement).Name));
            Convert2ImageCollection(rre);
            return true;
        }
        #endregion

        #region
        protected void Convert2ImageCollection(List<IReportElement> graphList,ReportImagesElement imageCollection)
        {
            if (imageCollection == null)
            {
                throw new ArgumentNullException("ImageCollection");
            }
            string propertyName = null;
            ReportGraphElement rge = null;
            foreach (IReportElement element in graphList)
            {
                rge = element as ReportGraphElement;
                if (rge != null)
                {
 
                }
            }
        }
        private string GetPropertyName(string graphName)
        {
            if (this.m_graphName2PropertyName.ContainsKey(graphName))
            {
                return this.m_graphName2PropertyName[graphName];
            }
            return null;
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
                if (this.m_parItemNo2NormalImage.ContainsKey(parItemNo))
                {
                    imageValue = this.m_parItemNo2NormalImage[parItemNo];
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
                ConfigManager.InitParItem2NormalImageDictionary(this.m_parItemNo2NormalImage);
            }
        }
        #endregion
    }
}
