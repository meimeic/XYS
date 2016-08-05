using System;
using System.Collections;
using System.Collections.Generic;


namespace XYS.FR.Service.Config
{
    public class LabConfigManager
    {
        private static readonly Hashtable ItemModelMap;
        private static readonly Hashtable SectionModelMap;

        private static LabSection FRConfig = (LabSection)System.Configuration.ConfigurationManager.GetSection("lab");

        static LabConfigManager()
        {

        }

        public static string GetModelPath(int modelNo)
        {
            return "";
        }

        public static int GetModelNo(int setionNo)
        {
            int result = 0;
            if (SectionModelMap.ContainsKey(setionNo))
            {
                result = (int)SectionModelMap[setionNo];
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
    }
}
