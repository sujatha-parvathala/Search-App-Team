using Elasticsearch.Net;
using Nest;
using Search_App.Common;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_App.BL
{
    public class ElasticSearchBL
    {
        private readonly FuzzyAndLCSS _fuzzyAndLCSS;

        public ElasticSearchBL()
        {
            _fuzzyAndLCSS = new FuzzyAndLCSS();
        }
        public List<SResponse> GetDataFromElasticSearch(SRequest request, DataSource ds)
        {

            List<SResponse> esResult = new List<SResponse>();
            List<SResponse> algoAppliedResult = new List<SResponse>();
            try
            {
                ElasticClient esClient = GetElasticClient();
                QueryContainer queryContainer = GetESQueryContainer(request);
                var esRequest = new SearchRequest<hacathonCustomer> { Query =queryContainer, Size = 200 };
                var esResponse = esClient.Search<hacathonCustomer>(esRequest);

               if(esResponse.IsValid)
                {
                    if (esResponse.Hits != null && esResponse.Hits.Count > 0)
                    {
                        var customers = esResponse.Hits.Select(h => new hacathonCustomer {
                            name=h.Source.name,
                            address=h.Source.address,
                            city=h.Source.city,
                            state=h.Source.state,
                            postalcode=h.Source.postalcode,
                        }).ToList();

                        esResult = customers.Select(c=> new SResponse {
                            Name=c.name, Address=c.address, StateCode=c.state,
                            PostalCode=c.postalcode,ADScore=0,NSScore=0,Score=0, City=c.city, Country="USA"
                        }).ToList();
                    }
                }

                algoAppliedResult = _fuzzyAndLCSS.GetResultByApplyingSearchAlgos(request, esResult);
            }
            catch (Exception ex)
            {

            }

            return algoAppliedResult;
        }

        private ElasticClient GetElasticClient()
        {
            var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));

            var settings = new ConnectionSettings(pool)
            .BasicAuthentication("elastic", "kjl1UF2VZd24HY7KDvIE")
            .CertificateFingerprint("8e4ecd35a960b6316f79640e2c97151b15263b2bdabed44181a96c1dc586f62b")
            .DefaultIndex("hacathon_customer")
            .EnableApiVersioningHeader();

            var elasticClient = new ElasticClient(settings);
            return elasticClient;
        }

        private QueryContainer GetESQueryContainer(SRequest request)
        {
            QueryContainer queryContainer = new QueryContainer();
            List<QueryContainer> shouldclause = new List<QueryContainer> ();
            List<QueryContainer> mustclause = new List<QueryContainer>();

            string name = (request != null && !string.IsNullOrEmpty(request.Name) && request.Name.Length > 0) ? request.Name.Trim() : string.Empty;
            string address = (request != null && !string.IsNullOrEmpty(request.Address) && request.Address.Length > 0) ? request.Address.Trim() : string.Empty;
            string city = (request != null && !string.IsNullOrEmpty(request.City) && request.City.Length > 0) ? request.City.Trim() : string.Empty;
            string state = (request != null && !string.IsNullOrEmpty(request.StateCode) && request.StateCode.Length > 0) ? request.StateCode.Trim() : string.Empty;
            string postalCode = (request != null && !string.IsNullOrEmpty(request.PostalCode) && request.PostalCode.Length > 0) ? request.PostalCode.Trim() : string.Empty;

            if((!string.IsNullOrEmpty(name) && name.Length > 0))
            {
                shouldclause.Add(new MatchQuery
                {
                    Field = "name",
                    Query = name,
                    FuzzyTranspositions = true,
                    Fuzziness = Fuzziness.Auto,
                    MinimumShouldMatch = 1
                });
            }
            if ((!string.IsNullOrEmpty(address) && address.Length > 0))
            {
                shouldclause.Add(new MatchQuery
                {
                    Field = "address",
                    Query = address,
                    FuzzyTranspositions = true,
                    Fuzziness = Fuzziness.Auto,
                    MinimumShouldMatch = 1
                });
            }
            if ((!string.IsNullOrEmpty(city) && city.Length > 0))
            {
                mustclause.Add(new MatchQuery
                {
                    Field = "city",
                    Query = city                    
                });
            }
            if ((!string.IsNullOrEmpty(state) && state.Length > 0))
            {
                mustclause.Add(new MatchQuery
                {
                    Field = "state",
                    Query = state
                });
            }
            if ((!string.IsNullOrEmpty(postalCode) && postalCode.Length > 0))
            {
                mustclause.Add(new MatchQuery
                {
                    Field = "postalcode",
                    Query = postalCode
                });
            }

            queryContainer = new BoolQuery { Should = shouldclause, MinimumShouldMatch = 1, Must = mustclause };          

            return queryContainer;
        }
    }

    public class hacathonCustomer
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalcode { get; set; }
        public string countrycode { get; set; }
    }
}