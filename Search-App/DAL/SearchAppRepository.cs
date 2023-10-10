using Search_App.Common;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.UI;

namespace Search_App.DAL
{
    public class SearchAppRepository
    {
        private readonly string connectionString = "";
        public SearchAppRepository()
        {
         connectionString = "Data Source=LAPTOP-N00QO3R2;" +
"Initial Catalog=Practice;" +
"Integrated Security=SSPI;";
        }

        public List<DataSource> GetAppDataSourceConfigurations(string appCode)
        {
            List<DataSource> configs = new List<DataSource>();
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select ds.GroupId as GroupId,dst.DSType as DSType,dst.Id as DSTypeId  from ApplicationDSConfigurations as apds inner join DataSources as ds on apds.GroupId = ds.GroupId\r\ninner join DataSourceTypes dst on ds.DataSourceType = dst.Id where AppCode=@p_appcode";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("p_appcode", appCode);
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        configs = dt.Select().ToList().Select(dr => new DataSource { 
                             GroupId = dr["GroupId"].ToString(),
                             DataSourceType = dr["DSType"].ToString(),
                             DataSourceTypeId = Convert.ToInt32(dr["DSTypeId"].ToString())
                              
                        }).ToList(); ;
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return configs;
        }

        public List<DSConfigDetails> GetDSConfigurationDetails(string groupId)
        {
            List<DSConfigDetails> configs = new List<DSConfigDetails>();
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select * from GroupConfigurations where GroupId=@p_groupId";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("p_groupId", groupId);
                    
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        configs = dt.Select().ToList().Select(dr => new DSConfigDetails
                        {
                            ConfigurationName = dr["ConfigName"].ToString(),
                            ConfigurationValue = dr["ConfigValue"].ToString()
                        }).ToList(); ;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return configs;
        }
        
        public ApplicationDetails GetApplicationDetails(string appUserName)
        {
            ApplicationDetails appDetails = new ApplicationDetails();
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select * from Applications where AppUserName=@p_appUserName";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@p_appUserName", appUserName);

                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        appDetails.AppCode = dt.Rows[0]["AppCode"].ToString();
                        appDetails.AppName = dt.Rows[0]["AppName"].ToString();
                        appDetails.AppUserName = dt.Rows[0]["AppUserName"].ToString();
                        appDetails.AppPassword = dt.Rows[0]["AppPassword"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return appDetails;
        }

        public List<DataSourceDetails> GetAllDataSourceDetails()
        {
            //select ds.Id as Id, ds.GroupId as DataSource, dst.DSType as DataSourceType, dst.Id as DataSourceTypeId
            //from DataSources ds inner
            //join DataSourceTypes dst on ds.DataSourceType = dst.Id

            List<DataSourceDetails> dsDetails = new List<DataSourceDetails>();
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select ds.Id as Id, ds.GroupId as DataSourceCode,ds.DataSourceName, dst.DSType as DataSourceType, dst.Id as DataSourceTypeId from DataSources ds inner join DataSourceTypes dst on ds.DataSourceType = dst.Id";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dsDetails = dt.Select().ToList().Select(dr => new DataSourceDetails
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            DataSourceCode = dr["DataSourceCode"].ToString(),
                            DataSourceName = dr["DataSourceName"].ToString(),
                            DataSourceType = dr["DataSourceType"].ToString(),
                            DataSourceTypeId = Convert.ToInt32(dr["DataSourceTypeId"].ToString())

                        }).ToList(); ;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return dsDetails;
        }

        public List<AppDataSources> GetAllAppConfiguredDataSources()
        {
            List<AppDataSources> dsDetails = new List<AppDataSources>();
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "Select * from ApplicationDSConfigurations";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dsDetails = dt.Select().ToList().Select(dr => new AppDataSources
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            DataSourceCode = dr["GroupId"].ToString(),
                            AppCode = dr["AppCode"].ToString()
                        }).ToList(); ;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return dsDetails;
        }

        public List<States> GetAllStates()
        {
            List<States> states = new List<States>();

            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "Select * from States";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;                   
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        states = dt.Select().ToList().Select(dr => new States
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            Name = dr["Name"].ToString(),
                            Code = dr["Code"].ToString(),
                            CountryCode = dr["CountryCode"].ToString()

                        }).ToList(); ;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return states;
        }

        public List<Cities> GetCities(string stateCode)
        {
            List<Cities> cities = new List<Cities>();

            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                   
                    string sqlQuery = "select * from cities where StateCode=@p_stateCode";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@p_stateCode", stateCode));
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        cities = dt.Select().ToList().Select(dr => new Cities
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            Name = dr["Name"].ToString(),
                            StateCode = dr["StateCode"].ToString()                           

                        }).ToList(); ;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return cities;
        }

        public string GetDataSourceName(string dsCode)
        {
            string dsName = "";
            try
            {
                SqlDataAdapter Da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "select DataSourceName from DataSources where GroupId =@p_dscode";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@p_dscode", dsCode);
                    Da = new SqlDataAdapter(command);
                    Da.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dsName = dt.Rows[0][0]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dsName;
        }
    
    }
}