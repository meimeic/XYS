using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportGraphHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_baseURI = "http://10.1.10.245:8090";
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
        private readonly Hashtable m_normalImageUri;
        #endregion

        #region 构造函数
        public ReportGraphHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportGraphHandler(string handlerName)
            : base(handlerName)
        {
            this.m_normalImageUri = new Hashtable(20);
        }
        #endregion

        #region 实现父类抽象方法
        protected override HandlerResult OperateReport(ReportReportElement report)
        {
            List<AbstractSubFillElement> graphList = report.GetReportItem(typeof(ReportGraphElement));
            if (IsExist(graphList))
            {
                Dictionary<string, string> imageMap = new Dictionary<string, string>(4);
                bool result = UploadImages(graphList, report.ReceiveDateTime.ToString("yyyyMMdd"), imageMap);
                if (result)
                {
                    return new HandlerResult();
                }
                else
                {
                    return new HandlerResult(0, "upload image failed!");
                }
            }
            return new HandlerResult();
        }
        #endregion

        #region graph项的内部处理逻辑
        #endregion

        #region 图片项添加处理
        #endregion

        #region 图片上传
        protected bool UploadImages(List<AbstractSubFillElement> graphList, string folderName, Dictionary<string, string> imageMap)
        {
            WebClient wc = null;
            string boundary = GenderBoundary();
            byte[] postData = GenderPostData(graphList, boundary);
            InitWebClient(wc, boundary, postData.Length);
            try
            {
                byte[] respose = wc.UploadData("/upload/lis", "POST", postData);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string GenderGUIDName()
        {
            return SystemInfo.NewGuid().ToString() + ".jpg";
        }
        private byte[] GenderPostData(List<AbstractSubFillElement> graphList, string boundary)
        {
            int imageSeque = 1;
            string fileName = null;
            string formName = null;
            ReportGraphElement rge = null;
            MemoryStream stream = new MemoryStream();
            foreach (AbstractSubFillElement element in graphList)
            {
                rge = element as ReportGraphElement;
                if (rge != null)
                {
                    fileName = GenderGUIDName();
                    formName = GenderFormName(imageSeque);
                    WriteImageData(rge.GraphImage, boundary, formName, fileName, stream);
                    imageSeque++;
                }
            }
            WritePostFooterData(boundary, stream);
            //读取内容
            stream.Position = 0;
            byte[] postBuffer = new byte[stream.Length];
            stream.Read(postBuffer, 0, postBuffer.Length);
            stream.Close();
            //
            return postBuffer;
        }
        private string GenderFormName(int i)
        {
            return "image" + i;
        }
        private string GenderPostHeader(string boundary, string formName, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SystemInfo.NewLine);
            sb.Append("Content-Disposition: form-data;");
            sb.Append("name=\"");
            sb.Append(formName);
            sb.Append("\";filename=\"");
            sb.Append(fileName);
            sb.Append("\"");
            sb.Append(SystemInfo.NewLine);
            sb.Append("Content-Type:image/jpg");
            sb.Append(SystemInfo.NewLine);
            sb.Append(SystemInfo.NewLine);
            return sb.ToString();
        }
        private string GenderPostFooter(string boundary)
        {
            return boundary + "--";
        }
        private void WriteImageData(byte[] imageData, string boundary, string formName, string fileName, MemoryStream stream)
        {
            string postHeader = GenderPostHeader(boundary, formName, fileName);
            byte[] headerBytes = Encoding.UTF8.GetBytes(postHeader);
            stream.Write(headerBytes, 0, headerBytes.Length);
            stream.Write(imageData, 0, imageData.Length);
        }
        private void WritePostFooterData(string boundary, MemoryStream stream)
        {
            string postFooter = GenderPostFooter(boundary);
            byte[] footerBytes = Encoding.UTF8.GetBytes(postFooter);
            stream.Write(footerBytes, 0, footerBytes.Length);
        }
        private void InitWebClient(WebClient wc, string boundary, int length)
        {
            wc.BaseAddress = m_baseURI;

            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Length", length.ToString());
            wc.Headers.Add("Content-Type", "multipart/form-data;boundary=" + boundary);
            wc.Headers.Add("Accept-Language", "utf-8");
        }
        private void Pic_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string returnMessage = Encoding.Default.GetString(e.Result);
            }
        }
        private string GenderBoundary()
        {
            return "----------------" + DateTime.Now.Ticks.ToString("x");
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
        private Dictionary<string, string> GetImageMap(ReportReportElement report)
        {
            Dictionary<string, string> imageMap = report.ReportImageMap;
            if (imageMap == null)
            {
                imageMap = new Dictionary<string, string>(2);
                report.ReportImageMap = imageMap;
            }
            return imageMap;
        }
        #endregion
    }
}
