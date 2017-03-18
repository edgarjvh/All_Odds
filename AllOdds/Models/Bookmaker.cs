using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllOdds.Models
{
    public class Bookmaker
    {
        public string name { get; set; }
        public int is_total { get; set; }
        public int is_handicap { get; set; }
        public List<Odd> odds { get; set; }
        public List<Total> totals { get; set; }
        public List<Handicap> handicaps { get; set; }

        public Bookmaker()
        {
            is_total = 0;
            is_handicap = 0;
            odds = new List<Odd>();
            totals = new List<Total>();
            handicaps = new List<Handicap>();
        }
    }
}