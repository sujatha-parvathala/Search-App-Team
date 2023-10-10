using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Search_App.Common;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Results;

namespace Search_App.BL
{
    public class SalesforceBL
    {
        private readonly SearchAppRepository _repo;
        private SalesforceClient _sfClient;
        private readonly FuzzyAndLCSS _fuzzyAndLCSS;

        public SalesforceBL()
        {
            _repo = new SearchAppRepository();
            _fuzzyAndLCSS = new FuzzyAndLCSS();
        }

        public List<SResponse> GetDataFromSalesforce(SRequest req, DataSource ds)
        {
            List<SResponse> sfResult = new List<SResponse>();  
            List<SResponse> algoAppliedResult = new List<SResponse>();
            string recordSource = _repo.GetDataSourceName(ds.GroupId);

            try
            {
                SetAuthTokenAndInstanceUrl(ds);
                var result = GetDataFromSF(req);

                if (result != null && (result.records != null && result.records.Count > 0))
                {
                    sfResult = result.records.ToList().Select(r => new SResponse
                    {
                        Name = r.Name,
                        Address = r.BillingStreet,
                        City = r.BillingCity,
                        StateCode = r.BillingState,
                        PostalCode = r.BillingPostalCode,
                         RecordSource = recordSource
                    }).ToList();

                    algoAppliedResult = _fuzzyAndLCSS.GetResultByApplyingSearchAlgos(req, sfResult);
                }
            }
            catch (Exception ex)
            {
            }

            return algoAppliedResult;

        }

        private SalesforceClient SetSalesforceClient(DataSource ds)
        {
            _sfClient = new SalesforceClient();

            //Get Salesforce configuration details from DB
            string groupId = ds.GroupId;

            var configs = _repo.GetDSConfigurationDetails(groupId);

            if (configs != null && configs.Count > 0)
            {
                foreach (var config in configs)
                {
                    switch (config.ConfigurationName)
                    {
                        case "ApiEndPoint":
                            _sfClient.ApiEndPoint = config.ConfigurationValue;
                            break;
                        case "LoginEndpoint":
                            _sfClient.LoginEndpoint = config.ConfigurationValue;
                            break;
                        case "UserName":
                            _sfClient.UserName = config.ConfigurationValue;
                            break;
                        case "Password":
                            _sfClient.Password = config.ConfigurationValue;
                            break;
                        case "ClientId":
                            _sfClient.ClientId = config.ConfigurationValue;
                            break;
                        case "ClientSecret":
                            _sfClient.ClientSecret = config.ConfigurationValue;
                            break;
                    }
                }
            }
            return _sfClient;
        }

        private void SetAuthTokenAndInstanceUrl(DataSource ds)
        {
           
            try
            {
                _sfClient = SetSalesforceClient(ds);

                HttpClient httpClient = new HttpClient();
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                 {
                    {"grant_type", "password"},
                    {"client_id", _sfClient.ClientId},
                    {"client_secret", _sfClient.ClientSecret},
                    {"username", _sfClient.UserName},
                    {"password", _sfClient.Password}
                });

                HttpResponseMessage message = httpClient.PostAsync(_sfClient.LoginEndpoint, content).Result;

                string response = message.Content.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(response);

                _sfClient.AuthToken = (string)obj["access_token"];
                _sfClient.InstanceUrl = (string)obj["instance_url"];

            }
            catch (Exception ex)
            {

            }           
        }

        private SFResult GetDataFromSF(SRequest req)
        {
            SFResult sfResult = new SFResult();
            try
            {
                string soqlQuery = getSOQLQuery(req);

                    //"query/?q=select Name,BillingStreet,BillingCity,BillingState,BillingPostalCode from account";
                HttpClient _httpClient = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_sfClient.InstanceUrl + _sfClient.ApiEndPoint + soqlQuery),
                    Content = null
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _sfClient.AuthToken);
                var resp = _httpClient.SendAsync(request).Result;
                var respContent = resp.Content.ReadAsStringAsync().Result;
                sfResult = JsonConvert.DeserializeObject<SFResult>(respContent);

            }
            catch (Exception ex)
            {

            }
            return sfResult;
                      
        }

        private string getSOQLQuery(SRequest request)
        {
            string soql = "";

            string clause = "";
           
           // string name = (request != null && !string.IsNullOrEmpty(request.Name) && request.Name.Length > 0) ? request.Name.Trim() : string.Empty;
           // string address = (request != null && !string.IsNullOrEmpty(request.Address) && request.Address.Length > 0) ? request.Address.Trim() : string.Empty;
            string city = (request != null && !string.IsNullOrEmpty(request.City) && request.City.Length > 0) ? request.City.Trim() : string.Empty;
            string state = (request != null && !string.IsNullOrEmpty(request.StateCode) && request.StateCode.Length > 0) ? request.StateCode.Trim() : string.Empty;
            string postalCode = (request != null && !string.IsNullOrEmpty(request.PostalCode) && request.PostalCode.Length > 0) ? request.PostalCode.Trim() : string.Empty;

            //Name,BillingStreet,BillingCity,BillingState,BillingPostalCode from account
            if (!string.IsNullOrEmpty(city) && city.Length > 0)
            {
                clause = string.Format("BillingCity='{0}'", city);
              
            }
            if (!string.IsNullOrEmpty(state) && state.Length > 0)
            {
                clause = (!string.IsNullOrEmpty(clause) && clause.Length > 0) ? clause + string.Format("AND BillingState='{0}'", state)
                    : string.Format("BillingState='{0}'", state);
              
            }
            if (!string.IsNullOrEmpty(postalCode) && postalCode.Length > 0)
            {
                clause = (!string.IsNullOrEmpty(clause) && clause.Length > 0) ? clause + string.Format("AND BillingPostalCode='{0}'", postalCode)
                     : string.Format("BillingPostalCode='{0}'", postalCode);
            }

            if(!string.IsNullOrEmpty(clause) && clause.Length>0)
            {
                soql = "query/?q=select Name,BillingStreet,BillingCity,BillingState,BillingPostalCode from account where "+ clause;

            }
            else
            {
                soql = "query/?q=select Name,BillingStreet,BillingCity,BillingState,BillingPostalCode from account";

            }

            return soql;
        }
    }
}