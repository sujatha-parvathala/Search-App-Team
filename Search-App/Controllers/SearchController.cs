using Search_App.BL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Search_App.Controllers
{
    public class SearchController : ApiController
    {
        private readonly SearchBL _searchBL;
        public SearchController()
        {
            _searchBL = new SearchBL();
            
        }
        [HttpGet]
        public IHttpActionResult GetTestMessage()
        {
            return Ok("Test Message");
        }

        [HttpPost]
        public IHttpActionResult GetResult(Request request)
        {
            List<SResponse> result = new List<SResponse>();
            
            try
            {
                SRequest srequest = new SRequest { 
                    Name=request.Name,
                    Address =request.Address,
                    City = request.City,
                    PostalCode = request.PostalCode,
                    Country = request.Country,
                    StateCode = request.StateCode
                };
                result = _searchBL.SearchResult(srequest, request.AppCode);
            }
            catch(Exception ex) { 
            }

            return Ok(result);

            //return Ok(new List<SResponse> { new SResponse { Name="Sujatha", Address="TNGOs Colony",
            // City="Hyderbad", StateCode="TL", Country="India", PostalCode="500032",
            // NSScore=100, ADScore=100,Score=100
            //} });
        }

    }
}
