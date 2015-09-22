using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Model
{
    public interface IPatientElement
    {
        string PatientElementName { get; }
        long PatientElementValue { get; }
    }
}
