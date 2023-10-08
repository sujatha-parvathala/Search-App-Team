using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Search_App.Common;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
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
                        PostalCode = r.BillingPostalCode
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
                string soqlQuery = "query/?q=select Name,BillingStreet,BillingCity,BillingState,BillingPostalCode from account";
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
    }
}