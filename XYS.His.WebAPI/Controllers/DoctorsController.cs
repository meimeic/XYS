using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XYS.His.WebAPI.Models;

namespace XYS.His.WebAPI.Controllers
{
    [RoutePrefix("api/his")]
    public class DoctorsController : ApiController
    {
        [HttpGet, Route("doctors/{doctorID}/patients")]
        public List<PatientModel> GetPatients(string doctorID)
        {
            List<PatientModel> res = new List<PatientModel>(3);
            PatientModel p = new PatientModel();
            p.Name = "张三";
            p.PatientId = "123";
            p.VisitNo = "1";
            p.BedNo = "10";
            res.Add(p);
            PatientModel p1 = new PatientModel();
            p1.Name = "李四";
            p1.PatientId = "456";
            p1.VisitNo = "2";
            p1.BedNo = "11";
            res.Add(p1);
            PatientModel p2 = new PatientModel();
            p2.Name = "王五";
            p2.PatientId = "789";
            p2.VisitNo = "3";
            p2.BedNo = "12";
            res.Add(p2);
            return res;
        }
    }
}