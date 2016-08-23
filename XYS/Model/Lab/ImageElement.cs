using System;
using System.Net;

using XYS.Report;
using XYS.Common;
namespace XYS.Model.Lab
{
    [Serializable]
    public class ImageElement : IFillElement
    {
        #region 私有字段
        private string m_reportID;
        private string m_name;
        private string m_url;
        private byte[] m_value;
        #endregion

        #region 构造函数
        public ImageElement()
        { }
        #endregion

        #region 实例属性
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
        [Column()]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public string Url
        {
            get { return this.m_url; }
            set
            {
                this.m_url = value;
                if (this.m_value == null)
                {
                    this.m_value = LoadURL(value);
                }
            }
        }
        [Column()]
        public byte[] Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }
        #endregion

        #region 静态私有方法
        private static byte[] LoadURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadData(url);
                }
            }
            return null;
        }
        #endregion
    }
}
