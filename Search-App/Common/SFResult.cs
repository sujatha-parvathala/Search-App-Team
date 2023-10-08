using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_App.Common
{
    public class SFResult
    {
        public int totalSize { get; set; }
        public bool done { get; set; }
        public List<SFRecord> records { get; set; }
    }
}