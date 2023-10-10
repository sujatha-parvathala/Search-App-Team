using Newtonsoft.Json;
using Search_App.Common;
using Search_App.DAL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_App.BL
{
    public class FlateFileBL
    {
        private readonly FuzzyAndLCSS _fuzzyAndLCSS;
        private readonly SearchAppRepository _repo;

        public FlateFileBL()
        {
            _fuzzyAndLCSS = new FuzzyAndLCSS();
            _repo = new SearchAppRepository();
        }

        public List<SResponse> GetDataFromFlatFile(SRequest request, DataSource ds)
        {
            List<SResponse> fileResult = new List<SResponse>();
            List<SResponse> algoAppliedResult = new List<SResponse>();
            string recordSource = _repo.GetDataSourceName(ds.GroupId);

            try
            {
                //string filePath = @"D:\Sujath;a\Data\CustData.txt";

                string filePath = System.Web.HttpContext.Current.Server.MapPath("/Data/CustData.txt");
                string fileContent = System.IO.File.ReadAllText(filePath);
                fileResult = JsonConvert.DeserializeObject<List<SResponse>>(fileContent);

                bool isCityProvided, isStateProvided, isPostalCodeProvided;
                isCityProvided = (!string.IsNullOrEmpty(request.City) && request.City.Trim().Length > 0);
                isStateProvided = (!string.IsNullOrEmpty(request.StateCode) && request.StateCode.Trim().Length > 0);
                isPostalCodeProvided = (!string.IsNullOrEmpty(request.PostalCode) && request.PostalCode.Trim().Length > 0);


                if (isCityProvided && isStateProvided && isPostalCodeProvided)
                {
                    fileResult =fileResult.Where(f=>string.Equals(f.City,request.City,StringComparison.OrdinalIgnoreCase) 
                    && string.Equals(f.StateCode, request.StateCode, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(f.PostalCode, request.PostalCode, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                else if(isCityProvided && isStateProvided)
                {
                    fileResult = fileResult.Where(f => string.Equals(f.City, request.City, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(f.StateCode, request.StateCode, StringComparison.OrdinalIgnoreCase)
                    ).ToList();

                }
                else if(isStateProvided && isPostalCodeProvided)
                {
                    fileResult = fileResult.Where(f => string.Equals(f.StateCode, request.StateCode, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(f.PostalCode, request.PostalCode, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                else if(isCityProvided && isPostalCodeProvided)
                {
                    fileResult = fileResult.Where(f => string.Equals(f.City, request.City, StringComparison.OrdinalIgnoreCase)
                     && string.Equals(f.PostalCode, request.PostalCode, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                else if (isCityProvided)
                {
                    fileResult = fileResult.Where(f => string.Equals(f.City, request.City, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                else if(isStateProvided)
                {
                    fileResult = fileResult.Where(f => string.Equals(f.StateCode, request.StateCode, StringComparison.CurrentCultureIgnoreCase)                  
                   ).ToList();

                }
                else if(isPostalCodeProvided)
                {
                    fileResult = fileResult.Where(f => string.Equals(f.PostalCode, request.PostalCode, StringComparison.OrdinalIgnoreCase)
                   ).ToList();
                }

                algoAppliedResult = _fuzzyAndLCSS.GetResultByApplyingSearchAlgos(request, fileResult);
                foreach (var item in algoAppliedResult)
                {
                    item.RecordSource = recordSource;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return algoAppliedResult;
        }
    }
}