using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllOdds.Models
{
    public class Odd
    {
        public double handicap_name { get; set; }
        public double total_name { get; set; }
        public long odd_id { get; set; }
        public string name { get; set; }
        public double value { get; set; }
        public double handicap { get; set; }
    }
}