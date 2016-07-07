using System;
using System.Collections.Generic;
using System.Web;

namespace XYS.His.WebAPI.Models
{
    public class PatientModel
    {
        public string Name { get; set; }
        public string PatientId { get; set; }

        public string VisitNo { get; set; }

        public string BedNo { get; set; }
    }
}