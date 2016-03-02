using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Util;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Export.Model;
namespace XYS.Lis.Export
{
    public class PDFExport:ReportExportSkeleton
    {
        private readonly static string m_defaultExportName = "PDFExport";
        
        private readonly Hashtable m_graph2ImageTable;
        private readonly Hashtable m_section2Order;
        private readonly Hashtable m_parItem2Order;

        private readonly Hashtable m_parItem2PrintModel;
        private readonly Hashtable m_section2PrintModel;

        public PDFExport()
            : this(m_defaultExportName)
        { }
        public PDFExport(string name)
            : base(m_defaultExportName)
        {
            this.m_graph2ImageTable = new Hashtable();
            this.m_section2Order = new Hashtable(20);
            this.m_section2PrintModel = new Hashtable(20);
            this.m_parItem2Order = new Hashtable(30);
            this.m_parItem2PrintModel = new Hashtable(30);
        }
        #region
        protected override void ConvertGraph2Image(List<ILisReportElement> graphList, List<IExportElement> imageList)
        {
            FRImage image = new FRImage();
            ReportGraphElement rge = null;
            foreach (ILisReportElement re in graphList)
            {
                rge = re as ReportGraphElement;
                if (rge != null)
                {
                    SetExportImage(rge, image);
                }
            }
            imageList.Add(image);
        }
        protected override void AfterExport(ReportReport export)
        {
            SetReportOrder(export);
            SetReportModel(export);
        }
        #endregion

        #region
        private void SetExportImage(ReportGraphElement graph, FRImage image)
        {
            string proName = GetPropertyName(graph.GraphName);
            if (!string.IsNullOrEmpty(proName))
            {
                PropertyInfo prop = image.GetType().GetProperty(proName);
                if (prop != null)
                {
                    SetProp(prop, image, graph.GraphImage);
                }
            }
        }
        private string GetPropertyName(string name)
        {
            if (this.m_graph2ImageTable.Count == 0)
            {
                //
            }
            return this.m_graph2ImageTable[name] as string;
        }
        #endregion

        #region 排序号设置
        protected virtual void SetReportOrder(ReportReport export)
        {
            SetReportOrderNoByParItem(export);
            if (export.OrderNo <= 0)
            {
                SetReportOrderNoBySection(export);
            }
        }
        protected virtual void SetReportOrderNoBySampleType(ReportReport export)
        {
        }
        protected virtual void SetReportOrderNoBySection(ReportReport export)
        {
            int preOrder = this.GetOrderNoBySectionNo(export.SectionNo);
            if (preOrder > 1000)
            {
                export.OrderNo = preOrder;
            }
            else if (preOrder > 0 && preOrder <= 1000)
            {
                int maxParItem = MaxOrder(export.ParItemList);
                if (maxParItem > 0)
                {
                    int sufOrder = maxParItem % 10000;
                    export.OrderNo = preOrder * 10000 + sufOrder;
                }
                else
                {
                    export.OrderNo = preOrder * 10000;
                }
            }
            else
            {
                export.OrderNo = 0;
            }
        }
        protected virtual void SetReportOrderNoByParItem(ReportReport export)
        {
            List<int> orderedParItemList = this.GetOrderedParItemList();
            export.OrderNo = Intersection(export.ParItemList, orderedParItemList);
        }
        protected int GetOrderNoByParItemNo(int parItemNo)
        {
            if (this.m_parItem2Order.Count == 0)
            {
                this.InitParItem2OrderTable();
            }
            object orderNo = this.m_parItem2Order[parItemNo];
            if (orderNo == null)
            {
                return 0;
            }
            else
            {
                return (int)orderNo;
            }
        }
        protected int GetOrderNoBySectionNo(int sectionNo)
        {
            if (this.m_section2Order.Count == 0)
            {
                this.InitSection2OrderTable();
            }
            object orderNo = this.m_section2Order[sectionNo];
            if (orderNo == null)
            {
                return 0;
            }
            else
            {
                return (int)orderNo;
            }
        }
        protected List<int> GetOrderedParItemList()
        {
            List<int> result = new List<int>();
            if (this.m_parItem2Order.Count == 0)
            {
                this.InitParItem2OrderTable();
            }
            int temp;
            foreach (object c in this.m_parItem2Order.Keys)
            {
                try
                {
                    temp = Convert.ToInt32(c);
                    result.Add(temp);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return result;
        }
        protected int Intersection(List<int> first, List<int> second)
        {
            int re = 0;
            int temp;
            IEnumerable<int> result = first.Intersect<int>(second);
            foreach (int i in result)
            {
                temp = this.GetOrderNoByParItemNo(i);
                if (temp > re)
                {
                    re = temp;
                }
            }
            return re;
        }
        protected int MaxOrder(List<int> l)
        {
            int result = -1;
            foreach (int order in l)
            {
                if (order > result)
                {
                    result = order;
                }
            }
            return result;
        }
        #endregion

        #region 模板号设置
        protected virtual void SetReportModel(ReportReport export)
        {
            SetReportModelNoByParItem(export);
            if (export.PrintModelNo <= 0)
            {
                SetReportModelNoBySectionNo(export);
            }
        }
        protected virtual void SetReportModelNoBySectionNo(ReportReport export)
        {
            export.PrintModelNo = this.GetReportModelNoBySectionNo(export.SectionNo);
        }
        protected virtual void SetReportModelNoByParItem(ReportReport export)
        {
            List<int> printModelNoList = new List<int>(5);
            foreach (int item in export.ParItemList)
            {
                printModelNoList.Add(this.GetReportModelNoByParItemNo(item));
            }
            export.PrintModelNo = GetMax(printModelNoList);
        }
        protected int GetReportModelNoByParItemNo(int parItemNo)
        {
            if (this.m_parItem2PrintModel.Count == 0)
            {
                this.InitParItem2ReportModelTable();
            }
            object modelNo = this.m_parItem2PrintModel[parItemNo];
            if (modelNo == null)
            {
                return -1;
            }
            else
            {
                return (int)modelNo;
            }
        }
        protected int GetReportModelNoBySectionNo(int sectionNo)
        {
            if (this.m_section2PrintModel.Count == 0)
            {
                this.InitSection2PrintModelTable();
            }
            object modelNo = this.m_section2PrintModel[sectionNo];
            if (modelNo == null)
            {
                return -1;
            }
            else
            {
                return (int)modelNo;
            }
        }
        protected int GetMax(List<int> source)
        {
            int result = -1;
            if (source.Count > 0)
            {
                result = source[0];
                foreach (int i in source)
                {
                    if (i > result)
                    {
                        result = i;
                    }
                }
            }
            return result;
        }
        #endregion

        #region  内部实例方法
        private void InitParItem2ReportModelTable()
        {
            LisMap.InitParItem2ReportModelTable(this.m_parItem2PrintModel);
        }
        private void InitSection2PrintModelTable()
        {
            LisMap.InitSection2PrintModelTable(this.m_section2PrintModel);
        }
        private void InitSection2OrderTable()
        {
            LisMap.InitSection2OrderNoTable(this.m_section2Order);
        }
        private void InitParItem2OrderTable()
        {
            LisMap.InitParItem2OrderNoTable(this.m_parItem2Order);
        }
        #endregion
    }
}
