using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Search_App.Common;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Web;

namespace Search_App
{
   
    public class SFRequest
    {
        public readonly string LoginEndpoint = ""; //= "https://login.salesforce.com/services/oauth2/token/hackathondata";
        public readonly string ApiEndpoint = "/services/data/v58.0/"; //Use your org's version number

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string ServiceUrl { get; set; }
        HttpClient client { get; set; }

        public SFRequest()
        {
            //Satish
            //Username = "satishhackathon@gmail.com";
            //Password = "wellsfargo@123InOg7eqxj7jM9e12eDXb4gH";
            //ClientId = "3MVG95mg0lk4batiWarI8Wkmk57LatLUhcr7AnSQnmM0gQxpfUx.KTk9HxnDNIlAunFdmwb3a7ICaTSrZjSkz";
            //ClientSecret = "D098C10A5C5BB03DCF23632C001580CB66B41959E27FB661536821923C8B6BAD";
            //LoginEndpoint = "https://saikishore123-dev-ed.my.salesforce.com/services/oauth2/token";

            //Kishore
            Username = "sfdc.learning@gmail.com";
            Password = "sfdc.learning@123Qkm7RkUXyykfa90smc7B14YHz";
            ClientId = "3MVG9Y6d_Btp4xp7JTljLdvhrwwqykjzt.nDEfEDThhaj5.gl9kPCF4SSPglbVwdh09wCAAZyJ1jtN8PV69pG";
            ClientSecret = "D1872AC026C5654D60E4206439C0E34360BA24364506654F483B9FDF39938DB6";
            LoginEndpoint = "https://saikishore123-dev-ed.my.salesforce.com/services/oauth2/token";


        }

        public List<SResponse> GetCustomerData()
        {
            client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
      {
          {"grant_type", "password"},
          {"client_id", ClientId},
          {"client_secret", ClientSecret},
          {"username", Username},
          {"password", Password}
      });

            HttpResponseMessage message = client.PostAsync(LoginEndpoint, content).Result;

            string response = message.Content.ReadAsStringAsync().Result;
            JObject obj = JObject.Parse(response);

            AuthToken = (string)obj["access_token"];
            ServiceUrl = (string)obj["instance_url"];
            string soqlQuery = "query/?q=select Name,BillingStreet,BillingCity,BillingState,BillingPostalCode from account";

            HttpClient _httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ServiceUrl + ApiEndpoint + soqlQuery),
                Content = null
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
            var resp = _httpClient.SendAsync(request).Result;
            var respContent = resp.Content.ReadAsStringAsync().Result;
            //var textReader = new System.IO.StreamReader(respContent);
            //var jsonReader = new JsonTextReader(textReader);
            //var jsonSerializer = JsonSerializer.CreateDefault();
            
            var result = JsonConvert.DeserializeObject<SFResult>(respContent);

            List<SResponse> sresults = new List<SResponse>();
            if (result != null && (result.records != null && result.records.Count>0))
            {
              sresults = result.records.ToList().Select(r=> new SResponse {
                  Name = r.Name,
                  Address=r.BillingStreet,
                  City = r.BillingCity,
                  StateCode=r.BillingState,
                  PostalCode =r.BillingPostalCode                  

              }).ToList();
            }
            return sresults;
        }

       
    }
}