using Search_App.Common;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Search_App.BL
{
    public class SqlDsBL
    {
        private readonly string connectionString;
        private readonly FuzzyAndLCSS _fuzzyAndLCSS;

        public SqlDsBL()
        {
            connectionString = "Data Source=LAPTOP-N00QO3R2;" +
"Initial Catalog=Practice;" +
"Integrated Security=SSPI;";
            _fuzzyAndLCSS = new FuzzyAndLCSS();
        }

        public List<SResponse> GetDataFromSQL(SRequest request, DataSource ds)
        {
            List<SResponse> sqlResult = new List<SResponse>();
            List<SResponse> algoAppliedResult = new List<SResponse>();
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select * from customer";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        sqlResult = dt.Select().ToList().Select(dr => new SResponse
                        {
                            Name = dr["Name"]?.ToString(),
                            Address = dr["Address"]?.ToString(),
                            City = dr["City"]?.ToString(),
                            StateCode = dr["State"]?.ToString(),
                            Country = dr["CountryCode"]?.ToString()
                        }).ToList();

                        algoAppliedResult = _fuzzyAndLCSS.GetResultByApplyingSearchAlgos(request, sqlResult);
                    }

                }

            }
            catch (Exception ex)
            {
            }


            return algoAppliedResult;
        }
    }
}