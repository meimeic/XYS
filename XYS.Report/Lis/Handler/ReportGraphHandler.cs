using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;  

using XYS.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportGraphHandler : ReportHandlerSkeleton
    {
        #region 字段
        private readonly Hashtable m_normalImageUri;
        private static readonly string m_baseURI = "http://10.1.11.10:8090";
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
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
                bool result = UploadImages(graphList, report.ReceiveDateTime.ToString("yyyyMMdd"), report.ReportImageMap);
                if (result)
                {
                    return new HandlerResult();
                }
                else
                {
                    return new HandlerResult(0, "upload images failed!");
                }
            }
            return new HandlerResult();
        }
        #endregion

        #region 图片上传
        protected bool UploadImages(List<AbstractSubFillElement> graphList, string folder, Dictionary<string, string> imageMap)
        {
            WebClient wc = new WebClient();

            //多张图片上传
            string boundary = GenderBoundary();
            byte[] postData = GenderPostData(graphList, boundary);
            InitWebClient(wc, boundary, folder);
            try
            {
                byte[] respose = wc.UploadData("/upload", "POST", postData);
                string res = Encoding.UTF8.GetString(respose);
                List<WebResult> resList = JsonConvert.DeserializeObject<List<WebResult>>(res);
                foreach(WebResult wr in resList)
                {
                    if (wr.code == 0)
                    {
                        imageMap.Add(wr.name, wr.imageUrl);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


            //单张图片上传
            //byte[] postData = null;
            //string boundary = null;
            //string imageName = null;
            //ReportGraphElement rge = null;
            //foreach (AbstractSubFillElement element in graphList)
            //{
            //    rge = element as ReportGraphElement;
            //    if (rge != null)
            //    {
            //        imageName = rge.GraphName;
            //        boundary = GenderBoundary();
            //        InitWebClient(wc, boundary, folder);
            //        postData = GenderPostData(rge, boundary);
            //        try
            //        {
            //            byte[] respose = wc.UploadData("/upload/lis", "POST", postData);
            //            string res = Encoding.UTF8.GetString(respose);
            //            string[] ress = res.Split(new char[] { ';' });
            //            if (ress.Length > 0)
            //            {
            //                if (ress[0].Equals("0"))
            //                {
            //                    imageMap.Add(imageName, ress[1]);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    }
            //}
        }
        private string GenderGUIDName()
        {
            return SystemInfo.NewGuid().ToString();
        }
        //单张图片上传
        private byte[] GenderPostData(ReportGraphElement graph, string boundary)
        {
            string fileName = GenderGUIDName();
            MemoryStream stream = new MemoryStream();

            WriteImageData(graph.GraphImage, boundary, "graph", fileName, stream);
            WritePostFooterData(boundary, stream);

            stream.Position = 0;
            byte[] postBuffer = new byte[stream.Length];
            stream.Read(postBuffer, 0, postBuffer.Length);
            stream.Close();

            return postBuffer;
        }
        //多张图片上传
        private byte[] GenderPostData(List<AbstractSubFillElement> graphList, string boundary)
        {
            string fileName = null;
            ReportGraphElement rge = null;
            MemoryStream stream = new MemoryStream();
            foreach (AbstractSubFillElement element in graphList)
            {
                rge = element as ReportGraphElement;
                if (rge != null)
                {
                    fileName = GenderGUIDName();
                    WriteImageData(rge.GraphImage, boundary, rge.GraphName, fileName, stream);
                }
            }
            WritePostFooterData(boundary, stream);
            //读取内容
            stream.Position = 0;
            byte[] postBuffer = new byte[stream.Length];
            stream.Read(postBuffer, 0, postBuffer.Length);
            stream.Close();
            return postBuffer;
        }
        private string GenderFormName(int i)
        {
            return "image" + i;
        }
        private string GenderBoundary()
        {
            return "--------------" + DateTime.Now.Ticks.ToString("x");
        }
        private string GenderPostHeader(string boundary, string formName, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append(SystemInfo.NewLine);
            sb.Append("Content-Disposition: form-data;");
            sb.Append("name=\"");
            sb.Append(formName);
            sb.Append("\";filename=\"");
            sb.Append(fileName);
            sb.Append("\"");
            sb.Append(SystemInfo.NewLine);
            sb.Append("Content-Type:image/jpeg");
            sb.Append(SystemInfo.NewLine);
            sb.Append(SystemInfo.NewLine);
            return sb.ToString();
        }
        private string GenderPostFooter(string boundary)
        {
            return "--" + boundary + "--";
        }
        private void WriteImageData(byte[] imageData, string boundary, string formName, string fileName, MemoryStream stream)
        {
            string postHeader = GenderPostHeader(boundary, formName, fileName);
            byte[] headerBytes = Encoding.UTF8.GetBytes(postHeader);
            stream.Write(headerBytes, 0, headerBytes.Length);
            stream.Write(imageData, 0, imageData.Length);
            //添加回车
            byte[] LFBytes = Encoding.UTF8.GetBytes(SystemInfo.NewLine);
            stream.Write(LFBytes, 0, LFBytes.Length);
        }
        private void WritePostFooterData(string boundary, MemoryStream stream)
        {
            string postFooter = GenderPostFooter(boundary);
            byte[] footerBytes = Encoding.UTF8.GetBytes(postFooter);
            stream.Write(footerBytes, 0, footerBytes.Length);
        }
        private void InitWebClient(WebClient wc, string boundary, string folder)
        {
            wc.BaseAddress = m_baseURI;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Set("Content-Type", "multipart/form-data;boundary=" + boundary);
            wc.Headers.Set("Accept-Language", "utf-8");
            wc.QueryString.Set("folder", folder);
        }
        #endregion

        #region 内部类
        class WebResult {
            WebResult()
            {
            }
            public int code { get; set; }
            public string name { get; set; }
            public string imageUrl { get; set; }
            public string message { get; set; }
        }
        #endregion
    }
}