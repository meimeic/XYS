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
    public class ReportGraphHandle : ReportHandleSkeleton
    {
        #region 私有字段
        private static readonly Hashtable NormalImageUri;
        private static readonly string ImageServer = "http://static.xys.com";
        private static readonly string UploadUri = "http://10.1.11.10:8090";
        #endregion

        #region 构造函数
        static ReportGraphHandle()
        {
            NormalImageUri = new Hashtable(20);
            ReportGraphHandle.InitNormalImageUriTable();
        }
        public ReportGraphHandle()
            : base()
        {
        }
        #endregion

        #region 实现父类抽象方法
        public override void ReportHandle(ReportReportElement report)
        {
            LOG.Info("报告图片项处理");
            List<IFillElement> graphList = report.GetReportItem(typeof(ReportGraphElement));
            if (IsExist(graphList))
            {
                LOG.Info("上传图片集合");
                string folderName = report.ReportInfo.ReceiveDateTime.ToString("yyyyMMdd");
                UploadImages(graphList, folderName, report.ReportImageMap, report.HandleResult);
                if (report.HandleResult.ResultCode < 0)
                {
                    this.OnhandleReportError(report);
                    return;
                }
                if (report.ReportInfo.SectionNo == 11)
                {
                    LOG.Info("尝试添加标准图片处理");
                    AddNormalImageBySuperItem(report.SuperItemList, report.ReportImageMap);
                }
            }
            this.OnHandleReportSuccess(report);
        }
        #endregion

        #region 图片上传
        protected void UploadImages(List<IFillElement> graphList, string folder, Dictionary<string, string> imageMap, HandleResult result)
        {
            WebClient wc = new WebClient();
            //多张图片上传
            string boundary = GenderBoundary();
            LOG.Info("生成上传图片数据");
            //获取上传数据字节数组
            byte[] postData = GenderPostData(graphList, boundary);
            InitWebClient(wc, boundary, folder);
            try
            {
                LOG.Info("开始上传,上传url为" + wc.BaseAddress + "/upload");
                byte[] resposeBytes = wc.UploadData("/upload", "POST", postData);
                string resposeStr = Encoding.UTF8.GetString(resposeBytes);
                List<WebImage> resList = JsonConvert.DeserializeObject<List<WebImage>>(resposeStr);
                foreach (WebImage wi in resList)
                {
                    if (!string.IsNullOrEmpty(wi.Path))
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
                    else
                    {
                        LOG.Error("图片服务器返回数据格式错误，上传图片集合失败！");
                        this.SetHandlerResult(result, -50, this.GetType(), new Exception("the image server have some unkown error,upload image(s) failed!"));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                LOG.Error("上传图片集合出错！", ex);
                this.SetHandlerResult(result, -51, this.GetType(), ex);
                return;
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
        private byte[] GenderPostData(List<IFillElement> graphList, string boundary)
        {
            string fileName = null;
            ReportGraphElement rge = null;
            MemoryStream stream = new MemoryStream();
            foreach (IFillElement element in graphList)
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
        //获取分割线
        private string GenderBoundary()
        {
            return "--------------" + DateTime.Now.Ticks.ToString("x");
        }
        //生成post数据首部分割字符串
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
        //生成post数据尾部分割字符串
        private string GenderPostFooter(string boundary)
        {
            return "--" + boundary + "--";
        }
        //将图片数据写入内存流
        private void WriteImageData(byte[] imageData, string boundary, string formName, string fileName, MemoryStream stream)
        {
            //获取表单头部字符串
            string postHeader = GenderPostHeader(boundary, formName, fileName);
            //表单头部字节
            byte[] headerBytes = Encoding.UTF8.GetBytes(postHeader);
            //写入内存流
            stream.Write(headerBytes, 0, headerBytes.Length);
            stream.Write(imageData, 0, imageData.Length);
            //添加回车字节
            byte[] LFBytes = Encoding.UTF8.GetBytes(SystemInfo.NewLine);
            //写入内存流
            stream.Write(LFBytes, 0, LFBytes.Length);
        }
        //将post数据的尾部分割线写入内存流
        private void WritePostFooterData(string boundary, MemoryStream stream)
        {
            string postFooter = GenderPostFooter(boundary);
            byte[] footerBytes = Encoding.UTF8.GetBytes(postFooter);
            stream.Write(footerBytes, 0, footerBytes.Length);
        }
        //初始化webclient
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
        private static void InitNormalImageUriTable()
        {
            lock (NormalImageUri)
            {
                NormalImageUri.Clear();
                NormalImageUri.Add(50006570, "/image/lis/report/normal/AML1-ETO.jpg");
                NormalImageUri.Add(90009363, "/image/lis/report/normal/ATM-CEP11.jpg");
                NormalImageUri.Add(90008499, "/image/lis/report/normal/BCL2.jpg");
                NormalImageUri.Add(90008738, "/image/lis/report/normal/BCL6.jpg");
                NormalImageUri.Add(50006569, "/image/lis/report/normal/BCR-ABL.jpg");
                NormalImageUri.Add(90009026, "/image/lis/report/normal/BCR-ABL-ASS1.jpg");
                NormalImageUri.Add(50006576, "/image/lis/report/normal/CBFB.jpg");
                NormalImageUri.Add(50006583, "/image/lis/report/normal/CCND1-IgH.jpg");
                NormalImageUri.Add(90009367, "/image/lis/report/normal/CDKN2A.jpg");
                NormalImageUri.Add(90008735, "/image/lis/report/normal/CEP12.jpg");
                NormalImageUri.Add(90008495, "/image/lis/report/normal/D7S486-CEP7.jpg");
                NormalImageUri.Add(50006573, "/image/lis/report/normal/CEPX-CEPY.jpg");
                NormalImageUri.Add(90008729, "/image/lis/report/normal/CKS1B-CDKN2C.jpg");
                NormalImageUri.Add(90009373, "/image/lis/report/normal/CRLF2.jpg");
                NormalImageUri.Add(90009370, "/image/lis/report/normal/D13S319.jpg");
                NormalImageUri.Add(90008497, "/image/lis/report/normal/D20S108.jpg");
                NormalImageUri.Add(90008494, "/image/lis/report/normal/EGR1-D5S721.jpg");
                NormalImageUri.Add(90008741, "/image/lis/report/normal/EVI1.jpg");
                NormalImageUri.Add(90008730, "/image/lis/report/normal/FGFR1-D8Z2.jpg");
                NormalImageUri.Add(50006574, "/image/lis/report/normal/IgH.jpg");
                NormalImageUri.Add(90009041, "/image/lis/report/normal/IGH-BCL2.jpg");
                NormalImageUri.Add(50006582, "/image/lis/report/normal/FGFR3-IgH.jpg");
                NormalImageUri.Add(50006581, "/image/lis/report/normal/MAF-IgH.jpg");
                NormalImageUri.Add(90009364, "/image/lis/report/normal/IGH-MAFB.jpg");
                NormalImageUri.Add(90009038, "/image/lis/report/normal/IGH-MYC.jpg");
                NormalImageUri.Add(50006575, "/image/lis/report/normal/MLL.jpg");
                NormalImageUri.Add(50006579, "/image/lis/report/normal/MYC.jpg");
                NormalImageUri.Add(90008744, "/image/lis/report/normal/MYC.jpg");
                NormalImageUri.Add(90009376, "/image/lis/report/normal/P2RY8.jpg");
                NormalImageUri.Add(90009362, "/image/lis/report/normal/P53-CEP17.jpg");
                NormalImageUri.Add(90009032, "/image/lis/report/normal/PDGFRA.jpg");
                NormalImageUri.Add(90009035, "/image/lis/report/normal/PDGFRB.jpg");
                NormalImageUri.Add(50006571, "/image/lis/report/normal/PML-RARA.jpg");
                NormalImageUri.Add(90009029, "/image/lis/report/normal/RARA.jpg");
                NormalImageUri.Add(50006578, "/image/lis/report/normal/RB-1.jpg");
                NormalImageUri.Add(90008496, "/image/lis/report/normal/TCF3-PBX1.jpg");
                NormalImageUri.Add(50006572, "/image/lis/report/normal/TEL-AML1.jpg");
                NormalImageUri.Add(50006577, "/image/lis/report/normal/SANTI8.jpg");
            }
        }
        private void AddNormalImageBySuperItem(List<int> superItem, Dictionary<string, string> imageMap)
        {
            foreach (int no in superItem)
            {
                string path = NormalImageUri[no] as string;
                if (path != null)
                {
                    imageMap["normal"] = ImageServer + path;
                    return;
                }
            }
        }
        #endregion

        #region WebImage内部类
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