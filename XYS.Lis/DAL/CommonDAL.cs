﻿using System;
using System.Collections;
using System.Data;
using XYS.Utility.DB;
namespace XYS.Lis.DAL
{
    public class CommonDAL
    {
        public static void GetSignImage(Hashtable imageTable)
        {
            string sql = "select cname,userimage from PUser where userimage is not null";
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            imageTable.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                imageTable.Add(dr["cname"].ToString(), (byte[])dr["userimage"]);
            }
        }
    }
}