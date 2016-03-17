using System;
using System.Net;
using System.Text;
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
        private static readonly string BasicUri = "10.1.10.245:8090";
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
            List<ILisReportElement> graphList = report.GetReportItem(typeof(ReportGraphElement));
            if (IsExist(graphList))
            {
                string imagePrefix = GenderImagePrefix(report.ReportExam.SectionNo, report.ReportExam.TestTypeNo, report.ReportExam.SampleNo);
                UploadImages(graphList, report.ReceiveDateTime.ToString("yyyyMMdd"), imagePrefix);
            }
            report.RemoveReportItem(typeof(ReportGraphElement));
            return true;
        }
        #endregion

        #region graph项的内部处理逻辑
        //protected void Convert2ImageCollection(List<ILisReportElement> graphList, AbstractInnerElement imageCollection)
        //{
        //    if (imageCollection == null)
        //    {
        //        throw new ArgumentNullException("ImageCollection");
        //    }
        //    string propertyName = null;
        //    PropertyInfo imageProperty = null;
        //    Type type = imageCollection.GetType();
        //    ReportGraphElement rge = null;
        //    foreach (ILisReportElement element in graphList)
        //    {
        //        rge = element as ReportGraphElement;
        //        if (rge != null)
        //        {
        //            propertyName = GetPropertyName(rge.GraphName);
        //            if (propertyName != null)
        //            {
        //                imageProperty = type.GetProperty(propertyName);
        //                if (imageProperty != null)
        //                {
        //                    SetImageProperty(imageProperty, rge.GraphImage, imageCollection);
        //                }
        //            }
        //        }
        //    }
        //}
        //private string GetPropertyName(string graphName)
        //{
        //    if (this.m_graphName2PropertyName.Count == 0)
        //    {
        //        InitgraphName2PropertyName();
        //    }
        //    if (graphName != null)
        //    {
        //        return this.m_graphName2PropertyName[graphName] as string;
        //    }
        //    return null;
        //}
        //private void SetImageProperty(PropertyInfo pro, object value, AbstractInnerElement imageCollection)
        //{
        //    try
        //    {
        //        pro.SetValue(imageCollection, value, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region 图片项添加处理
        //private void AddNormalGraph(List<int> parItemList, AbstractInnerElement imageCollection)
        //{
        //    if (imageCollection == null)
        //    {
        //        throw new ArgumentNullException("ImageCollection");
        //    }
        //    Type type = imageCollection.GetType();
        //    string propertyName = GetPropertyName("normal");
        //    if (parItemList.Count > 0 && propertyName != null)
        //    {
        //        PropertyInfo imageProperty = type.GetProperty(propertyName);
        //        if (imageProperty != null)
        //        {
        //            object imageValue = null;
        //            foreach (int parItemNo in parItemList)
        //            {
        //                imageValue = this.m_parItemNo2NormalImage[parItemNo];
        //                if (imageValue != null)
        //                {
        //                    SetImageProperty(imageProperty, imageValue, imageCollection);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //private void InitParItem2NormalImage()
        //{
        //    lock (this.m_parItemNo2NormalImage)
        //    {
        //        ConfigManager.InitParItem2NormalImageTable(this.m_parItemNo2NormalImage);
        //    }
        //}
        //private void InitgraphName2PropertyName()
        //{
        //    lock (this.m_graphName2PropertyName)
        //    {

        //    }
        //}
        #endregion

        #region
        protected void UploadImages(List<ILisReportElement> graphList, string receiveDate, string imagePrefix)
        {
            WebClient wc = null;
            string imageName = null;
            ReportGraphElement rge = null;
            foreach (ILisReportElement element in graphList)
            {
                rge = element as ReportGraphElement;
                if (rge != null)
                {
                    wc = new WebClient();
                    imageName = GenderImageName(imagePrefix, rge.GraphName);
                    InitWebClient(wc, receiveDate, imageName);
                    wc.UploadDataAsync(new Uri("/lis"), "POST", rge.GraphImage);
                }
            }
        }
        private string GenderImagePrefix(int sectionNo, int testTypeNo, string sampleNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sectionNo);
            sb.Append('_');
            sb.Append(testTypeNo);
            sb.Append('_');
            sb.Append(sampleNo);
            sb.Append('_');
            return sb.ToString();
        }
        private string GenderImageName(string imagePrefix, string Name)
        {
            return imagePrefix+Name+".jpg";
        }
        private void InitWebClient(WebClient wc, string receiveDate, string imageName)
        {
            wc.BaseAddress = BasicUri;
            wc.Encoding = Encoding.UTF8;

            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            wc.QueryString.Add("receivedate", receiveDate);
            wc.QueryString.Add("imagename", imageName);

            wc.UploadDataCompleted += new UploadDataCompletedEventHandler(Pic_UploadDataCompleted);
        }
        private void Pic_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string returnMessage = Encoding.Default.GetString(e.Result);
            }
        }
        #endregion

        #region
        private bool IsExist(Dictionary<string, string> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
