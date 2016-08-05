using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.FR.Service.Config
{
    public class LisConfigManager
    {
        private static LisSection FRConfig = (LisSection)System.Configuration.ConfigurationManager.GetSection("lis");
    }
}
