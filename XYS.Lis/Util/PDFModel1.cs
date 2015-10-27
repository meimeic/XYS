using System;
using System.Collections;
using System.Collections.Generic;

namespace XYS.Lis.Util
{
    public class PDFModel1
    {
         #region
        private static readonly Hashtable PARITEM2PRINTMODEL = new Hashtable();
        private static readonly Hashtable SECTION2PRINTMODEL = new Hashtable();
        private static readonly Hashtable PRINTMODELTABLE = new Hashtable();
        private static readonly Hashtable SECTION2ORDER = new Hashtable();
        private static readonly Hashtable PARITEM2ORDER = new Hashtable();
        #endregion

        #region
        public static int GetPrintModelNoByItem(int parItemNo)
        {
            if (PARITEM2PRINTMODEL.Count == 0)
            {
                InitItem2ModelTable();
            }
            object modelNo = PARITEM2PRINTMODEL[parItemNo];
            if (modelNo != null)
            {
                return (int)modelNo;
            }
            else
            {
                return -1;
            }
        }
        private static void InitItem2ModelTable()
        {
            //
        }
        public static PrintModelItem GetPrintItem(int modelNo)
        {
            if (PRINTMODELTABLE.Count == 0)
            {
                InitPrintModelTable();
            }
            return PRINTMODELTABLE[modelNo] as PrintModelItem;
        }
        private static void InitPrintModelTable()
        {

        }
        public static int GetPrintModelNoBySection(int sectionNo)
        {
            if (SECTION2PRINTMODEL.Count == 0)
            {
                InitSection2ModelTable();
            }
            object modelNo = SECTION2PRINTMODEL[sectionNo];
            if (modelNo != null)
            {
                return (int)modelNo;
            }
            else
            {
                return -1;
            }
        }
        private static void InitSection2ModelTable()
        {
 
        }
        public static int GetReportOrderByParItem(int parItemNo)
        {
            if (PARITEM2ORDER.Count == 0)
            {
                InitParItem2OrderTable();
            }
            object orderNo = PARITEM2ORDER[parItemNo];
            if (orderNo != null)
            {
                return (int)orderNo;
            }
            else
            {
                return -1;
            }
        }
        public static List<int> GetAllParItems()
        {
            List<int> result = new List<int>();
            if (PARITEM2ORDER.Count == 0)
            {
                InitParItem2OrderTable();
            }
            int temp;
            foreach (ICollection c in PARITEM2ORDER.Keys)
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
        private static void InitParItem2OrderTable()
        {
            //
        }
        public static int GetReportOrderBySection(int sectionNo)
        {
            if (SECTION2ORDER.Count == 0)
            {
                InitSection2OrderTable();
            }
            object orderNo = SECTION2ORDER[sectionNo];
            if (orderNo != null)
            {
                return (int)orderNo;
            }
            else
            {
                return -1;
            }
        }
        private static void InitSection2OrderTable()
        {
            //
        }
        
        #endregion

        #region
        public class PrintModelItem
        {
            #region
            private int m_modelNo;
            private string m_modelName;
            private string m_modelPath;
            #endregion

        }
        #endregion

    }
}
