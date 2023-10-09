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
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Configuration
    {
        public int Id { get; set; }
        public string DataSourceCode { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceType { get; set; }
        public int DataSourceTypeId { get; set; }
    }

    public class SelectedConfiguration
    {
        public int Id { get; set; }
        public string AppCode { get; set; }
        public string DataSourceCode { get; set; }
    }
}