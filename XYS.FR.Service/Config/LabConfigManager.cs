using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using XYS.Util;
namespace XYS.FR.Service.Config
{
    public class LabConfigManager
    {
        private static readonly Hashtable ItemModelMap;
        private static readonly Hashtable GroupModelMap;
        private static readonly Hashtable PrintModelMap;

        private static readonly Hashtable ItemOrderMap;
        private static readonly Hashtable GroupOrderMap;

        private static LabSection FRConfig = (LabSection)System.Configuration.ConfigurationManager.GetSection("lab");

        static LabConfigManager()
        {
            PrintModelMap = new Hashtable(20);
            ItemModelMap = new Hashtable(50);
            GroupModelMap = new Hashtable(30);

            ItemOrderMap = new Hashtable(50);
            GroupOrderMap = new Hashtable(30);

            InitGroup();
            InitItem();
            InitModel();
        }

        public static string GetModelPath(int modelNo)
        {
            string result = PrintModelMap[modelNo] as string;
            if (result == null)
            {

            }
            return result;
        }

        public static int GetModelNo(int setionNo)
        {
            int result = 0;
            if (GroupModelMap.ContainsKey(setionNo))
            {
                result = (int)GroupModelMap[setionNo];
            }
            return result;
        }
        public static int GetModelNo(List<int> itemList)
        {
            int result = 0;
            int temp = 0;
            foreach (int no in itemList)
            {
                if (ItemModelMap.ContainsKey(no))
                {
                    temp = (int)ItemModelMap[no];
                    if (temp > result)
                    {
                        result = temp;
                    }
                }
            }
            return result;
        }
        
        public static int GetOrderNo(int setionNo)
        {
            return 0;
        }
        public static int GetOrderNo(List<int> itemList)
        {
            return 0;
        }

        private static void InitGroup()
        {
            LabGroupCollection groups = FRConfig.GroupCollection;
            foreach (LabGroupSection group in groups)
            {
                GroupModelMap[group.Value] = group.ModelNo;
                GroupOrderMap[group.Value] = group.OrderNo;
            }
        }
        private static void InitItem()
        {
            LabItemCollection items = FRConfig.ItemCollection;
            foreach (LabItemSection item in items)
            {
                ItemModelMap[item.Value] = item.ModelNo;
                ItemOrderMap[item.Value] = item.OrderNo;
            }
        }
        private static void InitModel()
        {
            LabModelCollection models = FRConfig.ModelCollection;
            foreach (LabModelSection model in models)
            {
                PrintModelMap[model.Value] = Path.Combine(SystemInfo.ApplicationBaseDirectory, "Print", "Model", model.ModelName);
            }
        }
    }
}