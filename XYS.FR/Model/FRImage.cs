﻿using System;

using XYS.Report;
using XYS.Common;
namespace XYS.FR.Model
{
    public class FRImage : IExportElement
    {
        private static readonly int PropertyCount = 16;
        private byte[] m_c0;
        private byte[] m_c1;
        private byte[] m_c2;
        private byte[] m_c3;
        private byte[] m_c4;
        private byte[] m_c5;
        private byte[] m_c6;
        private byte[] m_c7;
        private string m_c8;
        private string m_c9;
        private string m_c10;
        private string m_c11;
        private string m_c12;
        private string m_c13;
        private string m_c14;
        private string m_c15;


        public FRImage()
        { }

        public static int ColumnCount
        {
            get { return PropertyCount; }
        }

        [Export]
        public byte[] C0
        {
            get { return this.m_c0; }
            set { this.m_c0 = value; }
        }
        [Export]
        public byte[] C1
        {
            get { return this.m_c1; }
            set { this.m_c1 = value; }
        }
        [Export]
        public byte[] C2
        {
            get { return this.m_c2; }
            set { this.m_c2 = value; }
        }
        [Export]
        public byte[] C3
        {
            get { return this.m_c3; }
            set { this.m_c3 = value; }
        }
        [Export]
        public byte[] C4
        {
            get { return this.m_c4; }
            set { this.m_c4 = value; }
        }
        [Export]
        public byte[] C5
        {
            get { return this.m_c5; }
            set { this.m_c5 = value; }
        }
        [Export]
        public byte[] C6
        {
            get { return this.m_c6; }
            set { this.m_c6 = value; }
        }
        [Export]
        public byte[] C7
        {
            get { return this.m_c7; }
            set { this.m_c7 = value; }
        }
        [Export]
        public string C8
        {
            get { return this.m_c8; }
            set { this.m_c8 = value; }
        }
        [Export]
        public string C9
        {
            get { return this.m_c9; }
            set { this.m_c9 = value; }
        }
        [Export]
        public string C10
        {
            get { return this.m_c10; }
            set { this.m_c10 = value; }
        }
        [Export]
        public string C11
        {
            get { return this.m_c11; }
            set { this.m_c11 = value; }
        }
        [Export]
        public string C12
        {
            get { return this.m_c12; }
            set { this.m_c12 = value; }
        }
        [Export]
        public string C13
        {
            get { return this.m_c13; }
            set { this.m_c13 = value; }
        }
        [Export]
        public string C14
        {
            get { return this.m_c14; }
            set { this.m_c14 = value; }
        }
        [Export]
        public string C15
        {
            get { return this.m_c15; }
            set { this.m_c15 = value; }
        }
    }
}