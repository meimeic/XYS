using System;

using XYS.Lis.Core;

namespace XYS.Lis
{
    public interface IReport
    {
        string Report2PDF(LisSearchRequire require);
        string Report2Json(LisSearchRequire require);
    }

}
