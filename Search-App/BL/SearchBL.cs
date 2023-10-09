using Search_App.Common;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Search_App.BL
{
    public class SearchBL
    {
        private readonly SearchAppRepository _repo;
        private readonly SalesforceBL _sfBL;
        private readonly SqlDsBL _sqlDsBL;
        private readonly ElasticSearchBL _elasticsearchBL;
        private readonly FlateFileBL _flatFileBL;
        public SearchBL()
        {
            _repo = new SearchAppRepository(); 
            _sfBL = new SalesforceBL();
            _sqlDsBL = new SqlDsBL();
            _elasticsearchBL= new ElasticSearchBL();
            _flatFileBL = new FlateFileBL();
        }

        public List<SResponse> SearchResult(SRequest request, string appCode)
        {
            List<SResponse> result = new List<SResponse>();
            List<SResponse> dsResult = new List<SResponse>();

            try
            {
                // Get DataSources list for the Given AppId
                List<DataSource> dbResult = _repo.GetAppDataSourceConfigurations(appCode);
                List<SResponse> serarchResult;
                //For each DataSource Get Data from corresponding DataSource and add append SerachResults
                if(dbResult!=null && dbResult.Count>0)
                {
                    foreach(DataSource d in dbResult)
                    {
                        switch (d.DataSourceTypeId) {
                            case 1:
                                serarchResult = _sfBL.GetDataFromSalesforce(request, d);
                                if(serarchResult!=null && serarchResult.Count>0)
                                {
                                    dsResult.AddRange(serarchResult.ToList());
                                }
                                break;
                            case 2:
                                serarchResult = _elasticsearchBL.GetDataFromElasticSearch(request, d);
                                if (serarchResult != null && serarchResult.Count > 0)
                                {
                                    dsResult.AddRange(serarchResult.ToList());
                                }
                                break;
                             case 3:
                                serarchResult = _sqlDsBL.GetDataFromSQL(request, d);
                                if (serarchResult != null && serarchResult.Count > 0)
                                {
                                    dsResult.AddRange(serarchResult.ToList());
                                }
                                break;
                             case 4:
                                serarchResult = _flatFileBL.GetDataFromFlatFile(request, d);
                               
                                if (serarchResult != null && serarchResult.Count > 0)
                                {
                                    dsResult.AddRange(serarchResult.ToList());
                                }
                                break;

                        }
                        
                    }
                }
                //return top 2000 results based on Score

                if(dsResult!=null && dsResult.Count>0) { 
                   result = dsResult.OrderByDescending(s => s.Score).ToList();
                    result = result.Take(2000).ToList();                   
                }
            }
            catch(Exception ex)
            {

            }

            return result;
        }

       

     
    }
}