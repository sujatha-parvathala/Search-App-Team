using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_App.Models
{
    public class Scores
    {
        public double TotalScore { get; set; }
        public int index { get; set; }
        public List<double> NMADDRScore { get; set; }    
        public List<string> NMADDRBestMatch { get; set; }
    }
}