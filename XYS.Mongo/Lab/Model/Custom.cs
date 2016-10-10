using System;
using MongoDB.Bson.Serialization.Attributes;

namespace XYS.Mongo.Lab.Model
{
    public class Custom
    {
        private string m_c0;
        private string m_c1;
        private string m_c2;
        private string m_c3;
        private string m_c4;
        private string m_c5;
        private string m_c6;
        private string m_c7;
        private string m_c8;
        private string m_c9;
        private string m_c10;
        private string m_c11;
        private string m_c12;
        private string m_c13;
        private string m_c14;
        private string m_c15;
        public Custom()
        { }

        [BsonElement("c0", Order = 0)]
        public string C0
        {
            get { return this.m_c0; }
            set { this.m_c0 = value; }
        }
        [BsonElement("c1", Order = 1)]
        public string C1
        {
            get { return this.m_c1; }
            set { this.m_c1 = value; }
        }
        [BsonElement("c2", Order = 2)]
        public string C2
        {
            get { return this.m_c2; }
            set { this.m_c2 = value; }
        }
        [BsonElement("c3", Order = 3)]
        public string C3
        {
            get { return this.m_c3; }
            set { this.m_c3 = value; }
        }
        [BsonElement("c4", Order = 4)]
        public string C4
        {
            get { return this.m_c4; }
            set { this.m_c4 = value; }
        }
        [BsonElement("c5", Order = 5)]
        public string C5
        {
            get { return this.m_c5; }
            set { this.m_c5 = value; }
        }
        [BsonElement("c6", Order = 6)]
        public string C6
        {
            get { return this.m_c6; }
            set { this.m_c6 = value; }
        }
        [BsonElement("c7", Order = 7)]
        public string C7
        {
            get { return this.m_c7; }
            set { this.m_c7 = value; }
        }
        [BsonElement("c8", Order = 8)]
        public string C8
        {
            get { return this.m_c8; }
            set { this.m_c8 = value; }
        }
        [BsonElement("c9", Order = 9)]
        public string C9
        {
            get { return this.m_c9; }
            set { this.m_c9 = value; }
        }
        [BsonElement("c10", Order = 10)]
        public string C10
        {
            get { return this.m_c10; }
            set { this.m_c10 = value; }
        }
        [BsonElement("c11", Order = 11)]
        public string C11
        {
            get { return this.m_c11; }
            set { this.m_c11 = value; }
        }
        [BsonElement("c12", Order = 12)]
        public string C12
        {
            get { return this.m_c12; }
            set { this.m_c12 = value; }
        }
        [BsonElement("c13", Order = 13)]
        public string C13
        {
            get { return this.m_c13; }
            set { this.m_c13 = value; }
        }
        [BsonElement("c14", Order = 14)]
        public string C14
        {
            get { return this.m_c14; }
            set { this.m_c14 = value; }
        }
        [BsonElement("c15", Order = 15)]
        public string C15
        {
            get { return this.m_c15; }
            set { this.m_c15 = value; }
        }
    }
}