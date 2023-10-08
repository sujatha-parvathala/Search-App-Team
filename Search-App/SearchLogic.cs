using FuzzySharp;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Web;

namespace Search_App
{
    public class SearchLogic
    {
        private readonly List<string> Punctuation;
        private readonly double nameWeight, dbaWeight,addressWeight;

        public SearchLogic()
        {
            Punctuation = new List<string> { ".",",","!","@","#" };
            nameWeight = 70.00;
            dbaWeight = 15.00;
            addressWeight = 15.00;
        }

        public List<SResponse> GetSearchResul(SRequest request)
        {
            List<SResponse> searchResult = new List<SResponse>();
            List<SResponse> finalResult = new List<SResponse>();

            bool DBANameProvided = false;
            bool NameProvided = false;
            bool AddressProvided = false;

            string customerType, firstName, middleName, lastName, companyName, dbaName, city, state, postalcode;
            string address, name;
            //fetching data from request
            customerType = request.CustomerType;
            firstName = request.FirstName;
            middleName = request.MiddleName;
            lastName = request.LastName;
            companyName = request.CompanyName;
            dbaName = request.DBAName;
            city = request.City;
            state = request.StateCode;
            postalcode = request.PostalCode;
            address = request.Address1 + " " + request.Address2;

            name = string.IsNullOrEmpty(companyName) ?
                string.Format("{0} {1} {2}", firstName, middleName, lastName) : companyName;

            name = string.IsNullOrEmpty(name)?"":name.Trim();
            address = string.IsNullOrEmpty(address) ? "" : address.Trim();
            dbaName = string.IsNullOrEmpty(dbaName) ? "" : dbaName.Trim();
          
            //Get Search result from DB or ES or SF

            searchResult= new List<SResponse>();

            //end of getting search result

            if(searchResult !=null && searchResult.Any())
            {
                NameProvided = (!string.IsNullOrEmpty(name) && name.Length > 0);
                DBANameProvided = (!string.IsNullOrEmpty(dbaName) && dbaName.Length > 0);
                AddressProvided = (!string.IsNullOrEmpty(address) && address.Length > 0);
            }

            if(AddressProvided && (NameProvided || DBANameProvided))
            {
                finalResult = GetNameAndAddressSearchResult(searchResult, name, dbaName, address);
            }
            else if(NameProvided || DBANameProvided)
            {
                finalResult = GetNameSearchResult(searchResult, name, dbaName);
            }
            else
            {
                finalResult = GetAddressSearchResult(searchResult, address, city, state);
            }

            return finalResult;

        }


        public List<SResponse> GetNameSearchResult(List<SResponse> result, string inputName,string dbaName)
        {
            bool DBANameProvided = false;
            bool NameProvided = false;

            NameProvided = (!string.IsNullOrEmpty(inputName) && inputName.Length > 0);
            DBANameProvided = (!string.IsNullOrEmpty(dbaName) && dbaName.Length > 0);

            inputName= GetFormattedString(inputName);
            dbaName= GetFormattedString(dbaName);

            int i = 0;
            double totalScore, nameScore, dbaScore;
            string best_name, best_dbaname;
            string curr_name, curr_dbaname;
            List<Scores> scores = new List<Scores>();

           foreach(var res in result)
            {
                totalScore = 0; nameScore=0; dbaScore = 0;
                best_name = ""; best_dbaname = "";
                curr_name = "";curr_dbaname = "";

                curr_name = GetFormattedString(res.Name);
                curr_dbaname = GetFormattedString(res.DBAName);

                if(NameProvided)
                {
                    var tuppleN = FuzzyScoreAndBestValue(curr_name, inputName);
                    nameScore = tuppleN.Item1;
                    best_name = tuppleN.Item2;
                }

                if(DBANameProvided)
                {
                    var tuppleN = FuzzyScoreAndBestValue(curr_dbaname, dbaName);
                    dbaScore = tuppleN.Item1;
                    best_dbaname = tuppleN.Item2;
                }

                if(DBANameProvided && NameProvided)
                {
                    var tot = nameWeight + dbaWeight;
                    totalScore = (double)((nameWeight/tot) * nameScore + (dbaWeight/tot)*dbaScore);
                    if(totalScore>0)
                    {
                        scores.Add(new Scores
                        {
                            index = i,
                            TotalScore = totalScore,
                            NMADDRScore = new List<double> { nameScore,dbaScore },
                            NMADDRBestMatch = new List<string> { best_name, best_dbaname}
                        }); 
                    }
                }
                else if(DBANameProvided)
                {
                    totalScore = dbaScore;
                    if(totalScore > 0)
                    {
                        scores.Add(new Scores
                        {
                            index = i,
                            TotalScore = totalScore,
                            NMADDRScore = new List<double> { dbaScore },
                            NMADDRBestMatch = new List<string> { best_dbaname }
                        });

                    }
                }
                else
                {
                    totalScore = nameScore;
                    if (totalScore > 0)
                    {
                        scores.Add(new Scores
                        {
                            index = i,
                            TotalScore = totalScore,
                            NMADDRScore = new List<double> { nameScore },
                            NMADDRBestMatch = new List<string> { best_name }
                        });
                    }
                }
                res.Score = totalScore;
                res.NSScore = nameScore;
                res.DBScore = dbaScore;
                i++;
            }

           scores = scores.OrderByDescending(s=>s.TotalScore).ToList();
           List<string> inputs = new List<string>();
           List<double> weights = new List<double>();
           
            if(!string.IsNullOrEmpty(inputName) && inputName.Length > 0)
            {
                inputs.Add(inputName);
                weights.Add(nameWeight);
            }
            if (!string.IsNullOrEmpty(dbaName) && dbaName.Length > 0)
            {
                inputs.Add(dbaName);
                weights.Add(dbaWeight);
            }

            //ApplyLSS Score

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();

            List<SResponse> finalResult = new List<SResponse>();

            foreach(var sc in scores.Where(s => s.TotalScore >=0.5).ToList())
            {
                result[sc.index].Score = sc.TotalScore;
                if(DBANameProvided)
                {
                    result[sc.index].DBScore = sc.NMADDRScore[sc.NMADDRScore.Count() - 1];                    
                }
                if(NameProvided)
                {
                    result[sc.index].NSScore = sc.NMADDRScore[0];
                }
                finalResult.Add(result[sc.index]);
            }

           return finalResult;
        }

