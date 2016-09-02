using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using XYS.Model.Lab;
namespace XYS.FR.Lab
{
    public class LabDAL
    {
        private static readonly DateTime MinTime;
        private static readonly string ConnectionString;

        static LabDAL()
        {
            MinTime = new DateTime(2011, 1, 1);
            ConnectionString = ConfigurationManager.ConnectionStrings["ReportMSSQL"].ConnectionString;
        }
        public LabDAL()
        {
        }

        public void SaveRecord(InfoElement info, int order, string filePath)
        {
            int update = this.UpdateActive(info.ReportID);
            if (update > 0)
            {
                //版本已存在
            }
            this.InsertRecord(info, order, filePath);
        }
        private int UpdateActive(string reportID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE  dbo.LabPDF ");
            sb.Append("SET IsActive=0 ");
            sb.Append(" where ReportID='");
            sb.Append(reportID);
            sb.Append("' and IsActive=1");
            int res = ExecuteSql(sb.ToString());
            return res;
        }
        private int InsertRecord(InfoElement info, int order, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO dbo.LabPDF(");
            sb.Append("Name,Gender,Age,CID,PID,ClinicName,VisitTime,DeptName,Doctor,BedNo,ClinicalDiagnosis,Explanation,SectionNo,SerialNo,SampleNo,SampleTypeNo,SampleTypeName,");
            sb.Append("Memo,Comment,Description,Technician,Checker,ReportContent,ReceiveTime,CollectTime,InceptTime,TestTime,CheckTime,FinalTime,InsertTime,ReportID,OrderNo,FilePath,IsActive");
            sb.Append(") values (");
            sb.Append("@Name,@Gender,@Age,@CID,@PID,@ClinicName,@VisitTime,@DeptName,@Doctor,@BedNo,@ClinicalDiagnosis,@Explanation,@SectionNo,@SerialNo,@SampleNo,@SampleTypeNo,@SampleTypeName,");
            sb.Append("@Memo,@Comment,@Description,@Technician,@Checker,@ReportContent,@ReceiveTime,@CollectTime,@InceptTime,@TestTime,@CheckTime,@FinalTime,@InsertTime,@ReportID,@OrderNo,@FilePath,@IsActive)");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,10),
					new SqlParameter("@Gender", SqlDbType.NVarChar,5),
					new SqlParameter("@Age", SqlDbType.VarChar,10),
					new SqlParameter("@CID", SqlDbType.VarChar,18),
					new SqlParameter("@PID", SqlDbType.VarChar,10),
					new SqlParameter("@ClinicName", SqlDbType.NVarChar,10),
                    new SqlParameter("@VisitTime", SqlDbType.Int,15),
                    new SqlParameter("@DeptName", SqlDbType.NVarChar,15),
                    new SqlParameter("@Doctor", SqlDbType.NVarChar,10),
                    new SqlParameter("@BedNo", SqlDbType.VarChar,10),
                    new SqlParameter("@ClinicalDiagnosis", SqlDbType.NVarChar,50),
                    new SqlParameter("@Explanation", SqlDbType.NVarChar,50),
                    new SqlParameter("@SectionNo", SqlDbType.Int),
                    new SqlParameter("@SerialNo", SqlDbType.VarChar,10),
                    new SqlParameter("@SampleNo", SqlDbType.VarChar,10),
                    new SqlParameter("@SampleTypeNo", SqlDbType.Int),
                    new SqlParameter("@SampleTypeName", SqlDbType.NVarChar,10),
                    new SqlParameter("@Memo", SqlDbType.NVarChar),
                    new SqlParameter("@Comment", SqlDbType.NVarChar),
                    new SqlParameter("@Description", SqlDbType.NVarChar),
                    new SqlParameter("@Technician", SqlDbType.NVarChar,10),
                    new SqlParameter("@Checker", SqlDbType.NVarChar,10),
                    new SqlParameter("@ReportContent", SqlDbType.NVarChar,500),
                    new SqlParameter("@ReceiveTime", SqlDbType.DateTime),
                    new SqlParameter("@CollectTime", SqlDbType.DateTime),
                    new SqlParameter("@InceptTime", SqlDbType.DateTime),
                    new SqlParameter("@TestTime", SqlDbType.DateTime),
                    new SqlParameter("@CheckTime", SqlDbType.DateTime),
                    new SqlParameter("@FinalTime", SqlDbType.DateTime),
                    new SqlParameter("@InsertTime", SqlDbType.DateTime),
                    new SqlParameter("@ReportID", SqlDbType.VarChar,50),
                    new SqlParameter("@OrderNo", SqlDbType.Int),
                    new SqlParameter("@FilePath", SqlDbType.VarChar,500),
                    new SqlParameter("@IsActive", SqlDbType.Int),
                                        };
            parameters[0].Value = info.PatientName;
            parameters[1].Value = info.GenderName;
            parameters[2].Value = info.AgeStr;
            parameters[3].Value = info.CID;
            parameters[4].Value = info.PatientID;
            parameters[5].Value = info.ClinicName;
            parameters[6].Value = info.VisitTimes;
            parameters[7].Value = info.DeptName;
            parameters[8].Value = info.Doctor;
            parameters[9].Value = info.BedNo;
            parameters[10].Value = info.ClinicalDiagnosis;
            parameters[11].Value = info.Explanation;
            parameters[12].Value = info.SectionNo;
            parameters[13].Value = info.SerialNo;
            parameters[14].Value = info.SampleNo;
            parameters[15].Value = info.SampleTypeNo;
            parameters[16].Value = info.SampleTypeName;
            parameters[17].Value = info.Memo;
            parameters[18].Value = info.Comment;
            parameters[19].Value = info.Description;
            parameters[20].Value = info.Technician;
            parameters[21].Value = info.Checker;
            parameters[22].Value = info.ReportContent;
            parameters[23].Value = this.GetDateValue(info.ReceiveTime);
            parameters[24].Value = this.GetDateValue(info.CollectTime);
            parameters[25].Value = this.GetDateValue(info.InceptTime);
            parameters[26].Value = this.GetDateValue(info.TestTime);
            parameters[27].Value = this.GetDateValue(info.CheckTime);
            parameters[28].Value = this.GetDateValue(info.FinalTime);
            parameters[29].Value = DateTime.Now;
            parameters[30].Value = info.ReportID;
            parameters[31].Value = order;
            parameters[32].Value = filePath;
            parameters[33].Value = 1;

            int res = ExecuteSql(sb.ToString(), parameters);
            return res;
        }
        private object GetDateValue(DateTime dt)
        {
            object res = null;
            if (dt > MinTime)
            {
                res = dt;
            }
            else
            {
                res = DBNull.Value;
            }
            return res;
        }
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, con))
                {
                    try
                    {
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        con.Close();
                        throw e;
                    }
                }
            }
        }
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(SQLString, cmd, null, con, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        private static void PrepareCommand(string cmdText, SqlCommand cmd, SqlTransaction trans, SqlConnection con, params SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SqlParameter parmeter in cmdParms)
                {
                    if ((parmeter.Direction == ParameterDirection.InputOutput || parmeter.Direction == ParameterDirection.Input) && parmeter.Value == null)
                    {
                        parmeter.Value = System.DBNull.Value;
                    }
                    cmd.Parameters.Add(parmeter);
                }
            }
        }
    }
}