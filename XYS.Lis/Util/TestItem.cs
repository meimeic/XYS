using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Util
{
    public class TestItem
    {
        #region
        private static readonly List<int> HideItems = new List<int>();
        private static readonly Hashtable ParItem2NormalImage = new Hashtable();
        #endregion

        #region
        public static List<int> GetHideItemNo
        {
            get
            {
                if (HideItems.Count == 0)
                {
                    InitHideItems();
                }
                return HideItems;
            }
        }

        private static void InitHideItems()
        {
            //
        }

        public static byte[] GetNormalImage(int parItemNo)
        {
            if (ParItem2NormalImage.Count == 0)
            {
                InitNormalImageTable();
            }
            return ParItem2NormalImage[parItemNo] as byte[];
        }

        private static void InitNormalImageTable()
        {
 
        }
        #endregion
    }
}