        public List<SResponse> GetAddressSearchResult(List<SResponse> result, string address, string city, string stateCode)
        {
            string inputAddress = GetFormattedString(address);

            int i = 0;
            double totalScore;
            string best_address, current_address;
            List<Scores> scores = new List<Scores>();

            foreach(var res in result)
            {
                totalScore = 0;
                best_address = ""; current_address = "";

                current_address = GetFormattedString(res.Address);
                var tupleN = FuzzyScoreAndBestValue(current_address, inputAddress);
                totalScore = tupleN.Item1;
                best_address = tupleN.Item2;

                if(totalScore > 0)
                {
                    scores.Add(new Scores { 
                        TotalScore=totalScore,
                        NMADDRScore = new List<double> { totalScore},
                        NMADDRBestMatch = new List<string> { best_address },
                        index =i
                    });
                }
                res.Score = totalScore;
                res.ADScore = totalScore;
                i++;
            }

            scores = scores.OrderByDescending(s=>s.TotalScore).ToList();

            ApplyLCSSCore(scores, new List<string> { inputAddress }, new List<double> { 100 });

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();



            return new List<SResponse>();
        }
    
        public List<SResponse> GetNameAndAddressSearchResult(List<SResponse> result, string inputName, string dbaName, string inputAddress)
        {
            return new List<SResponse>();
        }
        
        private string GetFormattedString (string inputString)
        {
            string formattedString = "";

            if(!string.IsNullOrEmpty(inputString) && inputString.Length>0)
            {
                inputString = inputString.Trim();
                var input = inputString.ToLower().Split(' ').ToList();
                input.Sort();
                formattedString = string.Join(" ", input);
            }

            return formattedString;

        }

        private Tuple<double,string> FuzzyScoreAndBestValue(string name1, string name2)
        {
            foreach(string p in Punctuation)
            {
                name1 = name1.ToLower().Replace(p, "");
                name2 = name2.ToLower().Replace(p, "");
            }
            List<string> name2Format = new List<string>();
            name2Format = name2.Split(' ').ToList();
            name2Format.Sort();
            name2 = string.Join(" ", name2Format);

            double score = Fuzz.TokenSetRatio(name1, name2);
            double simScore = (double)(score / 100);
            string bname = name1;

            return new Tuple<double, string>(simScore, bname);

        }
        
        public void ApplyLCSSCore(List<Scores> scores, List<string> inputValues, List<double> weights)
        {
            double fuzzyRP = 0.8;
            double lcsRP = 1 - fuzzyRP;
            double sumW = weights.Sum();
            weights = weights.Select(w => w /sumW).ToList();
            double finalScore = 0;
            foreach(Scores s in scores)
            {
                int j = 0;
                finalScore = 0;
                foreach(string val in inputValues)
                {
                    string name1 = "";
                    string name2 = "";
                    foreach(string p in Punctuation)
                    {
                        name1 = s.NMADDRBestMatch[j].Replace(p, "");
                        name2 = val.ToLower().Replace(p, "");
                    }
                    double fuzzyRatio = s.NMADDRScore[j];
                    double lcsRatio = FuzzySharp.Levenshtein.GetRatio(name1, name2);

                    s.NMADDRScore[j] = (double)(fuzzyRP * fuzzyRatio + lcsRP*lcsRatio);
                    finalScore = finalScore + (double)(s.NMADDRScore[j] * weights[j]);
                    j++;
                }
                s.TotalScore = Math.Round(finalScore * 100, 2);
                s.NMADDRScore = s.NMADDRScore.Select(val => Math.Round(val * 100, 2)).ToList();
            }
        }
    }
}