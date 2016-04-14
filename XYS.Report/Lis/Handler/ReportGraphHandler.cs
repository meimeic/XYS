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
        #region 私有字段
        private readonly Hashtable m_normalImageUri;
        private static readonly string  ImageServer="http://static.xys.com";
        private static readonly string UploadUri = "http://10.1.11.10:8090";
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
            this.InitNormalImageUriTable();
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
                    if (report.SectionNo == 11)
                    {
                        AddNormalImageBySuperItem(report.SuperItemList, report.ReportImageMap);
                    }
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
                List<WebImage> resList = JsonConvert.DeserializeObject<List<WebImage>>(res);
                foreach (WebImage wi in resList)
                {
                    if (string.IsNullOrEmpty(wi.Path))
                    {
                        if (wi.Path[0] == '/')
                        {
                            imageMap.Add(wi.Name, ImageServer + wi.Path);
                        }
                        else
                        {
                            imageMap.Add(wi.Name, ImageServer + "/" + wi.Path);
                        }
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
            wc.BaseAddress = UploadUri;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Set("Content-Type", "multipart/form-data;boundary=" + boundary);
            wc.Headers.Set("Accept-Language", "utf-8");
            wc.QueryString.Set("folder", folder);
        }
        #endregion

        #region 私有方法
        private void InitNormalImageUriTable()
        {
            this.m_normalImageUri.Clear();
            this.m_normalImageUri.Add(50006570, "/image/lis/report/normal/AML1-ETO.jpg");
            this.m_normalImageUri.Add(90009363, "/image/lis/report/normal/ATM-CEP11.jpg");
            this.m_normalImageUri.Add(90008499, "/image/lis/report/normal/BCL2.jpg");
            this.m_normalImageUri.Add(90008738, "/image/lis/report/normal/BCL6.jpg");
            this.m_normalImageUri.Add(50006569, "/image/lis/report/normal/BCR-ABL.jpg");
            this.m_normalImageUri.Add(90009026, "/image/lis/report/normal/BCR-ABL-ASS1.jpg");
            this.m_normalImageUri.Add(50006576, "/image/lis/report/normal/CBFB.jpg");
            this.m_normalImageUri.Add(50006583, "/image/lis/report/normal/CCND1-IgH.jpg");
            this.m_normalImageUri.Add(90009367, "/image/lis/report/normal/CDKN2A.jpg");
            this.m_normalImageUri.Add(90008735, "/image/lis/report/normal/CEP12.jpg");
            this.m_normalImageUri.Add(90008495, "/image/lis/report/normal/D7S486-CEP7.jpg");
            this.m_normalImageUri.Add(50006573, "/image/lis/report/normal/CEPX-CEPY.jpg");
            this.m_normalImageUri.Add(90008729, "/image/lis/report/normal/CKS1B-CDKN2C.jpg");
            this.m_normalImageUri.Add(90009373, "/image/lis/report/normal/CRLF2.jpg");
            this.m_normalImageUri.Add(90009370, "/image/lis/report/normal/D13S319.jpg");
            this.m_normalImageUri.Add(90008497, "/image/lis/report/normal/D20S108.jpg");
            this.m_normalImageUri.Add(90008494, "/image/lis/report/normal/EGR1-D5S721.jpg");
            this.m_normalImageUri.Add(90008741, "/image/lis/report/normal/EVI1.jpg");
            this.m_normalImageUri.Add(90008730, "/image/lis/report/normal/FGFR1-D8Z2.jpg");
            this.m_normalImageUri.Add(50006574, "/image/lis/report/normal/IgH.jpg");
            this.m_normalImageUri.Add(90009041, "/image/lis/report/normal/IGH-BCL2.jpg");
            this.m_normalImageUri.Add(50006582, "/image/lis/report/normal/FGFR3-IgH.jpg");
            this.m_normalImageUri.Add(50006581, "/image/lis/report/normal/MAF-IgH.jpg");
            this.m_normalImageUri.Add(90009364, "/image/lis/report/normal/IGH-MAFB.jpg");
            this.m_normalImageUri.Add(90009038, "/image/lis/report/normal/IGH-MYC.jpg");
            this.m_normalImageUri.Add(50006575, "/image/lis/report/normal/MLL.jpg");
            this.m_normalImageUri.Add(50006579, "/image/lis/report/normal/MYC.jpg");
            this.m_normalImageUri.Add(90008744, "/image/lis/report/normal/MYC.jpg");
            this.m_normalImageUri.Add(90009376, "/image/lis/report/normal/P2RY8.jpg");
            this.m_normalImageUri.Add(90009362, "/image/lis/report/normal/P53-CEP17.jpg");
            this.m_normalImageUri.Add(90009032, "/image/lis/report/normal/PDGFRA.jpg");
            this.m_normalImageUri.Add(90009035, "/image/lis/report/normal/PDGFRB.jpg");
            this.m_normalImageUri.Add(50006571, "/image/lis/report/normal/PML-RARA.jpg");
            this.m_normalImageUri.Add(90009029, "/image/lis/report/normal/RARA.jpg");
            this.m_normalImageUri.Add(50006578, "/image/lis/report/normal/RB-1.jpg");
            this.m_normalImageUri.Add(90008496, "/image/lis/report/normal/TCF3-PBX1.jpg");
            this.m_normalImageUri.Add(50006572, "/image/lis/report/normal/TEL-AML1.jpg");
            this.m_normalImageUri.Add(50006577, "/image/lis/report/normal/SANTI8.jpg");
        }
        private void AddNormalImageBySuperItem(List<int> superItem,Dictionary<string,string> imageMap)
        {
            foreach (int su in superItem)
            {
                string path = this.m_normalImageUri[su] as string;
                if (path != null)
                {
                    imageMap["normal"] = ImageServer + path;
                    return;
                }
            }
        }
        #endregion

        #region 内部类
        class WebImage
        {
            WebImage()
            {
            }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Message { get; set; }
        }
        #endregion
    }
}