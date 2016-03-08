using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.FRReport.Config
{
    public class LisFRConfigManager
    {
        private static LisFRSection FRConfig = (LisFRSection)System.Configuration.ConfigurationManager.GetSection("lisFR");
    }
}
