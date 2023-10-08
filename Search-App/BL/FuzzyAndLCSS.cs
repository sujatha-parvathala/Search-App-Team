using FuzzySharp;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_App.BL
{
    public class FuzzyAndLCSS
    {
        private readonly List<string> Punctuation;
        private readonly double nameWeight, addressWeight;

        public FuzzyAndLCSS()
        {
            Punctuation = new List<string> { ".", ",", "!", "@", "#" };
            nameWeight = 60.00;
            addressWeight = 40.00;
        }

        public List<SResponse> GetResultByApplyingSearchAlgos(SRequest request, List<SResponse> searchResult)
        {
            List<SResponse> finalResult = new List<SResponse>();

            bool NameProvided = false;
            bool AddressProvided = false;

            string name, address, city, state, postalcode;
            name = request.Name;
            address = request.Address;
            city = request.City;
            state = request.StateCode;
            postalcode = request.PostalCode;

            name = string.IsNullOrEmpty(name) ? "" : name.Trim();
            address = string.IsNullOrEmpty(address) ? "" : address.Trim();            

            //end of getting search results

            if (searchResult != null && searchResult.Any())
            {
                NameProvided = (!string.IsNullOrEmpty(name) && name.Length > 0);
                AddressProvided = (!string.IsNullOrEmpty(address) && address.Length > 0);

                if (AddressProvided && NameProvided)
                {
                    finalResult = GetNameAndAddressSearchResult(searchResult, name, address);
                }
                else if (NameProvided)
                {
                    finalResult = GetNameSearchResult(searchResult, name);
                }
                else
                {
                    finalResult = GetAddressSearchResult(searchResult, address, city, state);
                }
            }
            return finalResult;

        }

        private List<SResponse> GetNameSearchResult(List<SResponse> result, string inputName)
        {
            inputName = GetFormattedString(inputName);


            int i = 0;
            double totalScore;
            string best_name;
            string curr_name;
            List<Scores> scores = new List<Scores>();

            foreach (var res in result)
            {
                totalScore = 0;
                best_name = ""; curr_name = "";

                curr_name = GetFormattedString(res.Name);
                var tupleN = FuzzyScoreAndBestValue(curr_name, inputName);
                totalScore = tupleN.Item1;
                best_name = tupleN.Item2;

                if (totalScore > 0)
                {
                    scores.Add(new Scores
                    {
                        TotalScore = totalScore,
                        NMADDRScore = new List<double> { totalScore },
                        NMADDRBestMatch = new List<string> { best_name },
                        index = i
                    });
                }
                res.Score = totalScore;
                res.NSScore = totalScore;
                i++;
            }

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();

            ApplyLCSSCore(scores, new List<string> { inputName }, new List<double> { 100 });

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();

            List<SResponse> finalResult = new List<SResponse>();

            foreach (var sc in scores.Where(s => s.TotalScore > 50).ToList())
            {
                result[sc.index].Score = sc.TotalScore;
                result[sc.index].NSScore = sc.NMADDRScore[0];
                finalResult.Add(result[sc.index]);
            }

            return finalResult;
        }

        private List<SResponse> GetAddressSearchResult(List<SResponse> result, string address, string city, string stateCode)
        {
            string inputAddress = GetFormattedString(address);

            int i = 0;
            double totalScore;
            string best_address, current_address;
            List<Scores> scores = new List<Scores>();

            foreach (var res in result)
            {
                totalScore = 0;
                best_address = ""; current_address = "";

                current_address = GetFormattedString(res.Address);
                var tupleN = FuzzyScoreAndBestValue(current_address, inputAddress);
                totalScore = tupleN.Item1;
                best_address = tupleN.Item2;

                if (totalScore > 0)
                {
                    scores.Add(new Scores
                    {
                        TotalScore = totalScore,
                        NMADDRScore = new List<double> { totalScore },
                        NMADDRBestMatch = new List<string> { best_address },
                        index = i
                    });
                }
                res.Score = totalScore;
                res.ADScore = totalScore;
                i++;
            }

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();

            ApplyLCSSCore(scores, new List<string> { inputAddress }, new List<double> { 100 });

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();

            List<SResponse> finalResult = new List<SResponse>();

            foreach (var sc in scores.Where(s => s.TotalScore >= 50).ToList())
            {
                result[sc.index].Score = sc.TotalScore;
                result[sc.index].ADScore = sc.NMADDRScore[0];
                finalResult.Add(result[sc.index]);
            }

            return finalResult;
        }

        private List<SResponse> GetNameAndAddressSearchResult(List<SResponse> result, string inputName, string inputAddress)
        {
            List<SResponse> finalResult = new List<SResponse>();
            bool NameProvided = false;
            bool AddressProvided = false;
            inputName = GetFormattedString(inputName);
            inputAddress = GetFormattedString(inputAddress);

            NameProvided = (!string.IsNullOrEmpty(inputName) && inputName.Length > 0);
            AddressProvided = (!string.IsNullOrEmpty(inputAddress) && inputAddress.Length > 0);

            int i = 0;
            double totalScore, nameScore, addressScore;
            string best_name, best_address, curr_name, curr_address;
            List<Scores> scores = new List<Scores>();

            foreach (var res in result)
            {
                totalScore = 0; nameScore = 0; addressScore = 0;
                best_name = ""; best_address = ""; curr_address = ""; curr_name = "";

                curr_name = GetFormattedString(res.Name);
                curr_address = GetFormattedString(res.Address);

                var tupleN1 = FuzzyScoreAndBestValue(curr_name, inputName);
                nameScore = tupleN1.Item1;
                best_name = tupleN1.Item2;

                var tupleN2 = FuzzyScoreAndBestValue(curr_address, inputAddress);
                addressScore = tupleN2.Item1;
                best_address = tupleN2.Item2;

                double totalWeight = nameWeight + addressWeight;

                if ((nameWeight > 0) && (addressWeight > 0))
                {
                    totalScore = (double)(((nameWeight / totalWeight) * nameScore) + ((addressWeight / totalWeight) * addressScore));

                }
                else
                {
                    totalScore = (double)Math.Max(nameScore, addressScore);
                }
                //totalScore = (double)(((nameWeight / totalWeight) * nameScore) + ((addressWeight / totalWeight) * addressScore));
                // totalScore = (double)Math.Max(nameScore, addressScore);
                if (totalScore > 0)
                {
                    scores.Add(new Scores
                    {
                        index = i,
                        TotalScore = totalScore,
                        NMADDRScore = new List<double> { nameScore, addressScore },
                        NMADDRBestMatch = new List<string> { best_name, best_address }
                    });

                }

                res.Score = totalScore;
                res.NSScore = nameScore;
                res.ADScore = addressScore;

                i++;
            }

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();
            List<string> inputs = new List<string>();
            List<double> weights = new List<double>();

            inputs.AddRange(new List<string> { inputName, inputAddress });
            weights.AddRange(new List<double> { nameWeight, addressWeight });

            ApplyLCSSCore(scores, inputs, weights);

            scores = scores.OrderByDescending(s => s.TotalScore).ToList();

            foreach (var sc in scores.Where(s => s.TotalScore > 50).ToList())
            {
                result[sc.index].Score = sc.TotalScore;
                result[sc.index].NSScore = sc.NMADDRScore[0];
                result[sc.index].ADScore = sc.NMADDRScore[1];

                finalResult.Add(result[sc.index]);
            }

            return finalResult;
        }

        private string GetFormattedString(string inputString)
        {
            string formattedString = "";

            if (!string.IsNullOrEmpty(inputString) && inputString.Length > 0)
            {
                inputString = inputString.Trim();
                var input = inputString.ToLower().Split(' ').ToList();
                input.Sort();
                formattedString = string.Join(" ", input);
            }

            return formattedString;

        }

        private Tuple<double, string> FuzzyScoreAndBestValue(string name1, string name2)
        {
            foreach (string p in Punctuation)
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

        private void ApplyLCSSCore(List<Scores> scores, List<string> inputValues, List<double> weights)
        {
            double fuzzyRP = 0.8;
            double lcsRP = Math.Round(1 - fuzzyRP, 1);
            double sumW = weights.Sum();
            weights = weights.Select(w => w / sumW).ToList();
            double finalScore = 0;
            foreach (Scores s in scores)
            {
                int j = 0;
                finalScore = 0;
                foreach (string val in inputValues)
                {
                    string name1 = "";
                    string name2 = "";
                    foreach (string p in Punctuation)
                    {
                        name1 = s.NMADDRBestMatch[j].Replace(p, "");
                        name2 = val.ToLower().Replace(p, "");
                    }
                    double fuzzyRatio = s.NMADDRScore[j];
                    double lcsRatio = Math.Round(FuzzySharp.Levenshtein.GetRatio(name1, name2), 2);

                    s.NMADDRScore[j] = (double)(fuzzyRP * fuzzyRatio + lcsRP * lcsRatio);
                    if (weights[j] > 0)
                    {
                        finalScore = finalScore + (double)(s.NMADDRScore[j] * weights[j]);
                    }
                    else
                    {
                        finalScore = finalScore + s.NMADDRScore[j];
                    }

                    j++;
                }
                s.TotalScore = Math.Round(finalScore * 100, 2);
                s.NMADDRScore = s.NMADDRScore.Select(val => Math.Round(val * 100, 2)).ToList();
            }
        }
    }
}