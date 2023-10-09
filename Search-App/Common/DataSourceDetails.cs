using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_App.Common
{
    public class DataSourceDetails
    {
        public int Id { get; set; }
        public string DataSourceCode { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceType { get; set; }
        public int DataSourceTypeId { get; set; }
    }
}