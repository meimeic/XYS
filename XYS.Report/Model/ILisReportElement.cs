﻿using XYS.Model;
namespace XYS.Report.Model
{
    public enum ReportElementTag : int
    {
        Report = 1,
        Exam,
        Patient,
        Item,
        Graph,
        Custom,
        Temp
    }
    public interface ILisReportElement : IReportElement
    {
        ReportElementTag ElementTag { get; }
    }
}