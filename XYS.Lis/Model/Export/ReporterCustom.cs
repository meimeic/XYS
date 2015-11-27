﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using XYS.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Model.Export
{
    public class ReporterCustom : ILisExportElement
    {
        private static readonly ReportElementTag Default_Tag = ReportElementTag.CustomElement;
        private string m_name;
        private string m_column0;
        private string m_column1;
        private string m_column2;
        private string m_column3;
        private string m_column4;
        private string m_column5;
        private string m_column6;
        private string m_column7;
        private string m_column8;
        private string m_column9;
        private string m_column10;
        private string m_column11;
        private string m_column12;
        private string m_column13;
        private string m_column14;
        private string m_column15;
        private string m_column16;
        private string m_column17;
        private string m_column18;
        private string m_column19;

        [JsonIgnore]
        public ReportElementTag ElementTag
        {
            get { return Default_Tag; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public string Column0
        {
            get { return this.m_column0; }
            set { this.m_column0 = value; }
        }
        public string Column1
        {
            get { return this.m_column1; }
            set { this.m_column1 = value; }
        }
        public string Column2
        {
            get { return this.m_column2; }
            set { this.m_column2 = value; }
        }
        public string Column3
        {
            get { return this.m_column3; }
            set { this.m_column3 = value; }
        }
        public string Column4
        {
            get { return this.m_column4; }
            set { this.m_column4 = value; }
        }
        public string Column5
        {
            get { return this.m_column5; }
            set { this.m_column5 = value; }
        }
        public string Column6
        {
            get { return this.m_column6; }
            set { this.m_column6 = value; }
        }
        public string Column7
        {
            get { return this.m_column7; }
            set { this.m_column7 = value; }
        }
        public string Column8
        {
            get { return this.m_column8; }
            set { this.m_column8 = value; }
        }
        public string Column9
        {
            get { return this.m_column9; }
            set { this.m_column9 = value; }
        }
        public string Column10
        {
            get { return this.m_column10; }
            set { this.m_column10 = value; }
        }
        public string Column11
        {
            get { return this.m_column11; }
            set { this.m_column11 = value; }
        }
        public string Column12
        {
            get { return this.m_column12; }
            set { this.m_column12 = value; }
        }
        public string Column13
        {
            get { return this.m_column13; }
            set { this.m_column13 = value; }
        }
        public string Column14
        {
            get { return this.m_column14; }
            set { this.m_column14 = value; }
        }
        public string Column15
        {
            get { return this.m_column15; }
            set { this.m_column15 = value; }
        }
        public string Column16
        {
            get { return this.m_column16; }
            set { this.m_column16 = value; }
        }
        public string Column17
        {
            get { return this.m_column17; }
            set { this.m_column17 = value; }
        }
        public string Column18
        {
            get { return this.m_column18; }
            set { this.m_column18 = value; }
        }
        public string Column19
        {
            get { return this.m_column19; }
            set { this.m_column19 = value; }
        }
    }
}
