using Search_App.Common;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

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
        


    }
}