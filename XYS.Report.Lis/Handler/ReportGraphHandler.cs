using System;
using System.Reflection;
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
            if (rre.SectionNo == 11)
            {
                AddImageByParItem(rre.ParItemList, rre.ImageCollection);
            }
            return true;
        }
        #endregion

        #region
        protected void Convert2ImageCollection(List<IReportElement> graphList, ReportImagesElement imageCollection)
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
                    propertyName = GetPropertyName(rge.GraphName);
                    if (propertyName != null)
                    {
                        SetImageProperty(propertyName, rge.GraphImage, imageCollection);
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
        private void SetImageProperty(string propertyName, object value,ReportImagesElement imageCollection)
        {
            try
            {
                PropertyInfo pro = imageCollection.GetType().GetProperty(propertyName);
                if (pro != null)
                {
                    pro.SetValue(imageCollection, value, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 图片项添加处理
        private void AddImageByParItem(List<int> parItemList,ReportImagesElement imageCollection)
        {
            if (imageCollection == null)
            {
                throw new ArgumentNullException("ImageCollection");
            }
            string propertyName = GetPropertyName("normal");
            if (parItemList.Count > 0&&propertyName!=null)
            {
                object imageValue=null;
                if (this.m_parItemNo2NormalImage.Count == 0)
                {
                    this.InitParItem2NormalImage();
                }
                foreach (int parItemNo in parItemList)
                {
                    imageValue = this.m_parItemNo2NormalImage[parItemNo];
                    if (imageValue != null)
                    {
                        SetImageProperty(propertyName, imageValue, imageCollection);
                        break;
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
                //
            }
        }
        #endregion
    }
}
