using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Mvc;

namespace WebAtithya.Models
{
    public class ConnectionClass
    {
        SqlConnection conn;

        //For Execute Saving Query
        //        SqlConnection scon = null; SqlCommand cmd = null; SqlTransaction strans = null; scon = cs.GetConnection();
        //        strans = scon.BeginTransaction();
        //        strSQLQuery = "Delete From AcGroupPath Where GroupCode='" + StrDocID + "'";
        //        cmd = new SqlCommand(strSQLQuery, scon)
        //              {
        //                  Transaction = strans
        //               };
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //        strans.Commit();
        //        scon.Close();
        //        strans.Rollback();
        //        scon.Close();

        public SqlConnection GetConnection()
        {
            try
            {
                string str = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                conn = new SqlConnection(str);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                return conn;
            }
            catch
            {
                return conn;
            }
        }

        public void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public DataTable FillDatatable(string strCommand)
        {
            DataTable dt = new DataTable();
            if (strCommand != "")
            {
                using (SqlConnection con = GetConnection())
                {
                    //con.Open();
                    using (SqlCommand cmd = new SqlCommand(strCommand, con))
                    {
                        dt = new DataTable();
                        dt.Clear();
                        SqlDataReader rd = cmd.ExecuteReader();
                        dt.Load(rd);
                    }
                }
            }
            return dt;
        }

        public string execute_scaler_string(string str)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    //con.Open();
                    using (SqlCommand cmd = new SqlCommand(str, con))
                    {
                        var result = cmd.ExecuteScalar();
                        string flag = (result == null || result == DBNull.Value)
                                      ? ""
                                      : result.ToString();
                        return flag;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public int execute_nonquery(string str)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    //con.Open();
                    using (SqlCommand cmd = new SqlCommand(str, con))
                    {
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string ConvertDatatableToJson(DataTable dt)
        {
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }

        public ContentResult JsonContent(object data)
        {
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };
        }

        public string getMaxCode(string Prefix, string TableName, string UserName)
        {
            string query = $@"SELECT '{Prefix}' + CAST(ISNULL(MAX(CAST(SUBSTRING(Code, 3, LEN(Code)) AS INT)), 0) + 1 AS VARCHAR(10)) AS NextCode FROM {TableName} where UserName = '{UserName}'";
            return execute_scaler_string(query);
        }

    }
}