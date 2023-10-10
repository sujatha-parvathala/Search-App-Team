using Newtonsoft.Json;
using Search_App.Common;
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

        public FlateFileBL()
        {
            _fuzzyAndLCSS = new FuzzyAndLCSS();
        }

        public List<SResponse> GetDataFromFlatFile(SRequest request, DataSource ds)
        {
            List<SResponse> fileResult = new List<SResponse>();
            List<SResponse> algoAppliedResult = new List<SResponse>();

            try
            {
                //string filePath = @"D:\Sujath;a\Data\CustData.txt";

                string filePath = System.Web.HttpContext.Current.Server.MapPath("/Data/CustData.txt");
                string fileContent = System.IO.File.ReadAllText(filePath);
                fileResult = JsonConvert.DeserializeObject<List<SResponse>>(fileContent);

                algoAppliedResult = _fuzzyAndLCSS.GetResultByApplyingSearchAlgos(request, fileResult);
            }
            catch (Exception ex)
            {

                throw;
            }

            return algoAppliedResult;
        }
    }
}