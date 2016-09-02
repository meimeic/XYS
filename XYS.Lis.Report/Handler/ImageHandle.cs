using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Configuration;
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
        private static readonly string UploadUri;
        private static readonly string ImageLocal;
        private static readonly string ImageServer;
        #endregion

        #region 构造函数
        static ImageHandle()
        {
            ImageServer = ConfigurationManager.AppSettings["LabImageServer"].ToString();
            ImageLocal = ConfigurationManager.AppSettings["LabImageLocalDir"].ToString();
        }
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
            if (IsExist(elements))
            {
                LOG.Info("保存报告图片到本地");
                return SaveLocalImages(elements, RK);
            }
            return true;
        }
        #endregion

        #region 图片本地保存
        protected bool SaveLocalImages(List<IFillElement> elements, IReportKey RK)
        {
            ImageElement ie = null;
            string imgFileName = null;
            string midPath = DateTime.Now.ToString("yyyyMMdd");
            string imagePath = this.ImagePath(midPath);
            foreach (IFillElement element in elements)
            {
                ie = element as ImageElement;
                if (ie != null)
                {
                    ie.ReportID = RK.ID;
                    imgFileName = SaveImage(imagePath, ie.Value);
                    ie.Url = ImageServer + "/" + midPath + "/" + imgFileName;
                }
            }
            return true;
        }
        protected string SaveImage(string imagePath, byte[] bytes)
        {
            string imageName = this.ImageName();
            string imageFullName = Path.Combine(imagePath, imageName);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image image = Image.FromStream(ms);
                image.Save(imageFullName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return imageName;
        }
        private string ImageName()
        {
            return GenderGUIDName() + ".jpg";
        }
        private string ImagePath(string midPath)
        {
            string path = Path.Combine(ImageLocal, midPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        #endregion

        #region 图片上传
        protected bool UploadImages(List<IFillElement> elements, IReportKey RK)
        {
            ImageElement ie = null;
            string boundary = GenderBoundary();
            string folder = DateTime.Now.ToString("yyyyMMdd");
            //LOG.Info("生成上传图片数据");
            //获取上传数据
            byte[] postData = GenderPostData(elements, boundary);
            using (WebClient wc = new WebClient())
            {
                InitWebClient(wc, boundary, folder);
                try
                {
                    //LOG.Info("开始图片上传,上传url为" + wc.BaseAddress + "/upload");
                    byte[] resposeBytes = wc.UploadData("/upload", "POST", postData);
                    string resposeStr = Encoding.UTF8.GetString(resposeBytes);
                    List<WebImage> resList = JsonConvert.DeserializeObject<List<WebImage>>(resposeStr);

                    foreach (WebImage wi in resList)
                    {
                        if (!string.IsNullOrEmpty(wi.Path))
                        {
                            ie = GetImageByName(wi.Name, elements);
                            if (ie != null)
                            {
                                ie.ReportID = RK.ID;
                                if (wi.Path[0] == '/')
                                {
                                    ie.Url = ImageServer + wi.Path;
                                }
                                else
                                {
                                    ie.Url = ImageServer + "/" + wi.Path;
                                }
                            }
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
        }
        private string GenderGUIDName()
        {
            return SystemInfo.NewGuid().ToString();
        }
        //单张图片上传
        private byte[] GenderPostData(ImageElement img, string boundary)
        {
            string fileName = GenderGUIDName();
            MemoryStream stream = new MemoryStream();

            WriteImageData(img.Value, boundary, "graph", fileName, stream);
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
            ImageElement ie = null;
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (IFillElement element in elements)
                {
                    ie = element as ImageElement;
                    if (ie != null)
                    {
                        fileName = GenderGUIDName();
                        WriteImageData(ie.Value, boundary, ie.Name, fileName, stream);
                    }
                }
                WritePostFooterData(boundary, stream);
                //读取内容
                stream.Position = 0;
                byte[] postBuffer = new byte[stream.Length];
                stream.Read(postBuffer, 0, postBuffer.Length);
                return postBuffer;
            }
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

        private ImageElement GetImageByName(string name, List<IFillElement> elements)
        {
            ImageElement result = null;
            foreach (IFillElement element in elements)
            {
                result = element as ImageElement;
                if (result != null)
                {
                    if (result.Name.Equals(name))
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        #endregion

        #region WebImage内部类 图片保存结果
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