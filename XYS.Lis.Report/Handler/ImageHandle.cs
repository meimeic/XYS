using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;  

using XYS.Util;
using XYS.Report;
using XYS.Model.Lab;

using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report.Handler
{
    public class ImageHandle : HandleSkeleton
    {
        #region 私有字段
        private static readonly string ImageServer = "http://static.xys.com/img";
        private static readonly string UploadUri = "http://10.1.11.10:8090";
        #endregion

        #region 构造函数
        public ImageHandle(LabReportDAL dal)
            : base(dal)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool InnerHandle(IFillElement element, IReportKey RK)
        {
            List<IFillElement> elements = new List<IFillElement>(1);
            elements.Add(element);
            return InnerHandle(elements, RK);
        }

        protected override bool InnerHandle(List<IFillElement> elements, IReportKey RK)
        {
            LabPK key = RK as LabPK;
            if (key != null)
            {
                return UploadImages(elements, key);
            }
            return false;
        }
        #endregion

        #region 图片上传
        protected bool UploadImages(List<IFillElement> elements, LabPK RK)
        {
            ImageElement rie = null;
            WebClient wc = new WebClient();
            string boundary = GenderBoundary();
            string folder = RK.ReceiveDate.ToString("yyyyMMdd");
            //LOG.Info("生成上传图片数据");
            //获取上传数据
            byte[] postData = GenderPostData(elements, boundary);
            InitWebClient(wc, boundary, folder);
            try
            {
               //LOG.Info("开始图片上传,上传url为" + wc.BaseAddress + "/upload");
                byte[] resposeBytes = wc.UploadData("/upload", "POST", postData);
                string resposeStr = Encoding.UTF8.GetString(resposeBytes);
                List<WebImage> resList = JsonConvert.DeserializeObject<List<WebImage>>(resposeStr);

                elements.Clear();
                foreach (WebImage wi in resList)
                {
                    if (!string.IsNullOrEmpty(wi.Path))
                    {
                        rie = new ImageElement();
                        rie.ReportID = RK.ID;
                        rie.Name = wi.Name;
                        if (wi.Path[0] == '/')
                        {
                            rie.Url = ImageServer + wi.Path;
                        }
                        else
                        {
                            rie.Url = ImageServer + "/" + wi.Path;
                        }
                        elements.Add(rie);
                    }
                    else
                    {
                        //LOG.Error("图片服务器返回数据格式错误，上传图片集合失败");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //LOG.Error("上传图片集合出现异常！");
                return false;
            }
        }

        private string GenderGUIDName()
        {
            return SystemInfo.NewGuid().ToString();
        }
        //单张图片上传
        private byte[] GenderPostData(GraphElement graph, string boundary)
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
        private byte[] GenderPostData(List<IFillElement> elements, string boundary)
        {
            string fileName = null;
            GraphElement rge = null;
            MemoryStream stream = new MemoryStream();
            foreach (IFillElement element in elements)
            {
                rge = element as GraphElement;
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