using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Model
{
    public enum AgeType
    {
        year = 1, month, day
    }
    public enum ClinicType
    {
        hospital = 1, clinic, other, none
    }
    public enum GenderType
    {
        male = 1, female, other, none
    }
    public interface IPatientElement
    {
        string ClinicTypeName { get; }
        string GenderTypeName { get; }
    }
}
