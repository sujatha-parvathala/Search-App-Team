using Newtonsoft.Json;
using Search_App.Common;
using Search_App.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Search_App
{
    public partial class Configurations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataSources(); 
        }

        private void FillDataSources()
        {
            string appCode = Session["appCode"].ToString();
            SearchAppRepository _repo = new SearchAppRepository();

            List<DataSourceDetails> allDataSource = _repo.GetAllDataSourceDetails();
            var appDataSources = _repo.GetAllAppConfiguredDataSources().Where(a => a.AppCode == appCode).ToList();

            //string allConfigs = @"[{""Id"":1,""DataSourceCode"":""SFOrg-1"",""DataSourceName"":""Salesforce Org-1"",""DataSourceType"":""SalesForce"",""DataSourceTypeId"":1},{""Id"":2,""DataSourceCode"":""SFOrg-2"",""DataSourceName"":""Salesforce Org-1"",""DataSourceType"":""SalesForce"",""DataSourceTypeId"":1},{""Id"":3,""DataSourceCode"":""ES1-1"",""DataSourceName"":""ElasticSearch -1"",""DataSourceType"":""ElasticSearch"",""DataSourceTypeId"":2},{""Id"":4,""DataSourceCode"":""SQ-1"",""DataSourceName"":""SQL Database -1"",""DataSourceType"":""SQL"",""DataSourceTypeId"":3}]";
            //string selectedConfigs = @"[{""Id"":1,""AppCode"":""SBA"",""DataSourceCode"":""SFOrg-1""},{""Id"":2,""AppCode"":""SBA"",""DataSourceCode"":""SFOrg-2""},{""Id"":3,""AppCode"":""CRE"",""DataSourceCode"":""SQ-1""},{""Id"":4,""AppCode"":""CRE"",""DataSourceCode"":""ES1-1""}]";
            //List<Configuration> allConfigurations = JsonConvert.DeserializeObject<List<Configuration>>(allConfigs);
            //List<SelectedConfiguration> selectedConfigurations = JsonConvert.DeserializeObject<List<SelectedConfiguration>>(selectedConfigs);
            string configDiv = string.Empty;
            int i =0;
            //configDiv = configDiv + "<div class='row mb-3'";
            bool isAppDS = false;

            foreach (var ds in allDataSource)
            {
                i++;
                configDiv += "<div class='form-check'>";
                isAppDS = appDataSources.Any(a => a.DataSourceCode == ds.DataSourceCode);
                if(isAppDS)
                {
                    configDiv += "<input type='checkbox' class='form-check-input' id='customCheck" + i + "' checked='" + true + "'> ";
                }
                else
                {
                    configDiv += "<input type='checkbox' class='form-check-input' id='customCheck" + i + "'>";
                }
               
                string strlabel = ds.DataSourceName + " (" + ds.DataSourceType + ")";
                configDiv += "<label class='form-check-label' for='customCheck" + i +"'>" + strlabel + "</label>";

                
                configDiv += "</div>";
            }
           
            //configDiv += "</div>";

            divConfigurations.InnerHtml = configDiv;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}