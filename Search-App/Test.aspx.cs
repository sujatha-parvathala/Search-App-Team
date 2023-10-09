using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Search_App
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //SearchAppRepository repo = new SearchAppRepository();

            // var data = repo.GetAllDataSourceDetails();
            // var dsstring = JsonConvert.SerializeObject(data);

            //System.IO.File.WriteAllText(@"D:\Sujatha\AllDSData.json", dsstring);
            // var appData = repo.GetAllAppConfiguredDataSources();
            // var apDsString = JsonConvert.SerializeObject(appData);
            // System.IO.File.WriteAllText(@"D:\Sujatha\AppCongigDSdata.json", apDsString);


            //var dbResult = repo.GetAppDataSourceConfigurations("SBA");

            //SFRequest req = new SFRequest();

            //req.GetCustomerData();
            //double max = Math.Max(75.23, 80.54);
            //FuzzySearchLogic logic = new FuzzySearchLogic();
            //SRequest request = new SRequest { Name="Anandd", Address="Rd 6"};

            //var result = logic.GetSearchResult(request);

            // var node = new Uri("https://localhost:9200");

            var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));

            var settings = new ConnectionSettings(pool)
            .BasicAuthentication("elastic", "kjl1UF2VZd24HY7KDvIE")
            .CertificateFingerprint("8e4ecd35a960b6316f79640e2c97151b15263b2bdabed44181a96c1dc586f62b")
            .DefaultIndex("hacathon_customer")
            .EnableApiVersioningHeader();


                
              // Replace with your Elasticsearch server and index name
           
           

            //var settings = new ConnectionSettings(node).DefaultIndex("hacathon_customer");
            //settings.BasicAuthentication("elastic", "kjl1UF2VZd24HY7KDvIE");
            //var client = new ElasticClient(settings);

            var es = new ElasticClient(settings);
            var clasuse = new List<QueryContainer>();
            var fuzzyClause = new List<QueryContainer>();


            clasuse.Add(new MatchQuery
                {
                Field="name",
                Query = "sujatah",
                FuzzyTranspositions = true,
                Fuzziness=Fuzziness.Auto,
                MinimumShouldMatch=1
            });
            var serRequest = new SearchRequest<ESClass> { Query = new BoolQuery {Should =clasuse, MinimumShouldMatch=1}, Size = 200 };
            //var sResponse = es.Search<SResponse>(serRequest);
            var searchResponse = es.Search<ESClass>(serRequest);

            //fuzzyClause.Add(new FuzzyQuery { 
            //     Field="name",
            //     Value="sujatha parvathala ", Fuzziness = Fuzziness.Auto, Transpositions=true
            //});
            //fuzzyClause.Add(new FuzzyQuery
            //{
            //    Field = "address",
            //    Value = "Road 19",               
            //    Transpositions = true
            //});

            //var fuzzySearchReq = new SearchRequest<ESClass> { Query = new BoolQuery { Should  = fuzzyClause }, Size = 2000, MinScore=10 };
            //var fresult = es.Search<ESClass>(fuzzySearchReq);


        //    var res = searchResponse.Hits.Select(h => h.Source.name).ToList();

        //    //var sr = es.Search<ESClass>(s => s
        //    //.Query(q => q.ma
        //    //    .Fuzzy(fz => fz
        //    //        .Field(f => f.name)
        //    //        .Value("Sujata")
        //    //        //.Fuzziness(Fuzziness.Ratio) // You can adjust the fuzziness level
        //    //    )                
        //    //)

           
        //    );
        //);
        //    var hits = sr.Hits;

            if (!searchResponse.IsValid)
            {
                // Handle errors
                var debugInfo = searchResponse.DebugInformation;
                var error = searchResponse.ServerError.Error;
            }





        }
        

//        private void InsertStates()
//        {
//            string filePath = @"D:\Sujatha\Samples\Search-App\Search-App\Data\states.json";
//            DataTable dt = new DataTable();
//            dt.Columns.Add("Id",typeof(System.Int32));
//            dt.Columns.Add("Name", typeof(string));
//            dt.Columns.Add("StateCode", typeof(string));

//            string jsonString = System.IO.File.ReadAllText(filePath);
//            dt = JsonConvert.DeserializeObject<DataTable>(jsonString);

//            SqlDataAdapter Da = new SqlDataAdapter();
            
//            string conStr = "Data Source=LAPTOP-N00QO3R2;" +
//"Initial Catalog=Practice;" +
//"Integrated Security=SSPI;";

//            string insertString = @"insert into States([Id],[Name],Code,CountryCode) values(@Id,@Name,@Code,@CountryCode)";
//            SqlCommand insertCommand = new SqlCommand(insertString);
//            insertCommand.Parameters.Add("@Id", SqlDbType.Int);
//            insertCommand.Parameters.Add("@Name", SqlDbType.NVarChar);
//            insertCommand.Parameters.Add("@Code",SqlDbType.NVarChar);    
//            insertCommand.Parameters.Add("@CountryCode", SqlDbType.NVarChar);

//            using (SqlConnection connection = new SqlConnection(conStr))
//            {
//                connection.Open();
//                insertCommand.Connection = connection;
//                using (SqlDataAdapter adapter = new SqlDataAdapter())
//                {
//                    adapter.InsertCommand = insertCommand;
//                    adapter.Update(dt);
//                }
//            }



//        }
        }

    public class ESClass
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalcode { get; set; }
        public string countrycode { get; set; }
    }
}