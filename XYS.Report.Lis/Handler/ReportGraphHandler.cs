using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Report.Lis.Util;
using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportGraphHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
        private readonly Hashtable m_parItemNo2NormalImage;
        private readonly Hashtable m_graphName2PropertyName;
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
            this.m_graphName2PropertyName = new Hashtable(10);
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(ILisReportElement element)
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
            List<ILisReportElement> graphList = report.GetReportItem(typeof(ReportGraphElement).Name);
            if (IsExist(graphList))
            {
                report.CreateImageCollection();
                Convert2ImageCollection(graphList, report.ImageCollection);
            }
            report.RemoveReportItem(typeof(ReportGraphElement).Name);
            if (report.SectionNo == 11)
            {
                AddNormalGraph(report.ParItemList, report.ImageCollection);
            }
            return true;
        }
        #endregion

        #region graph项的内部处理逻辑
        protected void Convert2ImageCollection(List<ILisReportElement> graphList, ReportImagesElement imageCollection)
        {
            if (imageCollection == null)
            {
                throw new ArgumentNullException("ImageCollection");
            }
            string propertyName = null;
            PropertyInfo imageProperty = null;
            Type type = imageCollection.GetType();
            ReportGraphElement rge = null;
            foreach (ILisReportElement element in graphList)
            {
                rge = element as ReportGraphElement;
                if (rge != null)
                {
                    propertyName = GetPropertyName(rge.GraphName);
                    if (propertyName != null)
                    {
                        imageProperty = type.GetProperty(propertyName);
                        if (imageProperty != null)
                        {
                            SetImageProperty(imageProperty, rge.GraphImage, imageCollection);
                        }
                    }
                }
            }
        }
        private string GetPropertyName(string graphName)
        {
            if (this.m_graphName2PropertyName.Count == 0)
            {
                InitgraphName2PropertyName();
            }
            if (graphName != null)
            {
                return this.m_graphName2PropertyName[graphName] as string;
            }
            return null;
        }
        private void SetImageProperty(PropertyInfo pro, object value, ReportImagesElement imageCollection)
        {
            try
            {
                pro.SetValue(imageCollection, value, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 图片项添加处理
        private void AddNormalGraph(List<int> parItemList, ReportImagesElement imageCollection)
        {
            if (imageCollection == null)
            {
                throw new ArgumentNullException("ImageCollection");
            }
            Type type = imageCollection.GetType();
            string propertyName = GetPropertyName("normal");
            if (parItemList.Count > 0 && propertyName != null)
            {
                PropertyInfo imageProperty = type.GetProperty(propertyName);
                if (imageProperty != null)
                {
                    object imageValue = null;
                    foreach (int parItemNo in parItemList)
                    {
                        imageValue = this.m_parItemNo2NormalImage[parItemNo];
                        if (imageValue != null)
                        {
                            SetImageProperty(imageProperty, imageValue, imageCollection);
                            break;
                        }
                    }
                }
            }
        }
        private void InitParItem2NormalImage()
        {
            lock (this.m_parItemNo2NormalImage)
            {
                ConfigManager.InitParItem2NormalImageTable(this.m_parItemNo2NormalImage);
            }
        }
        private void InitgraphName2PropertyName()
        {
            lock (this.m_graphName2PropertyName)
            {

            }
        }
        #endregion
    }
}
