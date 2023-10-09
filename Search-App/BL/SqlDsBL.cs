﻿using Search_App.Common;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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

                // Get Where clause and Parameters
                var whereResult = GetWhereClauseAndParameters(request);
                string whereClause = whereResult.Item1;
                List<SqlParameter> parameters = whereResult.Item2;                   

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select * from customer";
                    SqlCommand command;
                    if (!string.IsNullOrEmpty(whereClause) && whereClause.Length>0)
                    {
                        sqlQuery = sqlQuery + " Where " + whereClause;
                        command = new SqlCommand(sqlQuery, con);
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    else
                    {
                        command = new SqlCommand(sqlQuery, con);
                    }
                   
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
    
        private Tuple<string, List<SqlParameter>> GetWhereClauseAndParameters(SRequest request)
        {
            string clause = "";
            List<SqlParameter> parameters = new List<SqlParameter>();

            string name = (request != null && !string.IsNullOrEmpty(request.Name) && request.Name.Length>0 ) ? request.Name.Trim() : string.Empty;
            string address = (request != null && !string.IsNullOrEmpty(request.Address) && request.Address.Length > 0) ? request.Address.Trim() : string.Empty;
            string city = (request != null && !string.IsNullOrEmpty(request.City) && request.City.Length > 0) ? request.City.Trim() : string.Empty;
            string state = (request != null && !string.IsNullOrEmpty(request.StateCode) && request.StateCode.Length > 0) ? request.StateCode.Trim() : string.Empty;
            string postalCode = (request != null && !string.IsNullOrEmpty(request.PostalCode) && request.PostalCode.Length > 0) ? request.PostalCode.Trim() : string.Empty;

            if ((!string.IsNullOrEmpty(name) && name.Length > 0) && !string.IsNullOrEmpty(address) && address.Length > 0)
            {
                if(request.AndLogicalOperator)
                {
                    clause = "(FREETEXT([Name],@p_name) AND FREETEXT([Address],@p_address))";
                }
                else
                {
                    clause = "(FREETEXT([Name],@p_name) OR FREETEXT([Address],@p_address))";
                }
               
                parameters.Add(new SqlParameter("@p_name", name));
                parameters.Add(new SqlParameter("@p_address", address));
            }
            else if (!string.IsNullOrEmpty(name) && name.Length > 0)
            {
                clause = "FREETEXT([Name], @p_Name)";
                parameters.Add(new SqlParameter("@p_name", name));
            }
            else if (!string.IsNullOrEmpty(address) && address.Length > 0)
            {
                clause = "FREETEXT([Address],@p_address)";
                parameters.Add(new SqlParameter("@p_address", address));
            }

            if (!string.IsNullOrEmpty(city) && city.Length > 0)
            {
                clause = (!string.IsNullOrEmpty(clause) && clause.Length > 0) ? clause + " AND City= @p_city"
                    : "City=@p_City";
                parameters.Add(new SqlParameter("@p_city", city));
            }
            if (!string.IsNullOrEmpty(state) && state.Length > 0)
            {
                clause = (!string.IsNullOrEmpty(clause) && clause.Length > 0) ? clause + " AND [State]= @p_state"
                    : "[State]= @p_state";
                parameters.Add(new SqlParameter("@p_state", state));
            }
            if (!string.IsNullOrEmpty(postalCode) && postalCode.Length > 0)
            {
                clause = (!string.IsNullOrEmpty(clause) && clause.Length > 0) ? clause + " AND PostalCode= @p_postalCode"
                    : "PostalCode= @p_postalCode";
                parameters.Add(new SqlParameter("@p_postalCode", postalCode));
            }



            return new Tuple<string, List<SqlParameter>>(clause, parameters);
        }
    }
}