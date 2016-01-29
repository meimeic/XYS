namespace XYS.Common
{
    public enum AgeType
    {
        year = 1, month, day,hours,none
    }
    public enum ClinicType
    {
        hospital = 1, clinic, other, none
    }
    public enum GenderType
    {
        male = 1, female, other, none
    }
    public interface IPatient
    {
        string ClinicName { get; }
        string GenderName { get; }
    }
}
