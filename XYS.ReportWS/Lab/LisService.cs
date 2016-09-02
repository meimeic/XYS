using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

using log4net;

using XYS.Model;
using XYS.Report;
using XYS.Lis.Report;
namespace XYS.ReportWS.Lab
{
    public class LisService
    {
        #region 静态只读字段
        private static readonly LisService ServiceInstance;
        private static readonly ILog LOG;
        #endregion

        #region 实例只读字段
        private readonly LabService LabService;
        private readonly XmlSerializer Serializer;
        private readonly FRService.LabPDFSoapClient FRClient;
        private readonly MongoService.LabMongoSoapClient MongoClient;
        #endregion

        #region 构造函数
        static LisService()
        {
            LOG = LogManager.GetLogger("ReportWS");
            ServiceInstance = new LisService();
        }
        private LisService()
        {
            this.LabService = LabService.LService;
            this.LabService.HandleCompleteEvent += this.PrintAndSave;

            this.Serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.FRClient = new FRService.LabPDFSoapClient("LabPDFSoap");
            this.MongoClient = new MongoService.LabMongoSoapClient("LabMongoSoap");
        }
        #endregion

        #region 静态属性
        public static LisService LService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 生产者方法
        public void Handle(string applyInfo)
        {
            LOG.Info("获取的xml报文为:" + applyInfo);
            if (!string.IsNullOrEmpty(applyInfo))
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    LOG.Info("处理xml报文成功");
                    this.GetApplyInfo(info);
                }
                catch (Exception ex)
                {
                    LOG.Error("处理xml报文异常:" + ex.Message);
                }
            }
            else
            {
                LOG.Warn("报文为空");
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
                        LOG.Info("申请单号为" + item.ApplyNo + "的报告加入处理队列");
                        this.HandleReport(item.ApplyNo);
                    }
                }
            }
        }
        private void HandleReport(string SerialNo)
        {
            LOG.Info("根据申请单处理报告,申请号为:" + SerialNo);
            string where = " where serialno='" + SerialNo + "'";
            LabService.InitReport(where);
        }
        #endregion

        #region 私有方法
        private void PrintAndSave(LabReport report)
        {
            LOG.Info("获取报告成功，报告ID为:" + report.Info.ReportID + ",将报告发送到打印服务");
            byte[] re = TransHelper.SerializeObject(report);
            this.FRClient.PrintPDF(re);

            LOG.Info("获取报告成功，报告ID为:" + report.Info.ReportID + ",将报告发送到保存服务");
            this.MongoClient.SaveToMongo(re);
        }
        #endregion
    }
}