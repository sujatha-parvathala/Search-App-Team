using Newtonsoft.Json;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Search_App
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           SearchAppRepository repo = new SearchAppRepository();

            var dbResult = repo.GetAppDataSourceConfigurations("SBA");
            
            SFRequest req = new SFRequest();

            req.GetCustomerData();
            double max = Math.Max(75.23, 80.54);
            FuzzySearchLogic logic = new FuzzySearchLogic();
            SRequest request = new SRequest { Name="Anandd", Address="Rd 6"};

            var result = logic.GetSearchResult(request);
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
}