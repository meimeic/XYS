using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

using XYS.Lis.Report;
namespace XYS.Report.WS
{
    public class LisService
    {
        #region 静态只读字段
        private static readonly LisService ServiceInstance;
        #endregion

        #region 实例只读字段
        private readonly XmlSerializer Serializer;
        private readonly LabService LabService;
        #endregion

        #region 实例字段
        private object logLock;
        #endregion

        #region 构造函数
        static LisService()
        {
            ServiceInstance = new LisService();
        }
        private LisService()
        {
            this.logLock = new object();
            this.Serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.LabService = LabService.LService;
        }
        #endregion

        #region 静态属性
        public static LisService RService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 生产者方法
        public void Deserialize(string applyInfo)
        {
            WriteLog(applyInfo);
            if (!string.IsNullOrEmpty(applyInfo))
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    WriteLog("处理xml报文成功");
                    this.GetApplyInfo(info);
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog("处理xml报文异常");
                }
            }
            else
            {
                WriteLog("报文为空");
            }
        }
        protected void GetApplyInfo(LabApplyInfo applyInfo)
        {
            List<ApplyItem> applyCollection = applyInfo.ApplyCollection;
            if (applyCollection != null && applyCollection.Count > 0)
            {
                foreach (ApplyItem item in applyCollection)
                {
                    //获取审核的报告单主键
                    if (item.ApplyStatus == 7)
                    {
                        WriteLog("申请单号为" + item.ApplyNo + "的报告加入存储队列");
                        this.GetReport(item.ApplyNo);
                    }
                }
            }
        }
        private void GetReport(string SerialNo)
        {
            string where = "serialno='" + SerialNo + "'";
            this.LabService.InitReport(where);
        }
        #endregion

        #region
        protected void WriteLog(string message)
        {
            lock (this.logLock)
            {
                var file = System.IO.File.AppendText("D:\\log\\log.txt");
                file.WriteLine(message);
                file.Close();
            }
        }
        #endregion
    }
}